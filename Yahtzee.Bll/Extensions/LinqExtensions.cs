using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Yahtzee.Bll.Extensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<CountObject<TSource>> GetDuplicateCounts<TSource, TKey>(this IEnumerable<TSource> list, Func<TSource, TKey> keySelector)
        {
            return list.GroupBy(x => keySelector(x))
                .Select(x => new CountObject<TSource>
                {
                    Object = x.First(),
                    Count = x.Count()
                })
                .OrderByDescending(x => x.Count);
        }

        public static IEnumerable<IEnumerable<TSource>> GetSequences<TSource>(this IEnumerable<TSource> list, Func<TSource, int> keySelector)
        {
            var orderedList = list.OrderBy(l => keySelector(l));

            var result = new List<List<TSource>>();

            foreach(var line in list)
            {
                List<TSource> lists = result.LastOrDefault();
                if (lists == null)
                {
                    result.Add(new List<TSource>());
                    result.Last().Add(line);
                    continue;
                }

                var number = keySelector(line);

                var lastNumber = lists.Select(l => keySelector(l)).Last();

                if (number != lastNumber + 1)
                {
                    result.Add(new List<TSource>());
                }

                result.Last().Add(line);
            }

            return result;
        }
    }

    public class CountObject<T>
    {
        public T Object { get; set; }
        public int Count { get; set; }
    }
}