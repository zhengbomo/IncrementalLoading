/* ==============================================================================
 * 功能描述：IIncrementalSource  
 * 创 建 者：贤凯
 * 创建日期：1/29/2015 8:57:43 PM
 * ==============================================================================*/

using System.Collections.Generic;
using System.Threading.Tasks;

namespace IncrementalLoading.Utility
{
    public interface IIncrementalSource<T>
    {
        Task<IEnumerable<T>> GetPagedItems(int pageIndex, int pageSize);
    }
}
