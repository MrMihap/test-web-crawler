using Mihap.CrawlerApi.Models;
using Mihap.CrawlerApi.Queue;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mihap.CrawlerApi.Processing
{
	public delegate void OnChildLinkProcessedDelegate(List<Link> links);
	public delegate void OnLinkProcessedDelegate(Link link);
	public class ProcessingWorker
	{
		public bool DoProcessing { get; set; } = true;

		private HttpClient httpClient = new HttpClient();

		public event OnChildLinkProcessedDelegate OnChildLinkProcessed;
		public event OnLinkProcessedDelegate OnLinkProcessed;

		public void Start()
		{
			Task.Run(() => ProcessingFunction());
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
				var candidates = ParseUrl(taskData);



				if (candidates != null && candidates.Count > 0)
					OnChildLinkProcessed?.Invoke(candidates);
			}
		}
		private List<Link> ParseUrl(TaskData taskData)
		{

			var result = new List<Link>();

			try
			{
				WebRequest request = WebRequest.Create(taskData.Link.Url);
				WebResponse response = request.GetResponse();
				string responseString = "";
				using (var reader = new StreamReader(response.GetResponseStream()))
				{
					responseString = reader.ReadToEnd();
				}
				string contentType = response.ContentType;
			}
			catch
			{
			}
			finally 
			{
				taskData.IsDone = true;
			}
			OnChildLinkProcessed?.Invoke(result);
			return result;
		}
		/// <summary>
		/// Extract all anchor tags using Regex
		/// Method from https://github.com/forcewake/Benchmarks
		/// </summary>

		public IEnumerable<string> RegexMethod(string Html)
		{
			List<string> hrefTags = new List<string>();

			Regex reHref = new Regex(@"(?inx)
				<a \s [^>]*
					href \s* = \s*
						(?<q> ['""] )
							(?<url> [^""]+ )
						\k<q>
				[^>]* >");
			foreach (Match match in reHref.Matches(Html))
			{
				hrefTags.Add(match.Groups["url"].ToString());
			}

			return hrefTags;
		}
	}
}
