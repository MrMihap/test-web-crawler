using Mihap.CrawlerApi.Models;
using Mihap.CrawlerApi.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
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
				{
					Console.WriteLine($"Task getted!  {taskData.Link.Url}");
					return taskData;
				}
			}
			return null;
		}

		public static void AddTask(TaskData data)
		{
			if (data.DepthLevel >= MaxDepth) 
				return;

			lock (TasksQueueLocks[data.DepthLevel])
				TasksQueues[data.DepthLevel].Enqueue(data);
			Console.WriteLine($"Inpt new Task!  {data.Link.Url}");
		}

		public static int GetCount()
		{
			// попытка получить актуальную задачу, чем ниже уровнь вложенности ссылки - тем выше приоритет

			int[] n = new int[MaxDepth];

			for (int i = 0; i < MaxDepth; i++)
			{
				lock (TasksQueueLocks[i])
				{
					n[i] = TasksQueues[i].Count;

				}
			}

			//dEBUG!!!
			Console.WriteLine($" {n[0]} {n[1]}");

			return n.Sum();
		}
	}
}
