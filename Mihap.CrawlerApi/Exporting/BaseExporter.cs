using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mihap.CrawlerApi.Exporting
{
	public abstract class BaseExporter
	{

		private protected object QueueLock = new object();
		private protected Queue<string> LinksQueue = new Queue<string>();

		private protected Task BackgroundWorkerTask;

		public virtual void RecieveNewLinkRecord(string record)
		{
			Task.Run(() =>
			{
				var linkrecord = record;
				lock (QueueLock)
				{
					LinksQueue.Enqueue(linkrecord);
				}
			});
		}
	}
}
