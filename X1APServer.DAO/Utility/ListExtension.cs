using System;
using System.Collections.Generic;
using System.Linq;

namespace X1APServer.Repository.Utility
{
    public static class ListExtension
    {
        /// <summary>
        /// 用來處理集合裡面的Recursive取出所有指定資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="childSelector"></param>
        /// <returns></returns>
        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> childSelector)
        {
            var stack = new Stack<T>(items);
            while (stack.Any())
            {
                var next = stack.Pop();
                yield return next;
                foreach (var child in childSelector(next))
                    stack.Push(child);
            }
        }
    }
}
