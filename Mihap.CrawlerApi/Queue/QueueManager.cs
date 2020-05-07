using Mihap.CrawlerApi.Models;
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
				TasksQueueLocks.Add(new object);
			}
			MaxDepth = maxDepth; 
		}

		public static TaskData GetTask()
		{
			TaskData taskData;
			bool TaskFound = false;
			for(int i = 0; i < MaxDepth; i++)
			{

				lock(TasksQueueLocks[i])
				{
					TaskFound = TasksQueues[i].TryDequeue(out taskData);
				}
				if (TaskFound) return taskData;
			}
			return null;
		}
		public static void AddTask(TaskData data, int depth)
		{
			if (depth >= MaxDepth) throw new IndexOutOfRangeException("depth is overheaded");

			lock (TasksQueueLocks[depth])
				TasksQueues[depth].Enqueue(data);
		}
	}
}
