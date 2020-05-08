using Mihap.CrawlerApi.Models;
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
		public bool IsDone { get; set; } = false;

		public Link Link { get; set; } = new Link();

		public List<TaskData> ChildTasks { get; set; } = new List<TaskData>();

		public static List<TaskData> EnrollToList(TaskData root)
		{
			List<TaskData> result = new List<TaskData>();

			foreach(var taskData in root.ChildTasks)
			{
				result.AddRange(TaskData.EnrollToList(taskData));
			}

			return result;
		}
	}
}
