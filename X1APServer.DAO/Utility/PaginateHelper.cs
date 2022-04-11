using System.Collections.Generic;
using System.Linq;

namespace X1APServer.Repository.Utility
{
    public class PaginateHelper
    {
        /// <summary>
        /// 用來處理Lazy loading分頁
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public static List<T> Paginate<T>(IOrderedQueryable<T> content, int? skip, int? take)
        {
            // Different variable type so that we can assign to it below...
            IQueryable<T> result = content;
            if (skip.HasValue) result = result.Skip(skip.Value);
            if (skip.HasValue && take.HasValue) result = result.Take(take.Value);
            return result.ToList();
        }
    }
}
