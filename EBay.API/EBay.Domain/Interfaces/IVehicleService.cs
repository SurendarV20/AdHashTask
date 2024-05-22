using EBay.Dto;
using Microsoft.AspNetCore.Http;

namespace EBay.Domain
{
    public interface IVehicleService
    {
        IEnumerable<VehicleDetailDto> GetAllVehicleDetail();
        Task<IEnumerable<string>> GetMakes();
        Task<IEnumerable<string>> GetModels(string make);
        Task<IEnumerable<VehicleDetailDto>> GetSearchResults(string query);
        Task<IEnumerable<VehicleDetailDto>> GetVehicleDetailList(VehicleDataListRequestDto vehicleDataListRequestDto);
        Task<IEnumerable<int>> GetYears(string make, string model);
        Task ImportVehicles(IFormFile file);
    }
}