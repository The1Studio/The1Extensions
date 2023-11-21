namespace UniT.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Random = UnityEngine.Random;

    public static class CollectionExtensions
    {
        public static T Choice<T>(this IList<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public static T Choice<T>(this ICollection<T> list, ICollection<int> weights)
        {
            var sumWeight = Random.Range(0, weights.Sum());
            return IterTools.StrictZip(list, weights)
                .First((_, weight) => (sumWeight -= weight) < 0)
                .Item1;
        }

        public static T Choice<T>(this ICollection<T> list, ICollection<float> weights)
        {
            var sumWeight = Random.Range(0, weights.Sum());
            return IterTools.StrictZip(list, weights)
                .First((_, weight) => (sumWeight -= weight) < 0)
                .Item1;
        }

        public static void RemoveAt<T>(this IList<T> list, Index index)
        {
            list.RemoveAt(index.IsFromEnd ? list.Count - index.Value : index.Value);
        }

        public static void Clear<T>(this ICollection<T> collection, Action<T> action)
        {
            collection.ForEach(action);
            collection.Clear();
        }

        public static Stack<T> ToStack<T>(this IEnumerable<T> enumerable)
        {
            return new(enumerable);
        }

        public static T PeekOrDefault<T>(this Stack<T> stack)
        {
            return stack.Count > 0 ? stack.Peek() : default;
        }

        public static T PeekOrDefault<T>(this Stack<T> stack, Func<T> valueFactory)
        {
            return stack.Count > 0 ? stack.Peek() : valueFactory();
        }

        public static T PopOrDefault<T>(this Stack<T> stack)
        {
            return stack.Count > 0 ? stack.Pop() : default;
        }

        public static T PopOrDefault<T>(this Stack<T> stack, Func<T> valueFactory)
        {
            return stack.Count > 0 ? stack.Pop() : valueFactory();
        }

        public static Queue<T> ToQueue<T>(this IEnumerable<T> enumerable)
        {
            return new(enumerable);
        }

        public static T PeekOrDefault<T>(this Queue<T> queue)
        {
            return queue.Count > 0 ? queue.Peek() : default;
        }

        public static T PeekOrDefault<T>(this Queue<T> queue, Func<T> valueFactory)
        {
            return queue.Count > 0 ? queue.Peek() : valueFactory();
        }

        public static T DequeueOrDefault<T>(this Queue<T> queue)
        {
            return queue.Count > 0 ? queue.Dequeue() : default;
        }

        public static T DequeueOrDefault<T>(this Queue<T> queue, Func<T> valueFactory)
        {
            return queue.Count > 0 ? queue.Dequeue() : valueFactory();
        }

        public static T[,] To2DArray<T>(this T[][] source)
        {
            try
            {
                var dimension1 = source.Length;
                var dimension2 = source.GroupBy(row => row.Length).Single().Key;
                var result     = new T[dimension1, dimension2];
                for (var i = 0; i < dimension1; ++i)
                {
                    for (var j = 0; j < dimension2; ++j)
                    {
                        result[i, j] = source[i][j];
                    }
                }
                return result;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("The given jagged array is not rectangular");
            }
        }
    }
}