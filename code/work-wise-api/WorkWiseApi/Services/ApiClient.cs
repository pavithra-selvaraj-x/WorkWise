using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient()
    {
        _httpClient = new HttpClient();
    }

    public T PostRequestAsync<T>(string apiUrl, string requestBody)
    {
        try
        {
            // Add custom header to the HttpClient request
            // _httpClient.DefaultRequestHeaders.Add(customHeaderName, customHeaderValue);

            // Convert request body to StringContent
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            // Send POST request
            var response = _httpClient.PostAsync(apiUrl, content).Result;

            // Check if request was successful
            if (response.IsSuccessStatusCode)
            {
                // Read response content as string
                // return await response.Content.ReadAsStringAsync();
                return response.Content.ReadFromJsonAsync<T>().Result;
            }
            else
            {
                // Handle unsuccessful request (e.g., log error, throw exception)
                // In this example, we're just returning null
                return default(T);
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions
            // In this example, we're just logging the exception
            Console.WriteLine($"Error: {ex.Message}");
            return default(T);
        }
    }
}
