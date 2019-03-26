using System.Collections.Concurrent;

namespace Ankama.Cube.Extensions
{
	public static class CollectionsExtensions
	{
		public static void Clear<T>(this ConcurrentQueue<T> queue)
		{
			T result;
			while (queue.TryDequeue(out result))
			{
			}
		}
	}
}
