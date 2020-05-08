using Mihap.CrawlerApi.Exporting;
using Mihap.CrawlerApi.Filtering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mihap.CrawlerApi
{
	public class WebCrawlerSettings
	{
		public FilterConvey filterConvey { get; set; } = new FilterConvey();

		public List<BaseExporter> exporters { get; set; } = new List<BaseExporter>();

		public string RootUrl { get; set; } = String.Empty;

		public int WorkersN { get; set; } = 2;

		public int MaxDepth { get; set; } = 1;

		public WebCrawlerSettings AddExporter(BaseExporter exporter)
		{
			exporters.Add(exporter);
			return this;
		}
	}
}
