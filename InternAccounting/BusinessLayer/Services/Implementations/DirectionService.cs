using AutoMapper;
using InternAccounting.BusinessLayer.DataTransferObjects.Direction;
using InternAccounting.BusinessLayer.DataTransferObjects.Intern;
using InternAccounting.BusinessLayer.Filter;
using InternAccounting.BusinessLayer.Services.Abstractions;
using InternAccounting.DataLayer.Entities;
using InternAccounting.DataLayer.Repositories.Abstractions;
using InternAccounting.DataLayer.Repositories.Implementations;

namespace InternAccounting.BusinessLayer.Services.Implementations
{
    public class DirectionService : IDirectionService
    {
        private readonly IDirectionsRepository _directionsRepository;
        private readonly IMapper _mapper;
        private readonly IInternsRepository _internsRepository;

        public DirectionService(IDirectionsRepository directionsRepository, IInternsRepository internsRepository, IMapper mapper)
        {
            _directionsRepository = directionsRepository;
            _internsRepository = internsRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<DirectionDto>> GetDirectionsAsync(FilterModel filter)
        {
            var directions = await _directionsRepository.GetFilteredDirectionsAsync(filter);
            var items = _mapper.Map<List<DirectionDto>>(directions);

            return new PaginatedList<DirectionDto>(
                items,
                directions.Count,
                directions.PageIndex,
                directions.PageSize
            );
        }

        public async Task<DirectionDetailDto> GetDirectionDetailsAsync(int id)
        {
            var direction = await _directionsRepository.GetDirectionWithInternsAsync(id);
            return new DirectionDetailDto
            {
                Id = direction.Id,
                Title = direction.Title,
                Description = direction.Description,
                SlotsCount = direction.SlotsCount,
                InternsCount = direction.Interns.Count,
                Interns = _mapper.Map<List<InternShortDto>>(direction.Interns)
            };
        }

        public async Task<DirectionDto> CreateDirectionAsync(CreateDirectionDto dto)
        {
            var direction = _mapper.Map<DirectionEntity>(dto);
            await _directionsRepository.AddDirectionAsync(direction);
            return _mapper.Map<DirectionDto>(direction);
        }

        public async Task UpdateDirectionAsync(int id, UpdateDirectionDto dto)
        {
            var direction = await _directionsRepository.GetDirectionWithInternsAsync(id);

            // Обновляем основные поля
            _mapper.Map(dto, direction);

            // Обновляем привязку стажеров
            await UpdateDirectionInternsAsync(direction, dto.InternIds);

            await _directionsRepository.UpdateDirectionAsync(direction);
        }

        private async Task UpdateDirectionInternsAsync(DirectionEntity direction, List<int> internIds)
        {
            if (direction.SlotsCount < internIds.Count)
            {
                throw new InvalidOperationException("Слоты заполнены");
            }

            // Получаем текущих и новых стажеров
            var currentInternIds = direction.Interns.Select(i => i.Id).ToList();
            var internsToAdd = internIds.Except(currentInternIds);
            var internsToRemove = currentInternIds.Except(internIds);

            // Удаляем старых
            foreach (var internId in internsToRemove)
            {
                var intern = direction.Interns.First(i => i.Id == internId);
                direction.Interns.Remove(intern);
            }

            // Добавляем новых
            foreach (var internId in internsToAdd)
            {
                var intern = await _internsRepository.GetInternWithDetailsAsync(internId);
                direction.Interns.Add(intern);
            }
        }

        public async Task DeleteDirectionAsync(int id)
        {
            if (await _directionsRepository.DirectionHasInternsAsync(id))
            {
                throw new InvalidOperationException("Нельзя удалить направление с привязанными стажерами");
            }

            await _directionsRepository.DeleteDirectionAsync(id);
        }

        public async Task<IEnumerable<DirectionDto>> GetAllDirectionsAsync()
        {
            var filter = new FilterModel { PageSize = int.MaxValue };
            var result = await GetDirectionsAsync(filter);
            return result;
        }
    }
}
