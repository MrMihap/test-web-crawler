using System;
using System.Collections.Generic;
using System.Text;

namespace Mihap.CrawlerApi.Models
{
	public class TaskData
	{
		public int DepthLevel { get; set; }
		public string Url { get; set; } = String.Empty;

		public List<TaskData> ChildTasks { get; set; } = new List<TaskData>();
	}
}
