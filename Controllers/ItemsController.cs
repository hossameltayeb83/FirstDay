using FirstDay.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FirstDay.Controllers
{
    public class ItemsController : Controller
    {
        private readonly FirstDayContext _context;
        public ItemsController()
        {
            _context = new FirstDayContext();
        }
        public async Task<ActionResult> Index(string search, int? categoryId)
        {
            var categories= await _context.Categories.ToListAsync();
            var categoriesVm=new List<CategoryViewModel>();
            foreach(var category in categories)
            {
                categoriesVm.Add(new CategoryViewModel(category));
            }
            ViewBag.Categories = categoriesVm;
            IQueryable<Item> itemsQuery=_context.Items;
            if (search != null)
            {
                itemsQuery=itemsQuery.Where(e=>e.Name.Contains(search));
            }
            if(categoryId != null)
            {
                itemsQuery=itemsQuery.Where(e=>e.CategoryId==categoryId);
            }

            var items = await itemsQuery.Join(_context.Categories,i=>i.CategoryId,c=>c.Id,(item,category)=>new { Item=item, CategoryName=category.Name}).ToListAsync();
            var itemsVm = new List<ItemViewModel>();
            foreach (var itemData in items)
            {
                itemsVm.Add(new ItemViewModel(itemData.Item) { CategoryName=itemData.CategoryName});
            }
            

            return View(itemsVm);
        }
        public async Task<ActionResult> Add()
        {
            var categories = await _context.Categories.ToListAsync();
            var categoriesVm = new List<CategoryViewModel>();
            foreach (var category in categories)
            {
                categoriesVm.Add(new CategoryViewModel(category));
            }
            ViewBag.Categories = categoriesVm;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Add(ItemViewModel itemViewModel)
        {
            if (ModelState.IsValid)
            {
                var item = itemViewModel.toEntity();
                 _context.Items.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(itemViewModel);
        }
        public async Task<ActionResult> Edit(int id)
        {
            var categories = await _context.Categories.ToListAsync();
            var categoriesVm = new List<CategoryViewModel>();
            foreach (var category in categories)
            {
                categoriesVm.Add(new CategoryViewModel(category));
            }
            ViewBag.Categories = categoriesVm;
            var item = await _context.Items.FindAsync(id);
            var itemVm= new ItemViewModel(item);
            return View(itemVm);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(ItemViewModel itemViewModel)
        {
            if(ModelState.IsValid)
            {
                var item= await _context.Items.FindAsync(itemViewModel.Id);
                item.Name=itemViewModel.Name;
                item.Count=itemViewModel.Count;
                item.CategoryId=itemViewModel.CategoryId;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(itemViewModel);
        }
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if(item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}