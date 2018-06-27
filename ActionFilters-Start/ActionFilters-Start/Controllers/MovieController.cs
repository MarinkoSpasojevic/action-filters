﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionFilters_Start.Entities;
using ActionFilters_Start.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ActionFilters_Start.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MovieController : Controller
    {
        private readonly MovieContext _context;

        public MovieController(MovieContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var movies = _context.Movies.ToList();

            return Ok(movies);
        }

        [HttpGet("{id}", Name = "MovieById")]
        public IActionResult Get(Guid id)
        {
            var movie = _context.Movies.SingleOrDefault(x => x.Id.Equals(id));
            if(movie == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(movie);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Movie movie)
        {
            if(movie == null)
            {
                return BadRequest("Movie object is null");
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Movies.Add(movie);
            _context.SaveChanges();

            return CreatedAtRoute("MovieById", new { id = movie.Id }, movie);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]Movie movie)
        {
            if(movie == null)
            {
                return BadRequest("Movie object is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbMovie = _context.Movies.SingleOrDefault(x => x.Id.Equals(id));
            if(dbMovie == null)
            {
                return NotFound();
            }

            dbMovie.Map(movie);

            _context.Movies.Update(dbMovie);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var dbMovie = _context.Movies.SingleOrDefault(x => x.Id.Equals(id));
            if (dbMovie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(dbMovie);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
