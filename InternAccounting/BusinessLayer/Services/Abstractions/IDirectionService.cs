using InternAccounting.BusinessLayer.DataTransferObjects.Direction;
using InternAccounting.BusinessLayer.Filter;
using InternAccounting.DataLayer.Entities;

namespace InternAccounting.BusinessLayer.Services.Abstractions
{
    public interface IDirectionService
    {
        public Task<PaginatedList<DirectionDto>> GetDirectionsAsync(FilterModel filter);
        public Task<DirectionDetailDto> GetDirectionDetailsAsync(int id);
        public Task<DirectionDto> CreateDirectionAsync(CreateDirectionDto dto);
        public Task UpdateDirectionAsync(int id, UpdateDirectionDto dto);
        public Task DeleteDirectionAsync(int id);
        public Task<IEnumerable<DirectionDto>> GetAllDirectionsAsync();
    }
}
