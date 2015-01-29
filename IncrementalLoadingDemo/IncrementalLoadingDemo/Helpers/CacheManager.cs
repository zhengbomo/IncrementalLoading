/* ==============================================================================
 * 功能描述：CacheManager  
 * 创 建 者：贤凯
 * 创建日期：1/29/2015 10:27:29 PM
 * ==============================================================================*/

using System.Collections.Generic;
using System.Threading.Tasks;

namespace IncrementalLoadingDemo.Helpers
{
    public class CacheManager
    {
        public static async Task SaveList()
        {
            await Task.Delay(1000);
        }

        public static async Task<List<string>> GetList()
        {
            await Task.Delay(1000);

            var list = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                list.Add("缓存数据" + i);
            }
            return list;
        }
    }
}
