using FirstDay.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FirstDay.Handlers
{
    public class CategoryHandler
    {
        private readonly FirstDayContext _context;
        public CategoryHandler()
        {
            _context = new FirstDayContext();
        }
        public async Task<List<CategoryViewModel>> GetList()
        {
            var categories = await _context.Categories.ToListAsync();
            var categoriesVm = new List<CategoryViewModel>();
            foreach (var category in categories)
            {
                categoriesVm.Add(new CategoryViewModel(category));
            }
            return categoriesVm;
        }
    }
}