using Mihap.CrawlerApi.Models;
using Mihap.CrawlerApi.Queue;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mihap.CrawlerApi.Processing
{
	public delegate void OnLinkProcessedDelegate(List<Link> links);
	public class ProcessingWorker
	{
		public bool DoProcessing { get; set; } = true;

		public event OnLinkProcessedDelegate OnLinkProcessed;

		public void Start()
		{
			Task.Run(() => { ProcessingFunction(); });
		}

		private void ProcessingFunction()
		{

			while(DoProcessing)
			{
				TaskData taskData = QueueManager.GetTask(); 
				if(taskData == null)
				{
					Task.Delay(150);
					continue;
				}

			}
		}
		private static async Task<List<Link>> ParseUrl(TaskData taskData)
		{
			await Task.CompletedTask.ConfigureAwait(false);

			var result = new List<Link>();

			return result;
		}
	}
}
