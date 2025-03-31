using InternAccounting.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternAccounting.DataLayer
{
    public interface IAppDbContext
    {
        DbSet<InternEntity> Interns { get; }
        DbSet<DirectionEntity> Directions { get; }
        DbSet<ProjectEntity> Projects { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
