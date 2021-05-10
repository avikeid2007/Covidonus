using Covidonus.Data.Models;
using Covidonus.Data.Models.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covidonus.Data.Repositories
{
    public class CovidRepository
    {
        public async Task<IEnumerable<StateWiseData>> GetCovidCountsAsync(bool isRefresh = false)
        {
            if (SeedDataRepository.MenuList == null || SeedDataRepository.MenuList?.Count() <= 0 || isRefresh)
                await SeedDataRepository.RefreshCovidDataAsync(!isRefresh);
            return SeedDataRepository.MenuList;

        }
        public IEnumerable<Resource> GetResource()
        {
            return SeedDataRepository.ResourceList;
        }
        public CovidNews GetNews()
        {
            return SeedDataRepository.News;
        }
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