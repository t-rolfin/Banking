using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Core.Interfaces
{
    public interface IRepository<T>
        where T : class
    {
        void Add(T entity);
    }
}
