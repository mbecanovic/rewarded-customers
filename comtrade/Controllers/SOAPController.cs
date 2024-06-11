using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        public SoapController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPerson(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://www.crcind.com/csp/samples/SOAP.Demo.cls?soap_method=FindPerson&id={id}");
            request.Headers.Add("Cookie", "CSPSESSIONID-SP-443-UP-csp-samples-=001000010000CdLRT6Tkdp0000_iCAlzIylIc8G_msf$ENCg--; CSPWSERVERID=00db463d2896c4250cfe0db6962adde0df59cbd9");

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var xmlContent = await response.Content.ReadAsStringAsync();
                var xmlDoc = XDocument.Parse(xmlContent);

                // Namespace-ovi u XML-u
                XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
                XNamespace tempuri = "http://tempuri.org";

                // Selektujemo odgovarajući čvor
                var personNode = xmlDoc.Descendants(soapenv + "Body").Descendants(tempuri + "FindPersonResponse").Descendants(tempuri + "FindPersonResult").FirstOrDefault();

                // Kreiramo objekat sa podacima koje smo izvukli
                var personData = new
                {
                    Name = (string)personNode.Element(tempuri + "Name"),
                    SSN = (string)personNode.Element(tempuri + "SSN"),
                    DOB = (string)personNode.Element(tempuri + "DOB"),
                    Home = new
                    {
                        Street = (string)personNode.Element(tempuri + "Home").Element(tempuri + "Street"),
                        City = (string)personNode.Element(tempuri + "Home").Element(tempuri + "City"),
                        State = (string)personNode.Element(tempuri + "Home").Element(tempuri + "State"),
                        Zip = (string)personNode.Element(tempuri + "Home").Element(tempuri + "Zip")
                    },
                    Office = new
                    {
                        Street = (string)personNode.Element(tempuri + "Office").Element(tempuri + "Street"),
                        City = (string)personNode.Element(tempuri + "Office").Element(tempuri + "City"),
                        State = (string)personNode.Element(tempuri + "Office").Element(tempuri + "State"),
                        Zip = (string)personNode.Element(tempuri + "Office").Element(tempuri + "Zip")
                    },
                    FavoriteColors = (string)personNode.Element(tempuri + "FavoriteColors").Element(tempuri + "FavoriteColorsItem"),
                    Age = (string)personNode.Element(tempuri + "Age")
                };

                // Konvertujemo objekat u JSON format
                return Ok(personData);
            }
            else
            {
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
        }
    }
}

