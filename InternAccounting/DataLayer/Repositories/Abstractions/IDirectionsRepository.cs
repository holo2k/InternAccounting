using InternAccounting.BusinessLayer.Filter;
using InternAccounting.DataLayer.Entities;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace InternAccounting.DataLayer.Repositories.Abstractions
{
    public interface IDirectionsRepository
    {
        Task<PaginatedList<DirectionEntity>> GetFilteredDirectionsAsync(FilterModel filter);
        Task<DirectionEntity> GetDirectionWithInternsAsync(int id);
        Task AddDirectionAsync(DirectionEntity direction);
        Task UpdateDirectionAsync(DirectionEntity direction);
        Task DeleteDirectionAsync(int id);
        Task<bool> DirectionHasInternsAsync(int id);
    }
}
