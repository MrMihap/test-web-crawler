using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mihap.CrawlerApi.Processing
{
	[DebuggerDisplay("{DepthLevel} : {Url} - {IsDone}")]
	public class TaskData
	{
		public int DepthLevel { get; set; }
		public string Url { get; set; } = String.Empty;
		public bool IsDone { get; set; } = false;

		public List<TaskData> ChildTasks { get; set; } = new List<TaskData>();

		//public static List<TaskData> EnrollToList(TaskData root)
		//{

		//}
	}
}
