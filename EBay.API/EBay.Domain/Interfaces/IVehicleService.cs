using EBay.Dto;
using Microsoft.AspNetCore.Http;

namespace EBay.Domain
{
    public interface IVehicleService
    {
        IEnumerable<VehicleDetailDto> GetAllVehicleDetail();
        Task<IEnumerable<string>> GetMakes();
        Task<IEnumerable<string>> GetModels();
        Task<IEnumerable<VehicleDetailDto>> GetSearchResults(string query);
        IEnumerable<VehicleDetailDto> GetVehicleDetailList(List<VehicleDataListRequestDto> vehicleDataListRequestDtoList);
        Task<IEnumerable<int>> GetYears();
        Task ImportVehicles(IFormFile file);
        void SaveNotes(IEnumerable<VehicleDetailDto> vehicleDetailDtos);
    }
}