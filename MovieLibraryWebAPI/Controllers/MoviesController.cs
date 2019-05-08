using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MovieLibraryWebAPI.Custom_Attributes;
using MovieLibraryWebAPI.Models;

namespace MovieLibraryWebAPI.Controllers
{
    [AllowCrossSiteAttribute]
    [RoutePrefix("api/Movies")]
    public class MoviesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Movies
        [Route("")]
        public IQueryable<Movie> GetMovies()
        {
            return db.Movies;
        }

        // GET: api/Movies/5
        [Route("{id:int}")]
        [ResponseType(typeof(Movie))]
        public IHttpActionResult GetMovie(int id)
        {
            Movie movie = db.Movies.Where(m => m.Id == id).SingleOrDefault();
            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        // GET: api/Movies/Title
        [Route("Title/{title}")]
        public IQueryable<Movie> GetMoviesByTitle(string title)
        {
            return db.Movies.Where(b => b.Genre.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        // GET: api/Movies/Genre
        [Route("Genre/{genre}")]
        public IQueryable<Movie> GetMoviesByGenre(string genre)
        {
            return db.Movies.Where(b => b.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase));
        }

        // GET: api/Movies/Director
        [Route("DirectorName/{name}")]
        public IQueryable<Movie> GetMoviesByDirectorName(string name)
        {
            return db.Movies.Where(b => b.Genre.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        // PUT: api/Movies/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMovie(int id, Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != movie.Id)
            {
                return BadRequest();
            }

            db.Entry(movie).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Movies
        [ResponseType(typeof(Movie))]
        public IHttpActionResult PostMovie(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Movies.Add(movie);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = movie.Id }, movie);
        }

        // DELETE: api/Movies/5
        [ResponseType(typeof(Movie))]
        public IHttpActionResult DeleteMovie(int id)
        {
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return NotFound();
            }

            db.Movies.Remove(movie);
            db.SaveChanges();

            return Ok(movie);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MovieExists(int id)
        {
            return db.Movies.Count(e => e.Id == id) > 0;
        }
    }
}