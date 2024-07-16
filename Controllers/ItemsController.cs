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
        private readonly ItemHandler _itemHandler;
        private readonly CategoryHandler _categoryHandler;
        public ItemsController()
        {
            _itemHandler = new ItemHandler();
            _categoryHandler= new CategoryHandler();
        }
        public async Task<ActionResult> Index()
        {
            ViewBag.Categories = await _categoryHandler.GetList();
            var itemList= new ItemList();
            await _itemHandler.GetList(itemList);
            return View(itemList);
        }

        [HttpPost]
        public async Task<ActionResult> Index(ItemList itemList)
        {
            await _itemHandler.GetList(itemList);
            return PartialView("ItemsResult",itemList);
        }
        public async Task<ActionResult> Form(int? id)
        {
            ViewBag.Categories = await _categoryHandler.GetList();
            if (id.HasValue)
            {
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