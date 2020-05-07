using System;
using System.Collections.Generic;
using System.Text;

namespace Mihap.CrawlerApi.Processing
{
	public class ProcessingManager
	{
		private List<Processor> Processors = new List<Processor>();
		private bool DoProcessing = false;

		public static ProcessingManager InitNewManager(int WorkersN)
		{
			if (WorkersN < 1) WorkersN = 1;

			var manager = new ProcessingManager();
			for(int i =0; i< WorkersN; i++)
			{
				manager.Processors.Add(new Processor());
			}

			return manager;
		}

		public void StartProcessing()
		{

		}
		public void StopProcessing()
		{
			throw new NotImplementedException();
		}
	}
}
