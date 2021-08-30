using System.Threading.Tasks;

namespace RssStore.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}