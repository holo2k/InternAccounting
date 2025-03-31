using InternAccounting.BusinessLayer.DataTransferObjects.Intern;
using InternAccounting.BusinessLayer.Filter;
using InternAccounting.DataLayer.Entities;

namespace InternAccounting.BusinessLayer.Services.Abstractions
{
    public interface IInternService
    {
        public Task<PaginatedList<InternDto>> GetInternsAsync(InternFilterModel filter);
        public Task<InternDetailDto> GetInternDetailsAsync(int id);
        public Task<InternDto> CreateInternAsync(CreateInternDto dto);
        public Task UpdateInternAsync(int id, CreateInternDto dto);
        public Task DeleteInternAsync(int id);
    }
}
