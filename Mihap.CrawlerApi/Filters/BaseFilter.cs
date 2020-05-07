using System;
using System.Collections.Generic;
using System.Text;

namespace Mihap.CrawlerApi.Filters
{
	public class BaseFilter
	{
		public FilterMode filterMode { get; set; } = FilterMode.Pass;

	}

	public enum FilterMode
	{
		Pass,
		NoPass
	}
}
