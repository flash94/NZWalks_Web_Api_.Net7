using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models.DTO;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            List<RegionDto> response = new List<RegionDto>();
            try
            {
                //get all regions from web API
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7262/api/regions");
                httpResponseMessage.EnsureSuccessStatusCode();

                var stringResponsebody = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>();
                response.AddRange(stringResponsebody);
            }
            catch (Exception)
            {
                //Log the exception
                throw;
            }
            return View(response);
        }
    }
}
