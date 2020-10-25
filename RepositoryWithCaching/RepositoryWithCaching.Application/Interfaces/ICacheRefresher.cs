using System.Threading.Tasks;

namespace RepositoryWithCaching.Application.Interfaces
{
    public interface ICacheRefresher
    {
        Task RefreshCache();
    }
}