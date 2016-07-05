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
    public class IngredientsController : Controller
    {
        private ngCookingDBEntities db = new ngCookingDBEntities();

        // GET: Ingredients
        public ActionResult Index()
        {
            ViewData["Recettes"] = db.Recettes.Count();
            if( TempData.ContainsKey( "test" ) )
            {
                ViewData["LimiteIngredient"] = 4 + ( ( int )TempData["test"] );
            }
            else
            {
                ViewData["LimiteIngredient"] = 4;
            }

            TempData["test"] = ViewData["LimiteIngredient"];
            ViewData["Recettes"] = db.Recettes.Count();
            var ingredients = db.Ingredients.Include(i => i.Categories);

            return View( ingredients.ToList() );
        }
         
        public ActionResult Ingredients()
        {
            TempData["test"] = 0;
            ViewData["LimiteIngredient"] = 4;
            ViewData["Recettes"] = db.Recettes.Count();
            var ingredients = db.Ingredients.Include(i => i.Categories);
            return View( ingredients.ToList() );
        }

        public ActionResult SearchByName(string input)
        {
            TempData["test"] = 0;
            ViewData["LimiteIngredient"] = 4;
            var ingredients = db.Ingredients.Include(i => i.Categories).Where(x => x.ingredientsId.Contains(input));
            return View( ingredients.ToList() );
        }

        // GET: Ingredients/Details/5
        public ActionResult Details( string id )
        {
            if( id == null )
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
            Ingredients ingredients = db.Ingredients.Find(id);
            if( ingredients == null )
            {
                return HttpNotFound();
            }
            return View( ingredients );
        }

        // GET: Ingredients/Create
        public ActionResult Create()
        {
            ViewBag.CategoriesForeignKey = new SelectList( db.Categories, "categoriesId", "nameToDisplay" );
            return View();
        }

        // POST: Ingredients/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( [Bind( Include = "ingredientsId,Calories,CategoriesForeignKey,IsAvailable,Name,Picture" )] Ingredients ingredients )
        {
            if( ModelState.IsValid )
            {
                db.Ingredients.Add( ingredients );
                db.SaveChanges();
                return RedirectToAction( "Index" );
            }

            ViewBag.CategoriesForeignKey = new SelectList( db.Categories, "categoriesId", "nameToDisplay", ingredients.CategoriesForeignKey );
            return View( ingredients );
        }

        // GET: Ingredients/Edit/5
        public ActionResult Edit( string id )
        {
            if( id == null )
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
            Ingredients ingredients = db.Ingredients.Find(id);
            if( ingredients == null )
            {
                return HttpNotFound();
            }
            ViewBag.CategoriesForeignKey = new SelectList( db.Categories, "categoriesId", "nameToDisplay", ingredients.CategoriesForeignKey );
            return View( ingredients );
        }

        // POST: Ingredients/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( [Bind( Include = "ingredientsId,Calories,CategoriesForeignKey,IsAvailable,Name,Picture" )] Ingredients ingredients )
        {
            if( ModelState.IsValid )
            {
                db.Entry( ingredients ).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction( "Index" );
            }
            ViewBag.CategoriesForeignKey = new SelectList( db.Categories, "categoriesId", "nameToDisplay", ingredients.CategoriesForeignKey );
            return View( ingredients );
        }

        // GET: Ingredients/Delete/5
        public ActionResult Delete( string id )
        {
            if( id == null )
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
            Ingredients ingredients = db.Ingredients.Find(id);
            if( ingredients == null )
            {
                return HttpNotFound();
            }
            return View( ingredients );
        }

        // POST: Ingredients/Delete/5
        [HttpPost, ActionName( "Delete" )]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed( string id )
        {
            Ingredients ingredients = db.Ingredients.Find(id);
            db.Ingredients.Remove( ingredients );
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
