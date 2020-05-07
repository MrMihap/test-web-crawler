using System;
using System.Collections.Generic;
using System.Text;

namespace Mihap.CrawlerApi.Processing
{
	public class ProcessingManager
	{
		private List<ProcessingWorker> Processors = new List<ProcessingWorker>();
		private bool DoProcessing = false;



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
			Processors.ForEach(x => x.Start());
		}
		public void StopProcessing()
		{
			Processors.ForEach(x => x.DoProcessing = false);
			throw new NotImplementedException();
		}
	}
}
