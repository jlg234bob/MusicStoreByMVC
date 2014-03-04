using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MusicStoreV4.Models
{
    public class Genre
    {
        [ScaffoldColumn(false)]
        public int GenreId { get; set; }

        public string Name { get; set; }

        public virtual List<Album> Albums { get; set; }
    }
}