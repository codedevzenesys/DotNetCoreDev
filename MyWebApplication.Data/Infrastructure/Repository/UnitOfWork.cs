using MyWebApplication.Data.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.Data.Infrastructure.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private ApplicationDBContext _context;
        public ICategoryRepository Category { get; private set; }

        public IProductRepository Product { get; private set; }

        public UnitOfWork(ApplicationDBContext context)
        {
            _context = context;
            Category= new CategoryRepository(context);
            Product = new ProductRepository(context);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
