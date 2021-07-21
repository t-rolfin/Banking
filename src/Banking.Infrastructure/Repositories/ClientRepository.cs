using Banking.Core.Entities;
using Banking.Core.Interfaces;
using Banking.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {

        private readonly ClientContext _context;

        public ClientRepository(ClientContext context)
        {
            _context = context;
        }


        public async Task<bool> CreateAsync(Client entity, CancellationToken cancellationToken)
        {
            try
            {
                if (CheckIfClientExistsByCNP(entity.CNP))
                    throw new ClientAlreadyExistsException();

                if (entity is null || entity == default)
                    throw new ArgumentNullException();

                await _context.AddAsync(entity);

                var response = await _context.SaveChangesAsync(cancellationToken);

                return response > 0 ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> UpdateAsync(Client entity, CancellationToken cancellationToken)
        {
            try
            {
                if (entity is null || entity == default)
                    throw new ArgumentNullException();

                _context.Entry(entity).State = EntityState.Modified;

                var response = await _context.SaveChangesAsync(cancellationToken);

                return response > 1 ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Client> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException();

            var client = await _context.Clients.FindAsync(id);
            return client;
        }


        private bool CheckIfClientExistsByCNP(string cnp)
        {
            return _context.Clients.Any(x => x.CNP == cnp);
        }
    }
}
