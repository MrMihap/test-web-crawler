using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mihap.CrawlerApi.Models
{
	[DebuggerDisplay("{Url} : {ContentType} - {ResponseLength}")]
	public class Link
	{
		public string Url { get; set; } = String.Empty;

		public string ContentType { get; set; }

		public int ResponseLength { get; set; }
	}
}
