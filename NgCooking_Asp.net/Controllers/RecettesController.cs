﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NgCooking_Asp.net.Models;
using System.IO;

namespace NgCooking_Asp.net.Controllers
{
    public class RecettesController : Controller
    {
        private ngCookingDBEntities db = new ngCookingDBEntities();

        public ActionResult Home()
        {
            ViewData["Recettes"] = db.Recettes.Count();
            return View( db.Recettes.ToList() );
        }
        public ActionResult Recettes()
        {
            ViewData["Recettes"] = db.Recettes.Count();
            ViewData["Model"] = db.Recettes.ToList();
            return View( db.Recettes.ToList() );
        }
        public ActionResult SearchByCalorie( int[ ] ints )
        {
            ViewData["Recettes"] = db.Recettes.Count();
            ViewData["Model"] = db.Ingredients.ToList();
            int min = ints[0];
            int max = ints[1];
            var recettes = db.Recettes.Where(x => x.calories >= min && x.calories <= max ).ToList();
            ViewData["dataByName"] = db.Recettes.First();
            return View( "Recettes", db.Recettes.First() );

        }
        [HttpPost]         
        public ActionResult Upload( FormCollection fc, HttpPostedFileBase file )
        {
            try
            {
                var ingredients = TempData["ListeIngredient"] as List<Ingredients>;
                HttpPostedFileBase photo = Request.Files["file"];
                Recettes Temp = new Recettes();
                ViewData["Categorie"] = db.Categories.Select( s => s.categoriesId ).ToList().ConvertAll( s => s.ToUpper() );
                Temp.recettesId = Request.Form["name"];
                Temp.name = Request.Form["name"].Replace('-',' ');
                Temp.isAvailable = true;                 
                Temp.preparation = Request.Form["preparation"]; 
                ingredients.ToList().ForEach( x => Temp.Ingredients.Add( db.Ingredients.Find( x.ingredientsId ) ) );
                Temp.calories = ingredients.Sum( s => s.Calories );
                Temp.creatorId = 1;

                if( photo != null && photo.ContentLength > 0 )
                {
                    var fileName = Path.GetFileName(photo.FileName);
                    var path = Path.Combine(Server.MapPath(("~/img/recettes/")), fileName);
                    photo.SaveAs( path );
                    Temp.picture = "img/recettes/" + fileName;
                }
                if( ModelState.IsValid )
                {
                    db.Recettes.Add( Temp );
                    db.SaveChanges();
                    return View( "Create" );
                }


                return View( "Create" );
            }
            catch( Exception e )
            {

                return View( e.Message );
            }
        }
        public ActionResult InsertCommentaire( string[ ] data )
        {
            ViewData["Recettes"] = db.Recettes.Count();
            ViewData["Model"] = db.Recettes.ToList();
            Comments comment = new Comments();
            comment.comment = data[2];
            comment.mark = int.Parse( data[0] );
            comment.title = data[1];
            comment.RecettesrecettesId = data[3];

            if( ModelState.IsValid )
            {
                db.Comments.Add( comment );
                db.SaveChanges();
                return Json( true );
            }
            //var recette = db.Recettes.Find( data[3] );
            return Json( true );
        }
        public ActionResult ListIngredients( string ingredients )
        {
            ViewData["Recettes"] = db.Recettes.Count();
            ViewData["Categorie"] = db.Categories.Select( s => s.categoriesId ).ToList().ConvertAll( s => s.ToUpper() );
            var ing = db.Ingredients.Where(x => x.ingredientsId == ingredients).ToList();
            List<Ingredients> test = new List<Ingredients>();
            if( TempData.ContainsKey( "ListeIngredient" ) )
            {
                var RangeIng = TempData["ListeIngredient"] as List<Ingredients>;
                foreach( var item in RangeIng )
                {
                    if( !( item.ingredientsId == ingredients ) )
                    {
                        test.Add( item );
                    }

                }
                var unique_items = new HashSet<Ingredients>(RangeIng);
                test.Add( ing.First() );
                ViewData["Ingredient"] = test;
                TempData["ListeIngredient"] = ViewData["Ingredient"];

                return View();
            }

            ViewData["Ingredient"] = ing.ToList();
            TempData["ListeIngredient"] = ing.ToList();
            return View();
        }
        public ActionResult SearchByName( string input )
        {
            ViewData["Recettes"] = db.Recettes.Count();
            ViewData["Model"] = db.Recettes.ToList();
            var recettes = db.Recettes.Where(x => x.name.ToLower() == input.ToLower()).ToList();
            ViewData["dataByName"] = recettes;
            return View( "Recettes", recettes );
        }
        public ActionResult SearchByIngredients( string input )
        {
            ViewData["Recettes"] = db.Recettes.Count();
            ViewData["Model"] = db.Recettes.ToList();
            String[] substrings = input.Split(';');
            var recettes = db.Recettes.Include(c =>c.Ingredients);
            List<Recettes> temps = new List<Recettes>();

            foreach( var item in recettes )
            {
                var ing = item.Ingredients.Select(c => c.ingredientsId).ToArray();
                if( ing.Intersect( substrings ).Any() )
                {
                    temps.Add( item );
                }
            }

            ViewData["dataByName"] = temps;
            return View( "Recettes", temps );

        }
        // GET: Recettes/Details/5
        public ActionResult Details( string id )
        {
            ViewData["Recettes"] = db.Recettes.Count();
            ViewData["Recette"] = db.Recettes.ToList();
            int somme = 0;
            ViewData["Vote"] = ListeInt();

            if( id == null )
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            }
            Recettes recettes = db.Recettes.Find(id);
            Communaute communaute = db.Communaute.Find(recettes.creatorId);
            ViewData["communaute"] = communaute.FirstName + " " + communaute.Surname;
            ViewData["Lien"] = communaute.id;

