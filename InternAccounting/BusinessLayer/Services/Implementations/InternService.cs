using AutoMapper;
using InternAccounting.BusinessLayer.DataTransferObjects.Intern;
using InternAccounting.BusinessLayer.Filter;
using InternAccounting.BusinessLayer.Services.Abstractions;
using InternAccounting.DataLayer.Entities;
using InternAccounting.DataLayer.Repositories.Abstractions;

namespace InternAccounting.BusinessLayer.Services.Implementations
{
    public class InternService : IInternService
    {
        private readonly IInternsRepository _internsRepository;
        private readonly IMapper _mapper;

        public InternService(
            IInternsRepository internsRepository,
            IMapper mapper)
        {
            _internsRepository = internsRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<InternDto>> GetInternsAsync(InternFilterModel filter)
        {
            var interns = await _internsRepository.GetFilteredInternsAsync(filter);
            return _mapper.Map<PaginatedList<InternDto>>(interns);
        }

        public async Task<InternDetailDto> GetInternDetailsAsync(int id)
        {
            var intern = await _internsRepository.GetInternWithDetailsAsync(id);
            return _mapper.Map<InternDetailDto>(intern);
        }

        public async Task<InternDto> CreateInternAsync(CreateInternDto dto)
        {
            if (await _internsRepository.EmailExistsAsync(dto.Email))
            {
                throw new InvalidOperationException("Email уже используется");
            }

            if (!string.IsNullOrEmpty(dto.PhoneNumber) &&
                await _internsRepository.PhoneNumberExistsAsync(dto.PhoneNumber))
            {
                throw new InvalidOperationException("Номер телефона уже используется");
            }

            var intern = _mapper.Map<InternEntity>(dto);
            intern.CreatedDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

            await _internsRepository.AddInternAsync(intern);
            return _mapper.Map<InternDto>(intern);
        }

        public async Task UpdateInternAsync(int id, CreateInternDto dto)
        {
            var intern = await _internsRepository.GetInternWithDetailsAsync(id);

            if (intern.Email.Address != dto.Email && await _internsRepository.EmailExistsAsync(dto.Email))
            {
                throw new InvalidOperationException("Email уже используется");
            }

            if (!string.IsNullOrEmpty(dto.PhoneNumber) &&
                intern.PhoneNumber?.Number != dto.PhoneNumber &&
                await _internsRepository.PhoneNumberExistsAsync(dto.PhoneNumber))
            {
                throw new InvalidOperationException("Номер телефона уже используется");
            }

            _mapper.Map(dto, intern);
            await _internsRepository.UpdateInternAsync(intern);
        }

        public async Task DeleteInternAsync(int id)
        {
            await _internsRepository.DeleteInternAsync(id);
        }
    }
}
