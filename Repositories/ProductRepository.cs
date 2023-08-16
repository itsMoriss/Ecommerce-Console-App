using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Net;


public class ProductRepository
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://localhost:3000/products"; // JSON Server URL

    public ProductRepository()
    {
        _httpClient = new HttpClient();
    }

    // Create a new product asynchronously
    public async Task<Product> CreateAsync(Product product)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(BaseUrl, product);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Product>(responseBody);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error creating product: {ex.Message}");
            return null;
        }
    }

    // Retrieve a product by its ID asynchronously
    public async Task<Product> GetByIdAsync(int id)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Product>(responseBody);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Product with ID {id} not found.");
                return null;
            }
            else
            {
                Console.WriteLine($"Error retrieving product (Status Code: {response.StatusCode}): {response.ReasonPhrase}");
                return null;
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error retrieving product: {ex.Message}");
            return null;
        }
    }

    // Retrieve all products asynchronously
    public async Task<List<Product>> GetAllProductsAsync()
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Product>>(responseBody);
            }
            else
            {
                Console.WriteLine($"Error retrieving products (Status Code: {response.StatusCode}): {response.ReasonPhrase}");
                return new List<Product>();
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error retrieving products: {ex.Message}");
            return new List<Product>();
        }
    }


    // Update a product asynchronously
    public async Task<Product> UpdateAsync(int id, Product product)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", product);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Product>(responseBody);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error updating product: {ex.Message}");
            return null;
        }
    }

    // Delete a product by its ID asynchronously
    public async Task DeleteAsync(int id)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}"); 
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error deleting product: {ex.Message}");
        }
    }

}
