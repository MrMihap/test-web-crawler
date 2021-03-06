﻿using System;
using System.Threading.Tasks;
using Mihap.CrawlerApi;
using Mihap.CrawlerApi.Exporting.Exporters;
using Mihap.CrawlerApi.Filtering;
using Mihap.CrawlerApi.Filtering.Filters;

namespace Mihap.Crawler
{
	class Program
	{
		static async Task Main(string[] args)
		{
			WebCrawler.OnCrawlingFinished += WebCrawlerClient_OnCrawlingFinished;

			await WebCrawler.RunCrowler(crawler =>
			{
				crawler.filterConvey
					.AddFilter(new ContentTypeFilter("css", FilterMode.NoPass))
					.AddFilter(new DomainZoneFilter("net", FilterMode.NoPass))
					.AddFilter(new DomainFilter("twitter", FilterMode.NoPass))
					.AddFilter(new DomainFilter("youtube", FilterMode.NoPass));

				crawler
					.AddExporter(new FileExporter("output.txt"))
					.AddExporter(new ConsoleExporter());

				crawler.MaxDepth = 3;

				crawler.WorkersN = 5;
				crawler.RootUrl = "https://metanit.com/sharp/tutorial/";
				//crawler.RootUrl = @"https://metanit.com/sharp/aspnet5/32.1.php";
			});
		}

		private static void WebCrawlerClient_OnCrawlingFinished()
		{
			Console.WriteLine("Finished");
		}
	}
}
