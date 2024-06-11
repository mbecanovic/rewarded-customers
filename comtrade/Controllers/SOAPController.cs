using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Xml.Linq;

[ApiController]
[Route("[controller]")]
public class SoapController : ControllerBase
{
    private readonly SoapServiceClient _soapServiceClient;

    public SoapController(SoapServiceClient soapServiceClient)
    {
        _soapServiceClient = soapServiceClient;
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public IActionResult ParseXml()
    {
        try
        {
            string xmlString = "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"https://www.crcind.com/csp/samples/SOAP.Demo.cls?soap_method=FindPerson&id=4/\">" +
                                "<SOAP-ENV:Body>" +
                                "<FindPersonResponse>" +
                                "<FindPersonResult>" +
                                "<Name>Baker,Marvin Z.</Name>" +
                                "<SSN>198-22-7709</SSN>" +
                                "<DOB>1960-02-29</DOB>" +
                                "<Home>" +
                                "<Street>1243 First Blvd</Street>" +
                                "<City>Queensbury</City>" +
                                "<State>NV</State>" +
                                "<Zip>26930</Zip>" +
                                "</Home>" +
                                "<Office>" +
                                "<Street>3717 Ash Drive</Street>" +
                                "<City>Zanesville</City>" +
                                "<State>NY</State>" +
                                "<Zip>19361</Zip>" +
                                "</Office>" +
                                "<FavoriteColors>" +
                                "<FavoriteColorsItem>Purple</FavoriteColorsItem>" +
                                "</FavoriteColors>" +
                                "<Age>64</Age>" +
                                "</FindPersonResult>" +
                                "</FindPersonResponse>" +
                                "</SOAP-ENV:Body>" +
                                "</SOAP-ENV:Envelope>";

            // Parsiranje XML dokumenta
            XDocument doc = XDocument.Parse(xmlString);
            XNamespace ns = "https://www.crcind.com/csp/samples/SOAP.Demo.cls?soap_method=FindPerson&id=4";

            // Pronalaženje FindPersonResult elementa
            var findPersonResult = doc.Descendants(ns + "FindPersonResult").FirstOrDefault();

            // Izvlačenje vrednosti iz FindPersonResult elementa
            string name = findPersonResult?.Element("Name")?.Value;
            string ssn = findPersonResult?.Element("SSN")?.Value;
            string dob = findPersonResult?.Element("DOB")?.Value;
            string homeStreet = findPersonResult?.Element("Home")?.Element("Street")?.Value;
            string homeCity = findPersonResult?.Element("Home")?.Element("City")?.Value;
            string homeState = findPersonResult?.Element("Home")?.Element("State")?.Value;
            string homeZip = findPersonResult?.Element("Home")?.Element("Zip")?.Value;
            string officeStreet = findPersonResult?.Element("Office")?.Element("Street")?.Value;
            string officeCity = findPersonResult?.Element("Office")?.Element("City")?.Value;
            string officeState = findPersonResult?.Element("Office")?.Element("State")?.Value;
            string officeZip = findPersonResult?.Element("Office")?.Element("Zip")?.Value;
            string favoriteColor = findPersonResult?.Element("FavoriteColors")?.Element("FavoriteColorsItem")?.Value;
            string age = findPersonResult?.Element("Age")?.Value;

            // Možete dalje obraditi izvučene vrednosti pre nego što ih vratite

            return Ok(new
            {
                Name = name,
                SSN = ssn,
                DOB = dob,
                Home = new
                {
                    Street = homeStreet,
                    City = homeCity,
                    State = homeState,
                    Zip = homeZip
                },
                Office = new
                {
                    Street = officeStreet,
                    City = officeCity,
                    State = officeState,
                    Zip = officeZip
                },
                FavoriteColor = favoriteColor,
                Age = age
            });
        }
        catch (Exception ex)
        {
            // Logovanje greške
            // Možete dodati svoju logiku za logovanje grešaka ovde

            return StatusCode(500, "Internal server error");
        }
    }
}
