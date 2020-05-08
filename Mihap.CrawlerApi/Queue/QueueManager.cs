using Mihap.CrawlerApi.Models;
using Mihap.CrawlerApi.Processing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mihap.CrawlerApi.Queue
{
	public delegate void OnTaskFinishedDelegate(Link link);
	public static class QueueManager
	{
		public static event OnTaskFinishedDelegate OnTaskFinished;

		private static List<Queue<TaskData>> TasksQueues = new List<Queue<TaskData>>();
		private static List<object> TasksQueueLocks = new List<object>();
		private static int MaxDepth = 0;
		
		public static void Init(int maxDepth)
		{
			for(int i =0; i < maxDepth; i++)
			{
				TasksQueues.Add(new Queue<TaskData>());
				TasksQueueLocks.Add(new object());
			}
			MaxDepth = maxDepth; 
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static TaskData GetTask()
		{
			TaskData taskData;
			bool TaskFound = false;
			// попытка получить актуальную задачу, чем ниже уровнь вложенности ссылки - тем выше приоритет
			for(int i = 0; i < MaxDepth; i++)
			{
				lock(TasksQueueLocks[i])
				{
					TaskFound = TasksQueues[i].TryDequeue(out taskData);
				}
				if (TaskFound) 
					return taskData;
				
			}
			return null;
		}
		public static void AddTask(TaskData data)
		{
			if (data.DepthLevel >= MaxDepth) return;
				//throw new IndexOutOfRangeException("depth is overheaded");

			lock (TasksQueueLocks[data.DepthLevel])
				TasksQueues[data.DepthLevel].Enqueue(data);
		}
	}
}
