using System.Threading.Tasks;

namespace CovidonusApi.Repositories.Abstraction
{
    public interface ISeedDataRepository
    {
        Task RefreshCovidDataAsync();
    }
}