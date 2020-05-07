using System;
using System.Collections.Generic;
using System.Text;

namespace Mihap.CrawlerApi.Processing
{
	public class TaskData
	{
		public int DepthLevel { get; set; }
		public string Url { get; set; } = String.Empty;
		public bool IsDone { get; set; } = false;

		public List<TaskData> ChildTasks { get; set; } = new List<TaskData>();
	}
}
