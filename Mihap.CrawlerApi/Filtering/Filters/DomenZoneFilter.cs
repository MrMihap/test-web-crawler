using Mihap.CrawlerApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mihap.CrawlerApi.Filtering
{
	public class DomenZoneFilter : BaseFilter
	{
		public string DomenZone { get; set; } = String.Empty;

		public DomenZoneFilter(string ContentType, FilterMode filterMode)
		{
			this.DomenZone = ContentType;
			this.filterMode = filterMode;
		}

		public override bool Pass(Link link)
		{
			if (link == null) return false;
			if (String.IsNullOrEmpty(DomenZone)) return false;



			switch(filterMode)
			{
				case FilterMode.Pass:
					if (GetDomenZone(link.Url).Contains(DomenZone))
						return true;
					else 
						return false;

				case FilterMode.NoPass:
					if (!GetDomenZone(link.Url).Contains(DomenZone))
						return true;
					else
						return false;
			}
			return false;
		}
		private string GetDomenZone(string url)
		{
			// MOCKED
			return "com";
		}
	}
}
