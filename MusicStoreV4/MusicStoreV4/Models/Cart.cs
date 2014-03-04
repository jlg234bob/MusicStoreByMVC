using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MusicStoreV4.Models
{
    public class Cart
    {
        public const string CartSessionKey = "CartId";
        private db db = new db();
        private HttpContextBase httpContext;

        public Cart(HttpContextBase context)
        {
            this.httpContext = context;
        }

        public string CartId
        {
            get
            {
                if (this.httpContext.Session[CartSessionKey]==null)
                {
                    if (!string.IsNullOrEmpty(this.httpContext.User.Identity.Name))
                    {
                        this.httpContext.Session[CartSessionKey] = httpContext.User.Identity.Name;
                    }
                    else
                    {
                        this.httpContext.Session[CartSessionKey] = new Guid().ToString();
                    }
                }

                return this.httpContext.Session[CartSessionKey].ToString();
            }

            private set
            {
                this.httpContext.Session[CartSessionKey] = value;
            }
        }

        public List<CartItem> CartItems
        {
            get
            {
                return db.CartItems.Where(item => item.CartId == this.CartId).ToList();
            }
        }

        /// <summary>
        /// Add album to cart
        /// </summary>
        /// <param name="AlbumId"></param>
        /// <returns>Album count of the cart item after this action</returns>
        public int AddItem(int AlbumId)
        {
            var album = db.Albums.Find(AlbumId);
            var cartItem = this.CartItems.SingleOrDefault(item => item.Album.AlbumId == AlbumId);

            if (album == null)
            {
                return 0;
            }

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartId = this.CartId,
                    AlbumId = AlbumId,
                    AlbumCount = 1
                };

                db.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.AlbumCount++;
            }

            db.SaveChanges();

            return cartItem.AlbumCount;
        }

        /// <summary>
        /// Remove album from cart
        /// </summary>
        /// <param name="cartItemId"></param>
        /// <returns>Album count of the cart item after this action</returns>
        public int RemoveItem(int cartItemId)
        {
            var cartItem = db.CartItems.Find(cartItemId);

            if (cartItem == null)
            {
                return 0;
            }

            cartItem.AlbumCount--;
            if (cartItem.AlbumCount <= 0)
            {
                cartItem.AlbumCount = 0;
                db.CartItems.Remove(cartItem);
            }

            db.SaveChanges();

            return cartItem.AlbumCount;
        }

        /// <summary>
        /// Migrate cart items to login acount
        /// </summary>
        public void MigrateLogin()
        {
            var loginName = this.httpContext.User.Identity.Name;

            if (loginName != null && loginName != this.CartId)
            {
                this.CartItems.ForEach(item => item.CartId = loginName);
            }

            db.SaveChanges();
            this.CartId = loginName;
        }

        public void FillOrderItems(Order order)
        {
            this.CartItems.ForEach(item =>
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    AlbumId = item.AlbumId,
                    AlbumCount = item.AlbumCount,
                    TotalMoney = item.Album.Price * item.AlbumCount
                };

                // Convert cart items as order items
                db.OrderItems.Add(orderItem);
                db.CartItems.Remove(item);
            });

            db.SaveChanges();
        }
    }
}