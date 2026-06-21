using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GestaoHigienizePrime.Services;

public class GoogleSheetsService : IGoogleSheetsService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly JsonSerializerSettings _jsonSettings;

    public GoogleSheetsService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["GoogleSheets:ApiBaseUrl"] ?? throw new InvalidOperationException("GoogleSheets:ApiBaseUrl not configured");
        _jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };
    }

    public async Task<List<T>> GetDataAsync<T>(string sheetName)
    {
        try
        {
            var url = $"{_baseUrl}?action=getAll&sheet={sheetName}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GoogleSheetsResponse<T>>(json, _jsonSettings);
            return result?.Data ?? new List<T>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting data from {sheetName}: {ex.Message}");
            return new List<T>();
        }
    }

    public async Task<T?> GetByIdAsync<T>(string sheetName, string id) where T : class
    {
        try
        {
            var url = $"{_baseUrl}?action=getById&sheet={sheetName}&id={id}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GoogleSheetsResponse<T>>(json, _jsonSettings);
            return result?.Data?.FirstOrDefault();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting by id from {sheetName}: {ex.Message}");
            return default;
        }
    }

    public async Task<bool> InsertDataAsync<T>(string sheetName, T data)
    {
        try
        {
            var json = JsonConvert.SerializeObject(new { action = "insert", sheet = sheetName, data }, _jsonSettings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_baseUrl, content);
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GoogleSheetsResponse<T>>(responseJson, _jsonSettings);
            return result?.Success ?? false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inserting data into {sheetName}: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateDataAsync<T>(string sheetName, string id, T data)
    {
        try
        {
            var json = JsonConvert.SerializeObject(new { action = "update", sheet = sheetName, id, data }, _jsonSettings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_baseUrl, content);
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GoogleSheetsResponse<T>>(responseJson, _jsonSettings);
            return result?.Success ?? false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating data in {sheetName}: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteDataAsync(string sheetName, string id)
    {
        try
        {
            var json = JsonConvert.SerializeObject(new { action = "delete", sheet = sheetName, id });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_baseUrl, content);
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GoogleSheetsResponse<bool>>(responseJson, _jsonSettings);
            return result?.Success ?? false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting data from {sheetName}: {ex.Message}");
            return false;
        }
    }

    public async Task<List<T>> QueryDataAsync<T>(string sheetName, string field, string value)
    {
        try
        {
            var url = $"{_baseUrl}?action=query&sheet={sheetName}&field={field}&value={Uri.EscapeDataString(value)}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GoogleSheetsResponse<T>>(json, _jsonSettings);
            return result?.Data ?? new List<T>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error querying data from {sheetName}: {ex.Message}");
            return new List<T>();
        }
    }
}

public class GoogleSheetsResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public List<T>? Data { get; set; }
}
