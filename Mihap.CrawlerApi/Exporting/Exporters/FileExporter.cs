using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Mihap.CrawlerApi.Exporting.Exporters
{
	public class FileExporter : BaseExporter
	{
		private string Path { get; set; }
		private volatile bool DoWriting = true;

		public FileExporter(string Path)
		{
			this.Path = Path;

			#region SOME PARANOIA
			if (File.Exists(Path))
				try
				{
					File.Delete(Path);
				}
				catch
				{ 
				}
			File.WriteAllText(Path, "Hello, reader!");
			#endregion

			BackgroundWorkerTask = Task.Run(() => { BackgroundWriter(); });
		}
		

		private void BackgroundWriter()
		{
			string record;

			while (DoWriting)
			{
				bool isEmpty = true;
				lock (QueueLock)
				{
					isEmpty = !LinksQueue.TryDequeue(out record);
				}
				if (isEmpty) 
				{  
					Task.Delay(250).Wait(); 
					continue;
				}
				File.AppendAllLines(this.Path, new string[] { record });
			}
		}
		~FileExporter()
		{
			DoWriting = false;
		}
	}
}
