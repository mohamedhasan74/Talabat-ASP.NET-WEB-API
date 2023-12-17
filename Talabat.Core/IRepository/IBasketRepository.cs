using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.IRepository
{
    public interface IBasketRepository
    {
        Task<bool> DeleteBasketAsync(string id);
        Task<CustomerBasket?> UpdateOrCreateBasketAsync(CustomerBasket basket);
        Task<CustomerBasket?> GetBasketAsync(string id);

    }
}
