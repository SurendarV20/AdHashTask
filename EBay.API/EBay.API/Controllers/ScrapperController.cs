using EBay.Dto;
using HtmlAgilityPack;
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
            string requestUrl = $"https://www.wholesalehyundaiparts.com/p/__/16187484/{partNumber}.html";
            //var data = await Helper.GetDataAsync(requestUrl);
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(requestUrl);
            var info = new ScrappedPartInfo();
            HtmlNode spanNode = doc.DocumentNode.SelectSingleNode("//span[@class='body-3 alt-stock-code-text']");

            if (spanNode != null)
            {
                info.PartNumbers = spanNode.InnerText.Trim().Split("; ").Where(s => !string.IsNullOrEmpty(s)).ToList();
            }

            HtmlNodeCollection imgNodes = doc.DocumentNode.SelectNodes("//img[@class='imgTires']");

            if (imgNodes != null)
            {
                foreach (HtmlNode imgNode in imgNodes)
                {
                    info.Images.Add(imgNode.Attributes.FirstOrDefault(s => s.Name == "src").Value);
                }
            }
            HtmlNode descNode = doc.DocumentNode.SelectSingleNode("//span[@class='prodDescriptH2']");

            if (descNode != null)
            {
                info.Description = descNode.InnerText;
            }

            HtmlNode priceNode = doc.DocumentNode.SelectSingleNode("//span[@class='productPriceSpan money-3']");

            if (priceNode != null)
            {
                info.Price = Convert.ToDecimal(priceNode.InnerText.Replace("$", ""));
            }

            return Ok(info);
        }
    }
}