            foreach( var item1 in recettes.Comments )
            {
                somme += item1.mark;
            }

            ViewData["Somme"] = somme;
            ViewData["indice"] = recettes.Comments.Count();

            if( recettes == null )
            {
                return HttpNotFound();
            }
            return View( recettes );
        }
        public ActionResult IngredientsToCategorie( string stateid )
        {
            List<Ingredients> objcity = new List<Ingredients>();
            objcity = db.Ingredients.Where( x => x.CategoriesForeignKey == stateid ).ToList();

            SelectList obgcity = new SelectList(objcity, "ingredientsId", "Name", 0);
            return Json( obgcity );
        }
        // GET: Recettes/Create
        public ActionResult Create( string ingredients )
        {
            ViewData["Recettes"] = db.Recettes.Count();
            ViewData["Categorie"] = db.Categories.Select( s => s.categoriesId ).ToList().ConvertAll( s => s.ToUpper() );
            var ing = db.Ingredients.Where(x => x.ingredientsId == ingredients).ToList();
            List<Ingredients> test = new List<Ingredients>();
            if( TempData.ContainsKey( "ListeIngredient" ) )
            {
                var RangeIng = TempData["ListeIngredient"] as List<Ingredients>;
                foreach( var item in RangeIng )
                {
                    if( !( item.ingredientsId == ingredients ) )
                    {
                        test.Add( item );
                    }

                }
                var unique_items = new HashSet<Ingredients>(RangeIng);
                test.Add( ing.First() );
                ViewData["Ingredient"] = test;
                TempData["ListeIngredient"] = ViewData["Ingredient"];

                return View();
            }

            ViewData["Ingredient"] = ing.ToList();
            TempData["ListeIngredient"] = ing.ToList();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( [Bind( Include = "recettesId,calories,creatorId,isAvailable,name,picture,preparation" )] Recettes recettes )
        {
            ViewData["Recettes"] = db.Recettes.Count();

            if( ModelState.IsValid )
            {
                db.Recettes.Add( recettes );
                db.SaveChanges();
                return RedirectToAction( "Index" );
            }

            return View( recettes );
        }

        public List<SelectListItem> ListeInt()
        {
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add( new SelectListItem { Text = "5", Value = "5" } );
            li.Add( new SelectListItem { Text = "4", Value = "4" } );
            li.Add( new SelectListItem { Text = "3", Value = "3" } );
            li.Add( new SelectListItem { Text = "2", Value = "2" } );
            li.Add( new SelectListItem { Text = "1", Value = "1" } );
            return li;
        }

        // GET: Recettes/Edit/5
        //public ActionResult Edit( string id )
        //{
        //    if( id == null )
        //    {
        //        return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
        //    }
        //    Recettes recettes = db.Recettes.Find(id);
        //    if( recettes == null )
        //    {
        //        return HttpNotFound();
        //    }
        //    return View( recettes );
        //}

        //// POST: Recettes/Edit/5
        //// Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        //// plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit( [Bind( Include = "recettesId,calories,creatorId,isAvailable,name,picture,preparation" )] Recettes recettes )
        //{
        //    if( ModelState.IsValid )
        //    {
        //        db.Entry( recettes ).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction( "Index" );
        //    }
        //    return View( recettes );
        //}

        //// GET: Recettes/Delete/5
        //public ActionResult Delete( string id )
        //{
        //    if( id == null )
        //    {
        //        return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
        //    }
        //    Recettes recettes = db.Recettes.Find(id);
        //    if( recettes == null )
        //    {
        //        return HttpNotFound();
        //    }
        //    return View( recettes );
        //}

        //// POST: Recettes/Delete/5
        //[HttpPost, ActionName( "Delete" )]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed( string id )
        //{
        //    Recettes recettes = db.Recettes.Find(id);
        //    db.Recettes.Remove( recettes );
        //    db.SaveChanges();
        //    return RedirectToAction( "Index" );
        //}

        //protected override void Dispose( bool disposing )
        //{
        //    if( disposing )
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose( disposing );
        //}
    }
}
