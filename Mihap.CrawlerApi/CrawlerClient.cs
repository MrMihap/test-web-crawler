using Mihap.CrawlerApi.Processing;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mihap.CrawlerApi
{
	public class WebCrawlerClient
	{
		private static object BlockingObject = new object();
		//private static WebCrawlerClient _Instance;
		public static WebCrawlerClient Instance { get; } = new WebCrawlerClient();
		
		
		private int MaxDepth = 0;
		private int WorkersCount;
		private  TaskData RootLinkTask;
		ProcessingManager processingManager;


		public static async Task RunCrowler(string StartUrl, int MaxDeep = 1, int WorkersCount = 2)
		{
			if (!Monitor.TryEnter(BlockingObject, 50)) throw new Exception("Worker Is Busy!");
			try
			{
				Instance.WorkersCount = WorkersCount;
				await Instance.Run( WorkersCount);
			}
			finally
			{
				Monitor.Exit(BlockingObject);
			}
		}
		private async Task Run(int WorkersCount)
		{
			await Task.CompletedTask.ConfigureAwait(false);
			processingManager = ProcessingManager.InitNewManager(WorkersCount);

			processingManager.StartProcessing();
		}


	}
}
