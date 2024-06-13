using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace comtrade.RewardedCustomer
{
    [Route("api/[controller]")]
    [ApiController]
    public class RewardedCustomerController : ControllerBase
    {
        private readonly RewardedCustomerContext _context;
        public RewardedCustomerController(RewardedCustomerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRewardedCustomers()
        {
            var customers = await _context.RewardedCustomers
                .Include(rc => rc.HomeAddress)
                .Include(rc => rc.OfficeAddress)
                .Select(rc => new RewardedCustomerDTO
                {
                    Name = rc.Name,
                    SSN = rc.SSN,
                    DOB = rc.DOB,
                    Home = new AddressDTO
                    {
                        Street = rc.HomeAddress.Street,
                        City = rc.HomeAddress.City,
                        State = rc.HomeAddress.State,
                        Zip = rc.HomeAddress.Zip
                    },
                    Office = new AddressDTO
                    {
                        Street = rc.OfficeAddress.Street,
                        City = rc.OfficeAddress.City,
                        State = rc.OfficeAddress.State,
                        Zip = rc.OfficeAddress.Zip
                    },
                    FavoriteColors = rc.FavoriteColors,
                    Age = rc.Age,
                    TimesRewarded = rc.TimesRewarded
                })
                .ToListAsync();

            return Ok(customers);
        }
    }
}
