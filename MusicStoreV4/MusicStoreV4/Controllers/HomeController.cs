using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStoreV4.Models;

namespace MusicStoreV4.Controllers
{
    public class HomeController : Controller
    {
        private db db = new db();

        public ActionResult Index()
        {
            ViewBag.Message = "Musci store Home page.";
            var top10SalesAlbums = GetTopSalesAlbums(10);

            return View(top10SalesAlbums);
        }

        private List<Album> GetTopSalesAlbums(int topCount)
        {
           var topSalesAlbums = db.OrderItems.GroupBy(oi => oi.Album, oi => oi.AlbumCount,
                (album, albumCount) => new
                {
                    Album=album,
                    TotalAlbumCount=albumCount.Sum()
                }).OrderByDescending(item=>item.TotalAlbumCount).Take(topCount).Select(item=>item.Album).ToList();

           if (topSalesAlbums.Count == 0)
           {
               topSalesAlbums = db.Albums.OrderBy(a => a.AlbumId).Take(topCount).ToList();
           }

           return topSalesAlbums;
        }

        public ActionResult GenreAlbums(int id)
        {
            var albums = db.Genres.Find(id).Albums;
            ViewBag.GenreName = db.Genres.Find(id).Name;

            return View("Index", albums);
        }
    }
}
