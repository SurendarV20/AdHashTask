using EBay.Domain;
using EBay.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace EBay.API.Controllers
{
    [Route("api/vehicle")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IDistributedCache _distributedCache;

        public VehicleController(IVehicleService vehicleService, IDistributedCache distributedCache)
        {
            _vehicleService = vehicleService;
            _distributedCache = distributedCache;
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
        public async Task<IActionResult> ViewAsync()
        {
            var data = Enumerable.Empty<VehicleDetailDto>();
            var cachedData = await _distributedCache.GetStringAsync("GetAllVehicleDetail");

            if (cachedData != null)
            {
                data = JsonConvert.DeserializeObject<IEnumerable<VehicleDetailDto>>(cachedData);
            }
            else
            {
                data = _vehicleService.GetAllVehicleDetail();
                var distributedCacheEntryOptions = new DistributedCacheEntryOptions();
                distributedCacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15);
                distributedCacheEntryOptions.SlidingExpiration = null;
                var json = JsonConvert.SerializeObject(data);
                await _distributedCache.SetStringAsync("GetAllVehicleDetail", json, distributedCacheEntryOptions);
            }
            return Ok(data);
        }



        [HttpPost("vehicleDataList")]
        public async Task<IActionResult> GetVehicleDataList([FromBody] List<VehicleDataListRequestDto> vehicleDataListRequestDtoList)
        {
            return Ok(_vehicleService.GetVehicleDetailList(vehicleDataListRequestDtoList));
        }

        [HttpPost("saveNotes")]
        public async Task<IActionResult> SaveNotes([FromBody] List<VehicleDetailDto> vehicleDetailList)
        {
            _vehicleService.SaveNotes(vehicleDetailList);

            return Ok();
        }
    }
}
