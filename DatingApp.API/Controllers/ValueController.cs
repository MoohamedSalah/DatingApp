using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValueController : ControllerBase
    {
        private readonly DataContext _context;
        public ValueController(DataContext context)
        {
            _context = context;

        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Values.ToListAsync());
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _context.Values.FirstOrDefaultAsync(s => s.Id == id));
        }

    }
}
