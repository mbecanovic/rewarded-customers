using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

public class SoapServiceClient
{
    private readonly HttpClient _client;

    public SoapServiceClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<string> FindPersonAsync(string personName)
    {
        var requestContent = new StringContent(
            $"<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:sam=\"http://tempuri.org\"><soapenv:Header/><soapenv:Body><sam:FindPerson><sam:name>{personName}</sam:name></sam:FindPerson></soapenv:Body></soapenv:Envelope>",
            Encoding.UTF8,
            "application/soap+xml"
        );

        var request = new HttpRequestMessage(HttpMethod.Post, "https://www.crcind.com/csp/samples/SOAP.Demo.cls")
        {
            Content = requestContent
        };

        var response = await _client.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            // Handle SOAP fault
            var soapFault = XElement.Parse(responseContent);
            var faultString = soapFault.Descendants(XName.Get("faultstring", "http://schemas.xmlsoap.org/soap/envelope/")).FirstOrDefault()?.Value;
            throw new HttpRequestException($"SOAP Fault: {faultString}");
        }

        return responseContent;
    }
}
