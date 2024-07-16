using FirstDay.Handlers;
using FirstDay.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace FirstDay.Controllers
{
    public class ItemsController : Controller
    {
        //private readonly FirstDayContext _context;
        private readonly ItemHandler _itemHandler;
        private readonly CategoryHandler _categoryHandler;
        public ItemsController()
        {
            //_context = new FirstDayContext();
            _itemHandler = new ItemHandler();
            _categoryHandler= new CategoryHandler();
        }
        public async Task<ActionResult> Index()
        {
            //var itemList=new ItemList();
            //var categories= await _context.Categories.ToListAsync();
            //var categoriesVm=new List<CategoryViewModel>();
            //foreach (var category in categories)
            //{
            //    categoriesVm.Add(new CategoryViewModel(category));
            //}
            ViewBag.Categories = await _categoryHandler.GetList();


            //var items = await _context.Items.Join(_context.Categories,i=>i.CategoryId,c=>c.Id,(item,category)=>new { Item=item, CategoryName=category.Name}).ToListAsync();
            //foreach (var itemData in items)
            //{
            //    itemList.Items.Add(new ItemItem(itemData.Item) { CategoryName=itemData.CategoryName});
            //}
            var itemList =await _itemHandler.GetList();

            return View(itemList);
        }

        [HttpPost]
        public async Task<ActionResult> Index(ItemList itemList)
        {
            //IQueryable<Item> itemsQuery = _context.Items;
            //if (itemList.Search != null)
            //{
            //    itemsQuery = itemsQuery.Where(e => e.Name.Contains(itemList.Search));
            //}
            //if (itemList.CategoryId != null)
            //{
            //    itemsQuery = itemsQuery.Where(e => e.CategoryId == itemList.CategoryId);
            //}

            //var items = await itemsQuery.Join(_context.Categories, i => i.CategoryId, c => c.Id, (item, category) => new { Item = item, CategoryName = category.Name }).ToListAsync();

            //foreach (var itemData in items)
            //{
            //    itemList.Items.Add(new ItemItem(itemData.Item) { CategoryName = itemData.CategoryName });
            //}
            await _itemHandler.GetList(itemList);
            return PartialView("ItemsResult",itemList);
        }
        public async Task<ActionResult> Form(int? id)
        {
            //var categories = await _context.Categories.ToListAsync();
            //var categoriesVm = new List<CategoryViewModel>();
            //foreach (var category in categories)
            //{
            //    categoriesVm.Add(new CategoryViewModel(category));
            //}
            ViewBag.Categories = await _categoryHandler.GetList();
            if (id.HasValue)
            {
                //var item = await _context.Items.FindAsync(id);
                //var itemPM = new ItemPM(item);
                var itemPM= await _itemHandler.GetSingle(id.Value);
                return View(itemPM);
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Save(ItemPM itemPM)
        {
            if (ModelState.IsValid)
            {
                var result=await _itemHandler.Save(itemPM);
                if (result)
                {
                    return new HttpStatusCodeResult(200);
                }
                //if (itemPM.Id.HasValue)
                //{
                //    var item = await _context.Items.FindAsync(itemPM.Id);
                //    item.Name = itemPM.Name;
                //    item.Count = itemPM.Count;
                //    item.CategoryId = itemPM.CategoryId;
                //}
                //else
                //{
                //    var item = itemPM.toEntity();
                //    _context.Items.Add(item);
                //}
                //await _context.SaveChangesAsync();
                //return new HttpStatusCodeResult(200);
            }
            return new HttpStatusCodeResult(400);

        }
        
        public async Task<ActionResult> Delete(int id)
        {
            if(await _itemHandler.Delete(id))
                return new HttpStatusCodeResult(200);
            else
               return new HttpStatusCodeResult(400);
        }
    }
}