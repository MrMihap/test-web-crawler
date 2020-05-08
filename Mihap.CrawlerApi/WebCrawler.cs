using Mihap.CrawlerApi.Processing;
using Mihap.CrawlerApi.Queue;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mihap.CrawlerApi
{
	public delegate void OnCrawlingFinishedDelegate();
	public class WebCrawler
	{
		private static object BlockingObject = new object();

		//private static WebCrawlerClient _Instance;
		public static WebCrawler Instance { get; } = new WebCrawler();

		public static event OnCrawlingFinishedDelegate OnCrawlingFinished;

		public WebCrawlerSettings settings;

		public TaskData RootTask;

		ProcessingManager processingManager;


		public static async Task RunCrowler(Action<WebCrawlerSettings> options)
		{
			if (!Monitor.TryEnter(BlockingObject, 50)) throw new Exception("Worker Is Busy!");
			try
			{
				WebCrawlerSettings settings = new WebCrawlerSettings();

				options.Invoke(settings);

				Instance.settings = settings;

				QueueManager.Init(settings.MaxDepth);

				QueueManager.AddTask(new TaskData() { DepthLevel = 0, Link = new Models.Link() { Url = settings.RootUrl } });

				await Instance.Run();
			}
			finally
			{
				Monitor.Exit(BlockingObject);
			}
		}

		private async Task Run()
		{
			await Task.CompletedTask.ConfigureAwait(false);
			processingManager = ProcessingManager.InitNewManager(settings.WorkersN);
			processingManager.OnLinkProcessed += ProcessingManager_OnLinkProcessed;
			processingManager.OnAllWorkersFinished += ProcessingManager_OnAllWorkersFinished;

			processingManager.StartProcessing();

			while (processingManager.DoProcessing)
			{
				await Task.Delay(500);
			}
		}

		private void ProcessingManager_OnLinkProcessed(TaskData link)
		{
			string record = $"{link.Link.Url}: {link.Link.ContentType}, {link.Link.ResponseLength}";
			if (settings.filterConvey.PassLink(link.Link))
				foreach (var exporter in Instance.settings.exporters)
				{
					exporter.RecieveNewLinkRecord(record);
				}
		}

		private void ProcessingManager_OnAllWorkersFinished()
		{
			OnCrawlingFinished?.Invoke();
		}
	}
}
