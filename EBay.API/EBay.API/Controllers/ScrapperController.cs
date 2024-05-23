using EBay.Dto;
using EBay.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace EBay.API.Controllers
{
    [Route("api/scrap")]
    [ApiController]
    public class ScrapperController : ControllerBase
    {

        [HttpPost("partNumber")]
        public async Task<IActionResult> GetScrappingDataAsync([FromQuery] string partNumber)
        {
            string requestUrl = "https://www.wholesalehyundaiparts.com/wm.aspx/GetUserVehicles";

            var reqData = new ScrapperRequestDto
            {
                pageUrl = "/productDetails.aspx",
                queryString = @$"\modelYear=0&stockNumber={partNumber}&ukey_product=16187484\"
            };
            return Ok(await Helper.PostDataAsync(requestUrl, reqData));
        }
    }
}
