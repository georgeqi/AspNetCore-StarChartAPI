using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CelestialObjectController(ApplicationDbContext  context)
        {
            _context = context;
        }

       [HttpGet("{id:int}")]       
        public IActionResult GetById(int id)
        {
            var result = _context.CelestialObjects.Find(id);
            if(result==null)
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
            if (result == null||result.Count()==0)
            {
                return NotFound();
            }

            foreach(var item in result)
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
    }
}
