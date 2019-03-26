using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine.Networking;

namespace Ankama.Cube.Network
{
	public static class TextWebRequest
	{
		public class Exception : System.Exception
		{
			public readonly long responseCode;

			public Exception(long responseCode, string message)
				: base(message)
			{
				this.responseCode = responseCode;
			}
		}

		public class AsyncResult<T, E> where E : Exception
		{
			public T value;

			public E exception;

			public bool hasException => exception != null;
		}

		public class AsyncResult : AsyncResult<string, Exception>
		{
		}

		public static IEnumerator ReadFile([NotNull] string url, [NotNull] AsyncResult result)
		{
			if (url.StartsWith("https://") || url.StartsWith("http://"))
			{
				url = UrlNoCache(url);
			}
			DownloadHandlerBuffer downloadHandler = new DownloadHandlerBuffer();
			UnityWebRequest val = new UnityWebRequest(url);
			val.set_disposeDownloadHandlerOnDispose(true);
			val.set_downloadHandler(downloadHandler);
			UnityWebRequest request = val;
			UnityWebRequest val2 = request;
			try
			{
				request.SendWebRequest();
				while (!request.get_isDone())
				{
					yield return null;
				}
				if (request.get_isHttpError() || request.get_isNetworkError())
				{
					result.exception = new Exception(request.get_responseCode(), request.get_error());
				}
				else
				{
					result.value = downloadHandler.get_text();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}

		private static string UrlNoCache([NotNull] string url)
		{
			long num = DateTime.Now.ToFileTimeUtc();
			return $"{url}?t={num}";
		}
	}
}
