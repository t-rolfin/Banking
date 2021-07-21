using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.Core.Interfaces
{
    public interface IAsyncRepository<T>
    {
        Task<bool> CreateAsync(T entity, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken);
        Task<T> GetByIdAsync(int id);
    }
}
