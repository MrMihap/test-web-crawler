using System;
using System.Threading.Tasks;
using Mihap.CrawlerApi;
using Mihap.CrawlerApi.Exporting.Exporters;
using Mihap.CrawlerApi.Filtering;

namespace Mihap.Crawler
{
	class Program
	{
		static async Task Main(string[] args)
		{
			WebCrawlerClient.OnCrawlingFinished += WebCrawlerClient_OnCrawlingFinished;

			await WebCrawlerClient.RunCrowler(crawler =>
			{
				crawler.filterConvey
					.AddFilter(new ContentTypeFilter("html", FilterMode.Pass))
					.AddFilter(new ContentTypeFilter("js", FilterMode.NoPass));

				crawler
					.AddExporter(new FileExporter("output.txt"))
					.AddExporter(new ConsoleExporter());

				crawler.MaxDepth = 3;

				crawler.WorkersN = 5;
				crawler.RootUrl = @"https://metanit.com/sharp/tutorial/";
				//crawler.RootUrl = @"https://metanit.com/sharp/aspnet5/32.1.php";
			});
		}

		private static void WebCrawlerClient_OnCrawlingFinished()
		{
			throw new NotImplementedException();
		}
	}
}
