using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstDay.ViewModels
{
    public class ItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public ItemViewModel()
        {
            
        }
        public ItemViewModel(Item item)
        {
            Id = item.Id;
            Name = item.Name;
            Count = item.Count;
            CategoryId = item.CategoryId;
        }
        public Item toEntity()
        {
            return new Item { Id = Id, Name = Name, Count = Count, CategoryId = CategoryId };
        }
    }
}