using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mihap.CrawlerApi
{
	public static class WebCrawlerClient
	{
		private static  object BlockingObject = new object();

		public static async Task RunCrowler(string StartUrl, int MaxDeep = 1)
		{
			if (!Monitor.TryEnter(BlockingObject, 50)) throw new Exception("Worker Is Busy!");
			try
			{
				await Task.CompletedTask.ConfigureAwait(false);

			}
			finally
			{
				Monitor.Exit(BlockingObject);
			}
		}
	}
}
