using Microsoft.AspNetCore.Mvc;
using SignalR.CovidChart.API.Enums;
using SignalR.CovidChart.API.Models;
using SignalR.CovidChart.API.Services;

namespace SignalR.CovidChart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidsController : ControllerBase
    {
        private readonly CovidService _covidService;

        public CovidsController(CovidService covidService)
        {
            _covidService = covidService;
        }

        [HttpPost]
        public async Task<IActionResult> SaveCovid(Covid covid)
        {
            await _covidService.SavedCovid(covid);
            //IQueryable<Covid> covidList = _covidService.GetList();
            return Ok(_covidService.GetCovidChartList());
        }

        [HttpGet]
        public IActionResult InitializeCovid()
        {
            Random random = new Random();
            Enumerable.Range(1, 10).ToList().ForEach(async x =>
            {
                foreach (ECity item in Enum.GetValues(typeof(ECity)))
                {
                    var newCovid = new Covid { City = item, Count = random.Next(100, 1000), CovidDate = DateTime.Now.AddDays(x) };
                    _covidService.SavedCovid(newCovid).Wait();

                    Thread.Sleep(1000);
                }
            });
            return Ok("Covid-19 dataları veritabanına kayıt edildi.");
        }
    }
}