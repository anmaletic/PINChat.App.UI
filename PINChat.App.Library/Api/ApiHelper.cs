using System.Net.Http.Headers;

namespace PINChat.App.Library.Api;

public class ApiHelper : IApiHelper
{
    private readonly string _apiEndpoint = "https://pinchat-api.anmal.dev";
    
    private HttpClient? _apiClient;
    public HttpClient ApiClient => _apiClient!;
    
    public ApiHelper()
    {
        InitializeClient();
    }

    private void InitializeClient()
    {
        _apiClient = new HttpClient
        {
            BaseAddress = new Uri(_apiEndpoint)
        };
        _apiClient.DefaultRequestHeaders.Accept.Clear();
        _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

}