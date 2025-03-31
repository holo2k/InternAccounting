using InternAccounting.BusinessLayer.Filter;
using InternAccounting.DAL;
using InternAccounting.DataLayer.Entities;
using InternAccounting.DataLayer.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace InternAccounting.DataLayer.Repositories.Implementations
{
    public class DirectionsRepository : IDirectionsRepository
    {
        private readonly AppDbContext _context;

        public DirectionsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<DirectionEntity>> GetFilteredDirectionsAsync(FilterModel filter)
        {
            var query = _context.Directions
                .Include(d => d.Interns)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.SearchString))
            {
                query = query.Where(d => d.Title.Contains(filter.SearchString) ||
                                    d.Description.Contains(filter.SearchString));
            }

            query = filter.SortField switch
            {
                "title_desc" => query.OrderByDescending(d => d.Title),
                "interns_asc" => query.OrderBy(d => d.Interns.Count),
                "interns_desc" => query.OrderByDescending(d => d.Interns.Count),
                _ => query.OrderBy(d => d.Title), // title_asc по умолчанию
            };

            return await PaginatedList<DirectionEntity>.CreateAsync(query, filter.PageNumber, filter.PageSize);
        }

        public async Task<DirectionEntity> GetDirectionWithInternsAsync(int id)
        {
            return await _context.Directions
                .Include(d => d.Interns)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AddDirectionAsync(DirectionEntity direction)
        {
            await _context.Directions.AddAsync(direction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDirectionAsync(DirectionEntity direction)
        {
            _context.Directions.Update(direction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDirectionAsync(int id)
        {
            var direction = await _context.Directions.FindAsync(id);
            if (direction != null)
            {
                _context.Directions.Remove(direction);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> DirectionHasInternsAsync(int id)
        {
            return await _context.Directions
                .AnyAsync(d => d.Id == id && d.Interns.Any());
        }
    }

}
