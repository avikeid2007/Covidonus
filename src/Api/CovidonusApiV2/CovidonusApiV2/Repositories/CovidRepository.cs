using CovidonusApiV2.Models;
using CovidonusApiV2.Models.DTOs;
using CovidonusApiV2.Repositories.Abstraction;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidonusApiV2.Repositories
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
                await _seedDataRepository.RefreshCovidDataAsync(!isRefresh);
            return MenuList;

        }
        public IEnumerable<Resource> GetResource()
        {
            return ResourceList;
        }
        //public CovidNews GetNews()
        //{
        //    return News;
        //}
        public IEnumerable<InfoGraphic> GetInfoGraphics()
        {
            return new List<InfoGraphic>
                    {
                        new InfoGraphic() { Order = 0, Type = "image", UrlToImage = "https://www.mygov.in/sites/all/themes/mygov/images/covid/symptoms.png" },
                        new InfoGraphic() { Order = 1, Type = "image", UrlToImage = "https://www.mygov.in/sites/all/themes/mygov/images/covid/block-one.png" },
                    };
        }
    }


}