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
        public CelestialObjectController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        
        [HttpGet("{id:int}")]
        [Route("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            var data = _context.CelestialObjects.Where(x => x.Id == id).FirstOrDefault();
            if(data is null)
            {
                return new NotFoundResult();
            }
            else
            {
                data.Satellites = new List<CelestialObject>() { data };
                return new OkObjectResult(data);
            }
        }

        [HttpGet("{name}")]
        [Route("GetByName/{name}")]
        public IActionResult GetByName(string name)
        {
            var data = _context.CelestialObjects.Where(x => x.Name == name).ToList();
            if (data.Count == 0)
            {
                return new NotFoundResult();
            }
            else
            {
                foreach (var item in data)
                {
                    item.Satellites = new List<CelestialObject>() { item };
                }
                return new OkObjectResult(data);
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _context.CelestialObjects.ToList();
            if (data.Count == 0)
            {
                return new NotFoundResult();
            }
            else
            {
                foreach (var item in data)
                {
                    item.Satellites = new List<CelestialObject>() { item };
                }
                return new OkObjectResult(data);
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var data = _context.CelestialObjects.Where(x => x.Id == id).FirstOrDefault();
            if (data is null)
            {
                return new NotFoundResult();
            }
            data.Name = celestialObject.Name;
            data.OrbitalPeriod = celestialObject.OrbitalPeriod;
            data.OrbitedObjectId = celestialObject.OrbitedObjectId;
            _context.CelestialObjects.Update(data);
            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var data = _context.CelestialObjects.Where(x => x.Id == id).FirstOrDefault();
            if (data is null)
            {
                return new NotFoundResult();
            }
            data.Name = name;
            _context.CelestialObjects.Update(data);
            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var data = _context.CelestialObjects.Where(x => x.Id == id).ToList();
            if (data.Count == 0)
            {
                return new NotFoundResult();
            }
            _context.CelestialObjects.RemoveRange(data);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}
