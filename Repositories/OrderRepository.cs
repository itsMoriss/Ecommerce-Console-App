using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class OrderRepository
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://localhost:3000/orders"; // JSON Server URL

    public OrderRepository()
    {
        _httpClient = new HttpClient();
    }

    // Create a new order asynchronously
    public async Task<Order> CreateAsync(Order order)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/orders", order);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Order>(responseBody);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error creating order: {ex.Message}");
            return null;
        }
    }

    // Retrieve an order by its ID asynchronously
    public async Task<List<Order>> GetAllOrdersAsync()
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{BaseUrl}/orders");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Order>>(responseBody);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error retrieving orders: {ex.Message}");
            return new List<Order>();
        }
    }

    // Update an order asynchronously
    public async Task<Order> UpdateAsync(int id, Order order)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/orders/{id}", order);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Order>(responseBody);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error updating order: {ex.Message}");
            return null;
        }
    }

    // Delete an order by its ID asynchronously
    public async Task DeleteAsync(int id)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{BaseUrl}/orders/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error deleting order: {ex.Message}");
        }
    }
}

