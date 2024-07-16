using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FirstDay.ViewModels
{
    public class ItemPM
    {
        public int? Id { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Item name must be between 3 and 50 characters")]

        public string Name { get; set; }
        [Range(0, 100000)]
        public int Count { get; set; }
        [Required(ErrorMessage ="*")]
        public int CategoryId { get; set; }
        public ItemPM()
        {

        }
        public ItemPM(Item item)
        {
            Id = item.Id;
            Name = item.Name;
            Count = item.Count;
            CategoryId = item.CategoryId;
        }
        public Item toEntity()
        {
            return new Item { Id = Id??0, Name = Name, Count = Count, CategoryId = CategoryId };
        }
    }
}