using Mihap.CrawlerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mihap.CrawlerApi.Processing
{
	public delegate void OnAllWorkersFinishedDelegate();
	public class ProcessingManager
	{
		private List<ProcessingWorker> Processors = new List<ProcessingWorker>();
		public bool DoProcessing = true;

		public event OnAllWorkersFinishedDelegate OnAllWorkersFinished;
		public event OnLinkProcessedDelegate OnLinkProcessed;

		public static ProcessingManager InitNewManager(int WorkersN)
		{
			if (WorkersN < 1) WorkersN = 1;

			var manager = new ProcessingManager();
			for(int i =0; i< WorkersN; i++)
			{
				manager.Processors.Add(new ProcessingWorker());
			}
			return manager;
		}

		public void StartProcessing()
		{
			Processors.ForEach(x => x.OnLinkProcessed += OnLinkProcessedHandler);
			Processors.ForEach(x => x.OnChildLinkProcessed += OnChildLinkProcessed);
			Processors.ForEach(x => x.Start());

			Task.Run(() => ControlFunction());
		}

		private void OnChildLinkProcessed(List<Link> links)
		{
			//throw new NotImplementedException();
		}

		private void OnLinkProcessedHandler(TaskData link)
		{
			OnLinkProcessed?.Invoke(link);
			//throw new NotImplementedException();
		}

		public void ControlFunction()
		{
			while(DoProcessing)
			{
				//check all task of max depth is Finished
				bool IsAllTaskDone = false;

				//if(TaskData.EnrollToList(WebCrawler.Instance.RootTask).Where(x=>x.IsDone == false).Count() == 0)
				//{
				//	IsAllTaskDone = true;
				//}

				//var test = TaskData.EnrollToList(WebCrawler.Instance.RootTask);

				if (IsAllTaskDone)
				{
					StopProcessing();
					OnAllWorkersFinished?.Invoke();
					break;
				}
				Thread.Sleep(1250);
			}
		}

		public void StopProcessing()
		{
			Processors.ForEach(x => x.DoProcessing = false);
			Processors.Clear();
		}
	}
}
