using Mihap.CrawlerApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mihap.CrawlerApi.Filtering
{
	public class ContentTypeFilter : BaseFilter
	{
		public string ContentType { get; set; } = String.Empty;

		public override bool Pass(Link link)
		{
			if (link == null) return false;
			if (String.IsNullOrEmpty(ContentType)) return false;
			switch(filterMode)
			{
				case FilterMode.Pass:
					if (link.ContentType.Contains(ContentType))
						return true;
					else 
						return false;

				case FilterMode.NoPass:
					if (!link.ContentType.Contains(ContentType))
						return true;
					else
						return false;
			}
			return false;
		}

		public ContentTypeFilter(string ContentType, FilterMode filterMode)
		{
			this.ContentType = ContentType;
			this.filterMode = filterMode;
		}

	}
}
