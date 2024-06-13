using comtrade.RewardedCustomer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace comtrade.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoapController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly RewardedCustomerContext _context;

        public SoapController(HttpClient httpClient, RewardedCustomerContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPerson(int id, string agentId)
        {
            // Provera i praćenje poziva API-ja
            var apiUsage = _context.ApiUsages.FirstOrDefault(a => a.AgentId == agentId);
            if (apiUsage != null)
            {
                if (apiUsage.CallCount >= 5 && apiUsage.LastCallTime.Date == DateTime.Today)
                {
                    return StatusCode(429, "Daily limit of 5 customers exceeded. Please try again tomorrow.");
                }

                if (apiUsage.LastCallTime.Date != DateTime.Today)
                {
                    apiUsage.CallCount = 0; // Resetujemo broj poziva svakog dana
                }

                apiUsage.CallCount += 1;
                apiUsage.LastCallTime = DateTime.Now;
                _context.ApiUsages.Update(apiUsage);
            }
            else
            {
                apiUsage = new ApiUsage
                {
                    AgentId = agentId,
                    CallCount = 1,
                    LastCallTime = DateTime.Now
                };
                _context.ApiUsages.Add(apiUsage);
            }

            await _context.SaveChangesAsync();

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://www.crcind.com/csp/samples/SOAP.Demo.cls?soap_method=FindPerson&id={id}");
            request.Headers.Add("Cookie", "CSPSESSIONID-SP-443-UP-csp-samples-=001000010000CdLRT6Tkdp0000_iCAlzIylIc8G_msf$ENCg--; CSPWSERVERID=00db463d2896c4250cfe0db6962adde0df59cbd9");

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var xmlContent = await response.Content.ReadAsStringAsync();
                var xmlDoc = XDocument.Parse(xmlContent);

                XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
                XNamespace tempuri = "http://tempuri.org";

                var personNode = xmlDoc.Descendants(soapenv + "Body")
                                        .Descendants(tempuri + "FindPersonResponse")
                                        .Descendants(tempuri + "FindPersonResult")
                                        .FirstOrDefault();

                if (personNode == null)
                {
                    return NotFound("Person not found in the SOAP response.");
                }

                var personData = new
                {
                    Id = (int?)personNode.Element(tempuri + "ID") ?? 0,
                    Name = (string)personNode.Element(tempuri + "Name"),
                    SSN = (string)personNode.Element(tempuri + "SSN"),
                    DOB = (string)personNode.Element(tempuri + "DOB"),
                    Home = new
                    {
                        Id = (int?)personNode.Element(tempuri + "Home")?.Element(tempuri + "ID") ?? 0,
                        Street = (string)personNode.Element(tempuri + "Home")?.Element(tempuri + "Street"),
                        City = (string)personNode.Element(tempuri + "Home")?.Element(tempuri + "City"),
                        State = (string)personNode.Element(tempuri + "Home")?.Element(tempuri + "State"),
                        Zip = (string)personNode.Element(tempuri + "Home")?.Element(tempuri + "Zip")
                    },
                    Office = new
                    {
                        Id = (int?)personNode.Element(tempuri + "Office")?.Element(tempuri + "ID") ?? 0,
                        Street = (string)personNode.Element(tempuri + "Office")?.Element(tempuri + "Street"),
                        City = (string)personNode.Element(tempuri + "Office")?.Element(tempuri + "City"),
                        State = (string)personNode.Element(tempuri + "Office")?.Element(tempuri + "State"),
                        Zip = (string)personNode.Element(tempuri + "Office")?.Element(tempuri + "Zip")
                    },
                    FavoriteColors = (string)personNode.Element(tempuri + "FavoriteColors")?.Element(tempuri + "FavoriteColorsItem"),
                    Age = (string)personNode.Element(tempuri + "Age")
                };

                var existingCustomer = _context.RewardedCustomers.FirstOrDefault(rc => rc.SSN == personData.SSN);
                if (existingCustomer != null)
                {
                    existingCustomer.TimesRewarded += 1;
                    _context.RewardedCustomers.Update(existingCustomer);
                    await _context.SaveChangesAsync();

                    return Ok(new
                    {
                        Message = "Customer has already been rewarded. TimesRewarded incremented.",
                        existingCustomer.Id,
                        existingCustomer.Name,
                        existingCustomer.SSN,
                        existingCustomer.DOB,
                        HomeAddressId = existingCustomer.HomeAddressId,
                        OfficeAddressId = existingCustomer.OfficeAddressId,
                        existingCustomer.FavoriteColors,
                        existingCustomer.Age,
                        existingCustomer.TimesRewarded
                    });
                }

                var homeAddress = new Address
                {
                    Id = personData.Home.Id,
                    Street = personData.Home.Street,
                    City = personData.Home.City,
                    State = personData.Home.State,
                    Zip = personData.Home.Zip
                };

                var officeAddress = new Address
                {
                    Id = personData.Office.Id,
                    Street = personData.Office.Street,
                    City = personData.Office.City,
                    State = personData.Office.State,
                    Zip = personData.Office.Zip
                };

                _context.Addresses.Add(homeAddress);
                _context.Addresses.Add(officeAddress);
                await _context.SaveChangesAsync();

                var customer = new RewardedCustomers
                {
                    Id = personData.Id,
                    Name = personData.Name,
                    SSN = personData.SSN,
                    DOB = personData.DOB,
                    HomeAddressId = homeAddress.Id,
                    OfficeAddressId = officeAddress.Id,
                    FavoriteColors = personData.FavoriteColors,
                    Age = personData.Age,
                    TimesRewarded = 1
                };

                _context.RewardedCustomers.Add(customer);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    customer.Id,
                    customer.Name,
                    customer.SSN,
                    customer.DOB,
                    HomeAddressId = homeAddress.Id,
                    HomeAddress = new
                    {
                        homeAddress.Id,
                        homeAddress.Street,
                        homeAddress.City,
                        homeAddress.State,
                        homeAddress.Zip
                    },
                    OfficeAddressId = officeAddress.Id,
                    OfficeAddress = new
                    {
                        officeAddress.Id,
                        officeAddress.Street,
                        officeAddress.City,
                        officeAddress.State,
                        officeAddress.Zip
                    },
                    customer.FavoriteColors,
                    customer.Age,
                    customer.TimesRewarded
                });
            }
            else
            {
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
        }

        // Pomoćna metoda za dobijanje SSN-a osobe prema ID-u
        private async Task<string> GetPersonSSN(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://www.crcind.com/csp/samples/SOAP.Demo.cls?soap_method=FindPerson&id={id}");
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var xmlContent = await response.Content.ReadAsStringAsync();
                var xmlDoc = XDocument.Parse(xmlContent);

                XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
                XNamespace tempuri = "http://tempuri.org";

                var personNode = xmlDoc.Descendants(soapenv + "Body")
                                        .Descendants(tempuri + "FindPersonResponse")
                                        .Descendants(tempuri + "FindPersonResult")
                                        .FirstOrDefault();

                if (personNode != null)
                {
                    return (string)personNode.Element(tempuri + "SSN");
                }
            }
            return null;
        }

    }
}
