using Mihap.CrawlerApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mihap.CrawlerApi.Filtering
{
	public class FilterConvey
	{
		private List<BaseFilter> Filters = new List<BaseFilter>();

		public FilterConvey AddFilter(BaseFilter baseFilter)
		{
			return this;
		}
		public bool PassLink(Link link)
		{
			foreach(var filter in Filters)
			{
				if (!filter.Pass(link)) return false;
			}
			return true;
		}
	}
}
