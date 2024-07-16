using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstDay.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CategoryViewModel(Category category)
        {
            Id = category.Id;
            Name = category.Name;
        }
    }
}