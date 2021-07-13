using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var result = _context.CelestialObjects.Find(id);
            if (result == null)
            {
                return NotFound();
            }

            result.Satellites = _context.CelestialObjects.Where(p => p.Id == result.OrbitedObjectId).ToList();

            return Ok(result);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var result = _context.CelestialObjects.Where(p => p.Name == name);
            if (result == null || result.Count() == 0)
            {
                return NotFound();
            }

            foreach (var item in result)
            {
                item.Satellites = _context.CelestialObjects.Where(p => p.OrbitedObjectId == item.Id).ToList();
            }

            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _context.CelestialObjects;

            foreach (var item in result)
            {
                item.Satellites = result.Where(p => p.OrbitedObjectId == item.Id).ToList();
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject item)
        {
            _context.CelestialObjects.Add(item);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject item)
        {
            var existing = _context.CelestialObjects.Find(id);
            if(existing==null)
            {
                return NotFound();
            }

            existing.Name = item.Name;
            existing.OrbitalPeriod = item.OrbitalPeriod;
            existing.OrbitedObjectId = item.OrbitedObjectId;

            _context.CelestialObjects.Update(existing);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var existing = _context.CelestialObjects.Find(id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = name;          
            _context.CelestialObjects.Update(existing);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _context.CelestialObjects.Where(p => p.Id == id || p.OrbitedObjectId == id);
            if (existing == null||existing.Count()==0)
            {
                return NotFound();
            }


            _context.CelestialObjects.RemoveRange(existing);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
