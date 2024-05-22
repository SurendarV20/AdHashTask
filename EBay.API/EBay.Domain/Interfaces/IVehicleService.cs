using Microsoft.AspNetCore.Http;

namespace EBay.Domain
{
    public interface IVehicleService
    {
        Task<IEnumerable<string>> GetMakes();
        Task<IEnumerable<string>> GetModels(string make);
        Task<IEnumerable<int>> GetYears(string make, string model);
        Task ImportVehicles(IFormFile file);
    }
}