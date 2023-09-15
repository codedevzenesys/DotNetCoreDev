using MyWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.Data.Infrastructure.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        
        void Update(Product product);

    }
}
