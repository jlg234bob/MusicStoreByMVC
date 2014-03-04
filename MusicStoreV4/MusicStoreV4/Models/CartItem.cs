using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MusicStoreV4.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }

        public string CartId { get; set; }

        public int AlbumId { get; set; }

        public int AlbumCount { get; set; }

        public virtual Album Album { get; set; }
    }
}