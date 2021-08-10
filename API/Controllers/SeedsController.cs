using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedsController : ControllerBase
    {
        private readonly API.Data.APIContext _context;
        public SeedsController(API.Data.APIContext context)
        {
            this._context = context;
        }
        [HttpGet("Create")]
        public IActionResult CreateDB()
        {
            try
            {
                _context.Database.EnsureCreated();
                return Ok("Successed Created Database");
            }
            catch (Exception ex)
            {
                return Problem(ex.ToString(), "CreateDB");
            }
        }
        [HttpGet("Delete")]
        public IActionResult DeleteDB()
        {
            try
            {
                _context.Database.EnsureDeleted();
                return Ok("Successed Deleted Database");
            }
            catch (Exception ex)
            {
                return Problem(ex.ToString(), "DeleteDB");
            }
        }
    }
}
