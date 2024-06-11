using comtrade.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace comtrade.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customer = await _context.Customers.ToListAsync();
            return Ok(customer);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomers(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return BadRequest("Customer not found");
            }
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomers(Customer customer)
        {
            _context.Customers.Add(customer);
            //not saving the change, we use await

            await _context.SaveChangesAsync();

            return Ok(await _context.Customers.ToListAsync());
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomers(Customer updatedCustomer)
        {
            var dbcustomer = await _context.Customers.FindAsync(updatedCustomer.Id);
            if (dbcustomer == null)
            {
                return BadRequest("Customer not found");
            }
            dbcustomer.ime = updatedCustomer.ime;
            dbcustomer.email = updatedCustomer.email;

            await _context.SaveChangesAsync();

            return Ok(await _context.Customers.ToListAsync());
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCustomers(int? id) 
        {
            var dbcustomer = await _context.Customers.FindAsync(id);
            if (dbcustomer == null)
            {
                return BadRequest("Customer not found");
            }
            _context.Customers.Remove(dbcustomer);

            await _context.SaveChangesAsync();

            return Ok(await _context.Customers.ToListAsync());
        }

    }
}
