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

    }
}
