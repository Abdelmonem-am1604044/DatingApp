using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp_API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{   
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _context;

        public ValuesController(DataContext context)
        {
            _context = context;
            
        }

        // GET api/values
        [HttpGet]
        public async Task <IActionResult> GetValues()
        {
            var values = await _context.Values.ToListAsync();

            Console.WriteLine(values);

            return Ok(values);
        }
        
        [AllowAnonymous]
        // GET api/values/5
        [HttpGet("{id}")]
        public async Task <IActionResult> GetValue(int id)
        {
            var value = await _context.Values.FirstOrDefaultAsync(result => result.Id == id);

            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}