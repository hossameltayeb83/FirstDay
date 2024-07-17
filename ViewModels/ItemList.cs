using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstDay.ViewModels
{
    public class ItemList
    {
        public string Search { get; set; }
        public int? CategoryId { get; set; }
        public SortDirection SortDirection { get; set; }
        public string SortColumn { get; set; }      
        public IList<ItemItem> Items { get; set; } = new List<ItemItem>();
    }
    public enum SortDirection
    {
        Ascending,
        Descending
    }
}