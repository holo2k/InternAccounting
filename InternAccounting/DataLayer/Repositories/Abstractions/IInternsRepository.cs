using InternAccounting.BusinessLayer.Filter;
using InternAccounting.DataLayer.Entities;

namespace InternAccounting.DataLayer.Repositories.Abstractions
{
    public interface IInternsRepository
    {
        Task<PaginatedList<InternEntity>> GetFilteredInternsAsync(InternFilterModel filter);
        Task<InternEntity> GetInternWithDetailsAsync(int id);
        Task AddInternAsync(InternEntity intern);
        Task UpdateInternAsync(InternEntity intern);
        Task DeleteInternAsync(int id);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> PhoneNumberExistsAsync(string phoneNumber);
    }
}
