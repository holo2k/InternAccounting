using InternAccounting.BusinessLayer.DataTransferObjects.Intern;
using InternAccounting.BusinessLayer.Filter;
using InternAccounting.DAL;
using InternAccounting.DataLayer.Entities;
using InternAccounting.DataLayer.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace InternAccounting.DataLayer.Repositories.Implementations
{
    public class InternsRepository : IInternsRepository
    {
        private readonly AppDbContext _context;

        public InternsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<InternEntity>> GetFilteredInternsAsync(InternFilterModel filter)
        {
            var query = _context.Interns
                .Include(i => i.Direction)
                .Include(i => i.Project)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.SearchString))
            {
                query = query.Where(i =>
                    i.FirstName.Contains(filter.SearchString) ||
                    i.LastName.Contains(filter.SearchString) ||
                    i.Email.Address.Contains(filter.SearchString));
            }

            if (filter.DirectionId.HasValue)
            {
                query = query.Where(i => i.DirectionId == filter.DirectionId);
            }

            if (filter.ProjectId.HasValue)
            {
                query = query.Where(i => i.ProjectId == filter.ProjectId);
            }

            if (!string.IsNullOrEmpty(filter.Gender))
            {
                query = query.Where(i => i.Sex == Enum.Parse<Gender>(filter.Gender));
            }

            query = filter.SortField switch
            {
                "name_desc" => query.OrderByDescending(i => i.LastName).ThenByDescending(i => i.FirstName),
                "date_asc" => query.OrderBy(i => i.CreatedDate),
                "date_desc" => query.OrderByDescending(i => i.CreatedDate),
                _ => query.OrderBy(i => i.LastName).ThenBy(i => i.FirstName), // name_asc по умолчанию
            };

            return await PaginatedList<InternEntity>.CreateAsync(query, filter.PageNumber, filter.PageSize);
        }

        public async Task<InternEntity> GetInternWithDetailsAsync(int id)
        {
            return await _context.Interns
                .Include(i => i.Direction)
                .Include(i => i.Project)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AddInternAsync(InternEntity intern)
        {
            intern.BirthDate = intern.BirthDate.ToUniversalTime();
            intern.CreatedDate = intern.CreatedDate.ToUniversalTime();

            await _context.Interns.AddAsync(intern);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateInternAsync(InternEntity intern)
        {
            intern.BirthDate = intern.BirthDate.ToUniversalTime();
            intern.CreatedDate = intern.CreatedDate.ToUniversalTime();

            _context.Interns.Update(intern);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteInternAsync(int id)
        {
            var intern = await _context.Interns.FindAsync(id);
            if (intern != null)
            {
                _context.Interns.Remove(intern);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var interns = await _context.Interns.ToListAsync();
            return interns.Any(i => i.Email.Address == email);
        }

        public async Task<bool> PhoneNumberExistsAsync(string phoneNumber)
        {
            var interns = await _context.Interns.ToListAsync();
            return interns.Any(i => i.PhoneNumber != null && i.PhoneNumber.Number == phoneNumber);
        }
    }
}
