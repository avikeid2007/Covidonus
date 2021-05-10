using CovidonusApiV2.Models;
using CovidonusApiV2.Models.DTOs;
using CovidonusApiV2.Repositories.Abstraction;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CovidonusApiV2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CovidController : ControllerBase
    {

        ICovidRepository _covidRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public CovidController(ICovidRepository covidRepository, IWebHostEnvironment hostingEnvironment)
        {
            _covidRepository = covidRepository;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet("Counts")]
        public async Task<IEnumerable<StateWiseData>> GetCovidCountsAsync(bool isRefresh = false)
        {
            return await _covidRepository.GetCovidCountsAsync(isRefresh);
        }
        [HttpGet("Resource")]
        public IEnumerable<Resource> GetResource()
        {
            return _covidRepository.GetResource();
        }
        [HttpGet("News")]
        public CovidNews GetNews()
        {
            return _covidRepository.GetNews();
        }
        [HttpGet("InfoGraphics")]
        public IEnumerable<InfoGraphic> GetInfoGraphics()
        {
            return _covidRepository.GetInfoGraphics();
        }

        [HttpGet("thumbnail/{code}")]
        public IActionResult GetThumbnail(string code)
        {
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "images", $"{code}.png");
            var imageFileStream = System.IO.File.OpenRead(path);
            return File($"~/images/{code}", "image/png");
        }
    }
}
