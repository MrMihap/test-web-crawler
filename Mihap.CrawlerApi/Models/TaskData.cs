using System;
using System.Collections.Generic;
using System.Text;

namespace Mihap.CrawlerApi.Models
{
	public class TaskData
	{
		public int DeepnessLevel { get; set; }

		public List<TaskData> ChildTasks { get; set; } = new List<TaskData>();
	}
}
