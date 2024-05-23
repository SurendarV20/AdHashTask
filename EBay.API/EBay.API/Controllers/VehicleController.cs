using EBay.Domain;
using EBay.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EBay.API.Controllers
{
    [Route("api/vehicle")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }


        [HttpGet("make")]
        public async Task<IActionResult> GetMakes()
        {
            return Ok(await _vehicleService.GetMakes());
        }

        [HttpGet("model")]
        public async Task<IActionResult> GetModels()
        {
            return Ok(await _vehicleService.GetModels());

        }

        [HttpGet("year")]
        public async Task<IActionResult> GetYears()
        {
            return Ok(await _vehicleService.GetYears());

        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportVehicles([FromForm] FileDto fileDto)
        {
            if (fileDto.File == null || fileDto.File.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var extension = Path.GetExtension(fileDto.File.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || extension != ".xlsx")
            {
                return BadRequest("Invalid file extension.");
            }
            await _vehicleService.ImportVehicles(fileDto.File);
            return Ok();

        }

        [HttpGet("search/{query}")]
        public async Task<IActionResult> GetSearchResult([FromRoute] string query)
        {
            return Ok(await _vehicleService.GetSearchResults(query));
        }


        [HttpGet("View")]
        public IActionResult View()
        {
            return Ok(_vehicleService.GetAllVehicleDetail());
        }



        [HttpPost("vehicleDataList")]
        public async Task<IActionResult> GetVehicleDataList([FromBody] List<VehicleDataListRequestDto> vehicleDataListRequestDtoList)
        {
            return Ok(_vehicleService.GetVehicleDetailList(vehicleDataListRequestDtoList));
        }
    }
}
