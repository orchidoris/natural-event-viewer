using System;
using System.Threading.Tasks;

namespace Eonet.Core
{
    public interface IEonetMemoryCache<TItem>
    {
        Task<TItem> GetOrCreate(object key, Func<Task<TItem>> createItem);
    }
}
