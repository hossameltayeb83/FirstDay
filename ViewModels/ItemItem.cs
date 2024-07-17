using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstDay.ViewModels
{
    public class ItemItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public string CategoryName { get; set; }
        public ItemItem()
        {

        }
        public ItemItem(Item item)
        {
            Id = item.Id;
            Name = item.Name;
            Count = item.Count;
            CategoryName=item.Category.Name;
        }


    }
}