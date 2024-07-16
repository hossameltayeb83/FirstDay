using FirstDay.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FirstDay.Handlers
{
    public class ItemHandler
    {
        private readonly FirstDayContext _context;
        public ItemHandler()
        {
            _context = new FirstDayContext();
        }
        public async Task<ItemList> GetList()
        {
            var itemList=new ItemList();
            await GetList(itemList);
            return itemList;
        }
        
        public async Task GetList(ItemList itemList)
        {
            IQueryable<Item> itemsQuery = _context.Items;
            if (itemList.Search != null)
            {
                itemsQuery = itemsQuery.Where(e => e.Name.Contains(itemList.Search));
            }
            if (itemList.CategoryId != null)
            {
                itemsQuery = itemsQuery.Where(e => e.CategoryId == itemList.CategoryId);
            }

            var items = await itemsQuery.Join(_context.Categories, i => i.CategoryId, c => c.Id, (item, category) => new { Item = item, CategoryName = category.Name }).ToListAsync();

            foreach (var itemData in items)
            {
                itemList.Items.Add(new ItemItem(itemData.Item) { CategoryName = itemData.CategoryName });
            }
            
        }
        public async Task<ItemPM> GetSingle(int id)
        {
          var item= await _context.Items.FindAsync(id);
          return new ItemPM(item);
        }
        public async Task<bool> Save(ItemPM itemPM)
        {
            if (itemPM.Id.HasValue)
            {
                var item = await _context.Items.FindAsync(itemPM.Id);
                item.Name = itemPM.Name;
                item.Count = itemPM.Count;
                item.CategoryId = itemPM.CategoryId;
            }
            else
            {
                var item = itemPM.toEntity();
                _context.Items.Add(item);
            }
            return await _context.SaveChangesAsync()>0;
        }
        public async Task<bool> Delete(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
               return await _context.SaveChangesAsync()>0;
            }
            return false;
        }
    }
}