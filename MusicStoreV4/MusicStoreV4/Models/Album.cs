using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MusicStoreV4.Models
{
    public class Album
    {
        [ScaffoldColumn(false)]
        public int AlbumId { get; set; }

        public int ArtistId { get; set; }

        public int GenreId { get; set; }

        [Required(ErrorMessage = "Album Title is required")]
        public string Title { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Album Price is required")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Album about Url is required")]
        public string AlbumArtUrl { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual Genre Genre { get; set; }
    }
}