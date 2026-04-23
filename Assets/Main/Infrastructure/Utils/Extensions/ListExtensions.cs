using System.Collections.Generic;
using Main.Context.Core.General;

namespace Main.Infrastructure.Utils.Extensions
{
    public static class ListExtensions
    {
        private static void Swap<T>(this IList<T> list, int index1, int index2)
        {
            (list[index1], list[index2]) = (list[index2], list[index1]);
        }

        public static void Swap<T>(this IList<T> list, T item1, T item2)
        {
            Swap(list, list.IndexOf(item1), list.IndexOf(item2));
        }
        
        public static void Shuffle<T>(this List<T> list, RandomManager randomManager)
        {
            var count = list.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = randomManager.Next(i, count);
                (list[i], list[r]) = (list[r], list[i]);
            }
        }
        
        public static void ShuffleAllDifferent<T>(this List<T> list, RandomManager randomManager)
        {
            var count = list.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = randomManager.Next(i + 1, count);
                (list[i], list[r]) = (list[r], list[i]);
            }
        }
        
        public static void Move<T>(this IList<T> list, int oldIndex, int newIndex)
        {
            var item = list[oldIndex];

            list.RemoveAt(oldIndex);

            if (newIndex > oldIndex)
            {
                newIndex--;
            }

            list.Insert(newIndex, item);
        }
    }
}
