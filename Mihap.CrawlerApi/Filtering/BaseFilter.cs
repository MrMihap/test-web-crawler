using Mihap.CrawlerApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mihap.CrawlerApi.Filtering
{
	public  class BaseFilter
	{
		public FilterMode filterMode { get; set; } = FilterMode.Pass;

		public virtual bool Pass(Link link)
		{
			if (link == null)
				return false;

			switch (filterMode)
			{
				case FilterMode.NoPass:
					return false;
				case FilterMode.Pass:
					return true;
			}
			return false;
		}
	}

	public enum FilterMode
	{
		/// <summary>
		/// Pass if condition is true
		/// </summary>
		Pass,
		/// <summary>
		/// Pass if condition is false
		/// </summary>
		NoPass
	}
}
