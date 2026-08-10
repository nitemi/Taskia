using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Taskia.Domain.Common;

namespace Taskia.Application.Common.Interfaces;

public interface IRepository<TEntity, TId> where TEntity : AggregateRoot<TId>
{
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}
