using System.Text;

public class SoapServiceClient
{
    private readonly HttpClient _client;

    public SoapServiceClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<string> FindPersonAsync(string personName)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://www.crcind.com/csp/samples/SOAP.Demo.cls")
        {
            Content = new StringContent($"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:sam=\"http://tempuri.org\"><soap:Header/><soap:Body><sam:FindPerson><sam:name>{personName}</sam:name></sam:FindPerson></soap:Body></soap:Envelope>", Encoding.UTF8, "application/soap+xml")
            
        };
        Console.WriteLine(await request.Content.ReadAsStringAsync());
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
