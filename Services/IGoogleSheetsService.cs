namespace GestaoHigienizePrime.Services;

public interface IGoogleSheetsService
{
    Task<List<T>> GetDataAsync<T>(string sheetName);
    Task<T?> GetByIdAsync<T>(string sheetName, string id) where T : class;
    Task<bool> InsertDataAsync<T>(string sheetName, T data);
    Task<bool> UpdateDataAsync<T>(string sheetName, string id, T data);
    Task<bool> DeleteDataAsync(string sheetName, string id);
    Task<List<T>> QueryDataAsync<T>(string sheetName, string field, string value);
}
