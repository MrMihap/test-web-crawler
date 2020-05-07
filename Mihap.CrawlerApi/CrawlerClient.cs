﻿using Mihap.CrawlerApi.Processing;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mihap.CrawlerApi
{
	public delegate void OnCrawlingFinishedDelegate();
	public class WebCrawlerClient
	{
		private static object BlockingObject = new object();

		//private static WebCrawlerClient _Instance;
		public static WebCrawlerClient Instance { get; } = new WebCrawlerClient();

		public static event OnCrawlingFinishedDelegate OnCrawlingFinished;

		private CrawlerClientSettings settings;
		private  TaskData RootLinkTask;
		ProcessingManager processingManager;


		public static async Task RunCrowler(Action<CrawlerClientSettings> options)
		{
			if (!Monitor.TryEnter(BlockingObject, 50)) throw new Exception("Worker Is Busy!");
			try
			{
				CrawlerClientSettings settings = new CrawlerClientSettings();

				options.Invoke(settings);

				//await Instance.Run( WorkersCount);
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
			processingManager.OnAllWorkersFinished += ProcessingManager_OnAllWorkersFinished;
			processingManager.StartProcessing();
		}

		private void ProcessingManager_OnAllWorkersFinished()
		{
			OnCrawlingFinished?.Invoke();
		}
	}
}
