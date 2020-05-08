using System;
using System.Collections.Generic;
using System.Text;

namespace Mihap.CrawlerApi.Models
{
	public class Link
	{
		public string Url { get; set; } = String.Empty;

		public string ContentType { get; set; }

		public int ResponseLength { get; set; }
	}
}
