﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NgCooking_Asp.net.Models;

namespace NgCooking_Asp.net.Controllers
{
    public class CategoriesController : Controller
    {
        private ngCookingDBEntities db = new ngCookingDBEntities();

        // GET: Categories
        public ActionResult Index()
        {
            ViewData["Recettes"] = db.Recettes.Count();
            return View( db.Categories.ToList() );
        }
        // GET: Categories/Details/5
        public ActionResult Details( string id )
        {
            if( id == null )
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
            Categories categories = db.Categories.Find(id);
            if( categories == null )
            {
                return HttpNotFound();
            }
            return View( categories );
        }
        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Categories/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( [Bind( Include = "categoriesId,nameToDisplay" )] Categories categories )
        {
            if( ModelState.IsValid )
            {
                db.Categories.Add( categories );
                db.SaveChanges();
                return RedirectToAction( "Index" );
            }

            return View( categories );
        }
        // GET: Categories/Edit/5
        public ActionResult Edit( string id )
        {
            if( id == null )
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
            Categories categories = db.Categories.Find(id);
            if( categories == null )
            {
                return HttpNotFound();
            }
            return View( categories );
        }

        // POST: Categories/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( [Bind( Include = "categoriesId,nameToDisplay" )] Categories categories )
        {
            if( ModelState.IsValid )
            {
                db.Entry( categories ).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction( "Index" );
            }
            return View( categories );
        }
        // GET: Categories/Delete/5
        public ActionResult Delete( string id )
        {
            if( id == null )
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
            Categories categories = db.Categories.Find(id);
            if( categories == null )
            {
                return HttpNotFound();
            }
            return View( categories );
        }
        // POST: Categories/Delete/5
        [HttpPost, ActionName( "Delete" )]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed( string id )
        {
            Categories categories = db.Categories.Find(id);
            db.Categories.Remove( categories );
            db.SaveChanges();
            return RedirectToAction( "Index" );
        }
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                db.Dispose();
            }
            base.Dispose( disposing );
        }
    }
}
