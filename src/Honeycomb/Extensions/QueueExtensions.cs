using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Honeycomb.Extensions
{
    static class QueueExtensions
    {
        internal static IEnumerable<T> DequeueChunk<T>(this ConcurrentQueue<T> queue, int chunkSize)
        {
            for (int i = 0; i < chunkSize && queue.Count > 0; i++)
            {
                queue.TryDequeue(out T item);
                yield return item;
            }
        }
    }
}