using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStoreV4.Models;

namespace MusicStoreV4.Controllers
{
    public class ShoppingCartController : Controller
    {
        private db db = new db();

        //
        // GET: /ShoppingCart/

        public ActionResult Index()
        {
            Cart cart = new Cart(HttpContext);

            return View(cart.CartItems);
        }

        [HttpGet]
        public ActionResult AddItem(int Id)
        {
            var album = db.Albums.Find(Id);

            return View(album);
        }

        [HttpPost]
        [ActionName("AddItem")]
        public ActionResult ConfirmAddItem(int Id)
        {
            Cart cart = new Cart(HttpContext);
            cart.AddItem(Id);

            return RedirectToAction("Index", cart.CartItems);
        }

        [HttpPost]
        public ActionResult RemoveItem(int Id)
        {
            Cart cart = new Cart(HttpContext);
            var albumCount = cart.RemoveItem(Id);
            var totalMoney = cart.CartItems.Sum(item => item.Album.Price * item.AlbumCount);
            var totalAlbum = cart.CartItems.Sum(item => item.AlbumCount);

            var data = new
            {
                CartItemId = Id,
                AlbumCount = albumCount,
                TotalMoney = totalMoney,
                TotalAlbum = totalAlbum
            };

            return Json(data);
        }

        [ChildActionOnly]
        public PartialViewResult CartSummary()
        {
            var cart = new Cart(HttpContext);

            return PartialView(cart.CartItems);
        }

        [HttpGet]
        [Authorize]
        public ActionResult CheckOut()
        {
            var order = new Order();

            return View(order);
        }

        [HttpPost]
        [Authorize]
        [ActionName("CheckOut")]
        public ActionResult PostCheckOut(FormCollection fc)
        {
            var cart = new Cart(HttpContext);           
            var order = new Order();

            if (ModelState.IsValid && TryUpdateModel(order))
            {
                order.DateCreated = DateTime.Now;
                order.AccountName = HttpContext.User.Identity.Name;

                db.Orders.Add(order);
                db.SaveChanges();
                cart.FillOrderItems(order);

                return RedirectToAction("Index", "Store");
            }
            else
            {
                return RedirectToAction("CheckOut");
            }
        }
    }
}
