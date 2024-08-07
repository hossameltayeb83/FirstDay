﻿using FirstDay.Helpers;
using FirstDay.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
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
        public async Task<List<string>> GetSearchList(string search)
        {
            if(search == null)
            {
                return await _context.Items.Select(e=>e.Name).ToListAsync();
            }
            return await _context.Items.Where(e=>e.Name.Contains(search)).Select(e=>e.Name).ToListAsync();
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
            if(itemList.SortColumn != null)
            {
                if(typeof(Item).GetProperty(itemList.SortColumn) != null)
                {
                    itemsQuery = itemsQuery.OrderByProperty(itemList.SortColumn,itemList.SortDirection);
                }
                else if (itemList.SortColumn == "CategoryName")
                {
                    itemsQuery = itemsQuery.OrderByNavigationProperty("Name", "Category", itemList.SortDirection);
                }
            }

            var items = await itemsQuery.Include(e => e.Category).ToListAsync();

            foreach (var item in items)
            {
                itemList.Items.Add(new ItemItem(item));
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