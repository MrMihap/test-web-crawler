using Mihap.CrawlerApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mihap.CrawlerApi.Filtering.Filters
{
	public class DomainFilter : BaseFilter
	{
		public string Domain { get; set; } = string.Empty;

		public DomainFilter(string Domain, FilterMode filterMode)
		{
			this.Domain = Domain;
			this.filterMode = filterMode;
		}

		public override bool Pass(Link link)
		{
			if (link == null) return false;
			if (String.IsNullOrEmpty(Domain)) return false;



			switch (filterMode)
			{
				case FilterMode.Pass:
					if (GetDomain(link.Url).Contains(Domain))
						return true;
					else
						return false;

				case FilterMode.NoPass:
					if (GetDomain(link.Url).Contains(Domain))
						return false;
					else
						return true;
			}
			return false;
		}
		private string GetDomain(string url)
		{
			// MOCKED
			var domain = (new Uri(url)).Host;
			

			return domain;
		}
	}
}
