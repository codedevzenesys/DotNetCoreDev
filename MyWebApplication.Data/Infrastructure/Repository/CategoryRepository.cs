using MyWebApplication.Data.Infrastructure.IRepository;
using MyWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.Data.Infrastructure.Repository
{
    public class CategoryRepository:Repository<Category>,ICategoryRepository
    {
        private ApplicationDBContext _context;
        public CategoryRepository(ApplicationDBContext context):base(context) { 
            _context = context;
        }

        public void Update(Category category)
        {
            var categoryDB = _context.Categories.FirstOrDefault(c => c.Id == category.Id);
            if (categoryDB!=null)
            {
                categoryDB.Name = category.Name;
                categoryDB.DisplayOrder = category.DisplayOrder;
                
            }
            _context.Categories.Update(categoryDB);
        }
    }
}
