using E_Commerce.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Domain.RepositoryContracts
{
    public interface ICartRepositroy
    {
        public Task<Cart?> GetCartWithItemsAsync(Guid userId);
    }
}
