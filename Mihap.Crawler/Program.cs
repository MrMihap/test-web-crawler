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
					.AddExporter(new FileExporter("output"))
					.AddExporter(new ConsoleExporter());

				crawler.MaxDepth = 3;

				crawler.WorkersN = 5;

				crawler.RootUrl = @"https://docs.microsoft.com/ru-ru/dotnet/csharp/language-reference/proposals/csharp-7.1/async-main";
			});
		}

		private static void WebCrawlerClient_OnCrawlingFinished()
		{
			throw new NotImplementedException();
		}
	}
}
