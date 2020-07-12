using CovidonusApi.Models;
using CovidonusApi.Models.DTOs;
using CovidonusApi.Repositories.Abstraction;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidonusApi.Repositories
{
    public class CovidRepository : CoreRepository, ICovidRepository
    {
        ISeedDataRepository _seedDataRepository;
        public CovidRepository(ISeedDataRepository seedDataRepository)
        {
            _seedDataRepository = seedDataRepository;
        }
        public async Task<IEnumerable<StateWiseData>> GetCovidCountsAsync(bool isRefresh = false)
        {
            if (MenuList == null || MenuList?.Count() <= 0 || isRefresh)
                await _seedDataRepository.RefreshCovidDataAsync();
            return MenuList;

        }
        public IEnumerable<Resource> GetResource()
        {
            return ResourceList;
        }
        public CovidNews GetNews()
        {
            return News;
        }
        public IEnumerable<InfoGraphic> GetInfoGraphics()
        {
            return new List<InfoGraphic>
                    {
                        new InfoGraphic() { Title = "Covid 19 Symptoms", Order = 0, Type = "image", UrlToImage = "http://covidonusapi.avnishkumar.co.in/InfoGraphics/covid0.jpg" },
                        new InfoGraphic() { Title = "Covid 19 Symptoms", Order = 1, Type = "image", UrlToImage = "http://covidonusapi.avnishkumar.co.in/InfoGraphics/covid1.jpg" },
                        new InfoGraphic() { Title = "Covid 19 Symptoms", Order = 2, Type = "image", UrlToImage = "http://covidonusapi.avnishkumar.co.in/InfoGraphics/covid2.jpg" },
                        new InfoGraphic() { Title = "Covid 19 Symptoms", Order = 3, Type = "image", UrlToImage = "http://covidonusapi.avnishkumar.co.in/InfoGraphics/covid3.jpg" }
                    };
        }
    }


}