using System.Threading.Tasks;

namespace CovidonusApiV2.Repositories.Abstraction
{
    public interface ISeedDataRepository
    {
        Task RefreshCovidDataAsync(bool isRefreshNews = true);
    }
}