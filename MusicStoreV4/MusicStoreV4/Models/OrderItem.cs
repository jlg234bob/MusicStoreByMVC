using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MusicStoreV4.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public int AlbumId { get; set; }

        public int AlbumCount { get; set; }

        public decimal TotalMoney { get; set; }

        public virtual Album Album { get; set; }
        public virtual Order Order { get; set; }
    }
}