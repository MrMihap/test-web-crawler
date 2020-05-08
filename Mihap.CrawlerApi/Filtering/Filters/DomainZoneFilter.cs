using Mihap.CrawlerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mihap.CrawlerApi.Filtering.Filters
{
	public class DomainZoneFilter : BaseFilter
	{
		public string DomainZone { get; set; } = String.Empty;

		public DomainZoneFilter(string ContentType, FilterMode filterMode)
		{
			this.DomainZone = ContentType;
			this.filterMode = filterMode;
		}

		public override bool Pass(Link link)
		{
			if (link == null) return false;
			if (String.IsNullOrEmpty(DomainZone)) return false;

			switch(filterMode)
			{
				case FilterMode.Pass:
					if (GetDomainZone(link.Url).Contains(DomainZone))
						return true;
					else 
						return false;

				case FilterMode.NoPass:
					if (GetDomainZone(link.Url).Contains(DomainZone))
						return false;
					else
						return true;
			}
			return false;
		}
		private string GetDomainZone(string url)
		{
			// MOCKED
			var domain = (new Uri(url)).Host;
			if(domain.Contains("."))
			{
				return domain.Split('.').Last();
			}

			return "undefined";
		}
	}
}
