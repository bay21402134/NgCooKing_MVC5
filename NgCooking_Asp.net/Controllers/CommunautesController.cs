using System;
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
    public class CommunautesController : Controller
    {
        private ngCookingDBEntities db = new ngCookingDBEntities();


        // GET: Communautes
        public ActionResult Index()
        {

            ViewData["OrderBy"] = SelectCommunaute();
            ViewData["Recettes"] = db.Recettes.Count();

            if( TempData.ContainsKey( "test" ) )
            {
                ViewData["LimitCommunaute"] = 4 + ( ( int )TempData["test"] );
            }
            else
            {
                ViewData["LimitCommunaute"] = 8;
            }

            TempData["test"] = ViewData["LimitCommunaute"];
            return View( db.Communaute.ToList() );
        }

        public ActionResult Communautes()
        {
            ViewData["OrderBy"] = SelectCommunaute();
            ViewData["LimitCommunaute"] = 8;
            return View( "Communautes", db.Communaute.ToList() );
        }

        // GET: Communautes/Details/5
        public ActionResult Details( int? id )
        {
            ViewData["Recettes"] = db.Recettes.Count();

            if( id == null )
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
            Communaute communaute = db.Communaute.Find(id);

            ViewData["RecettePerso"] = db.Recettes.Where( x => x.creatorId == communaute.id ).ToList();


            if( communaute == null )
            {
                return HttpNotFound();
            }
            return View( communaute );
        }

        // GET: Communautes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Communautes/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( [Bind( Include = "id,Bio,Birth,City,Email,FirstName,Level,Password,Picture,Surname" )] Communaute communaute )
        {
            if( ModelState.IsValid )
            {
                db.Communaute.Add( communaute );
                db.SaveChanges();
                return RedirectToAction( "Index" );
            }

            return View( communaute );
        }

        // GET: Communautes/Edit/5
        public ActionResult Edit( int? id )
        {
            if( id == null )
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
            Communaute communaute = db.Communaute.Find(id);
            if( communaute == null )
            {
                return HttpNotFound();
            }
            return View( communaute );
        }

        // POST: Communautes/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( [Bind( Include = "id,Bio,Birth,City,Email,FirstName,Level,Password,Picture,Surname" )] Communaute communaute )
        {
            if( ModelState.IsValid )
            {
                db.Entry( communaute ).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction( "Index" );
            }
            return View( communaute );
        }

        // GET: Communautes/Delete/5
        public ActionResult Delete( int? id )
        {
            if( id == null )
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
            Communaute communaute = db.Communaute.Find(id);
            if( communaute == null )
            {
                return HttpNotFound();
            }
            return View( communaute );
        }

        // POST: Communautes/Delete/5
        [HttpPost, ActionName( "Delete" )]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed( int id )
        {
            Communaute communaute = db.Communaute.Find(id);
            db.Communaute.Remove( communaute );
            db.SaveChanges();
            return RedirectToAction( "Index" );
        }

        public List<SelectListItem> SelectCommunaute()
        {
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add( new SelectListItem { Text = "Ordre alphabétique (A->Z)", Value = "FirstName" } );
            li.Add( new SelectListItem { Text = "Ordre alphabétique (Z->A)", Value = "-FirstName" } );
            li.Add( new SelectListItem { Text = "Les mieux notés d'abord", Value = "2" } );
            li.Add( new SelectListItem { Text = "Les moins bien notés d'abord", Value = "3" } );
            li.Add( new SelectListItem { Text = "Les plus productifs d'abord", Value = "4" } );
            li.Add( new SelectListItem { Text = "Les moins productifs d'abord", Value = "5" } );

            return li;
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
