using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStoreV4.Models;

namespace MusicStoreV4.Controllers
{
    public class StoreController : Controller
    {
        private db db = new db();

        //
        // GET: /Store/

        public ActionResult Index()
        {
            var albums = db.Albums.ToList();
            return View(albums);
        }

        //
        // GET: /Store/Details/5

        public ActionResult Details(int id = 0)
        {
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        public ActionResult GenreAlbums(int id)
        {
            var albums = db.Genres.Find(id).Albums;
            ViewBag.GenreName = db.Genres.Find(id).Name;

            return View("Index", albums);
        }

        public PartialViewResult GenreMenu()
        {
            var genres = db.Genres;

            return PartialView(genres);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}