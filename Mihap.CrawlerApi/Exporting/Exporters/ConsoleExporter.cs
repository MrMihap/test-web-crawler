using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Mihap.CrawlerApi.Exporting.Exporters
{
	public class ConsoleExporter : BaseExporter
	{
		private volatile bool DoWriting = true;

		public ConsoleExporter(string Path)
		{
			BackgroundWorkerTask = Task.Run(() => { BackgroundWriter(); });
		}

		private void BackgroundWriter()
		{
			while(DoWriting)
			{
				string record;
				bool isEmpty = true;
				lock (QueueLock)
				{
					isEmpty = !LinksQueue.TryDequeue(out record);
				}
				if (isEmpty) {  Task.Delay(250).Wait(); continue;}
				Console.WriteLine(record);
			}
		}

		~ConsoleExporter()
		{
			DoWriting = false;
		}
	}
}
