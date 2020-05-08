﻿using HtmlAgilityPack;
using Mihap.CrawlerApi.Models;
using Mihap.CrawlerApi.Queue;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Mihap.CrawlerApi.Processing
{
	public delegate void OnChildLinkProcessedDelegate(List<Link> links);
	public delegate void OnChildLinkFoundDelegate(TaskData link);
	public delegate void OnLinkProcessedDelegate(Link link);
	public class ProcessingWorker
	{
		public bool DoProcessing { get; set; } = true;

		private HttpClient httpClient = new HttpClient();

		public event OnChildLinkProcessedDelegate OnChildLinkProcessed;
		public event OnLinkProcessedDelegate OnLinkProcessed;
		public event OnChildLinkFoundDelegate OnChildLinkFound;

		public void Start()
		{
			Task.Run(() => ProcessingFunction());
		}

		private void ProcessingFunction()
		{
			while (DoProcessing)
			{
				TaskData taskData = QueueManager.GetTask();
				if (taskData == null)
				{
					Thread.Sleep(250);
					continue;
				}
				var candidates = ProcessUrl(taskData);

				if (candidates != null && candidates.Count > 0)
					OnChildLinkProcessed?.Invoke(candidates);
			}
		}
		private List<Link> ProcessUrl(TaskData taskData)
		{
			var result = new List<Link>();
			var domen = (new Uri(taskData.Link.Url)).Host;
		
			try
			{
				WebRequest request = WebRequest.Create(taskData.Link.Url);
				request.Credentials = CredentialCache.DefaultCredentials;
				request.Headers.Add("User-Agent", "PostmanRuntime/7.24.0");
				WebResponse response = request.GetResponse();
				taskData.Link.ContentType = response.ContentType;


				int MaxDepth = WebCrawlerClient.Instance.settings.MaxDepth;

				// если глубина не превысила целевую
				// ищем ссылки глубже
				if (taskData.DepthLevel <= MaxDepth)
				{
					string responseString = "";

					// вычитыаем html
					using (var reader = new StreamReader(response.GetResponseStream()))
					{
						responseString = reader.ReadToEnd();
					}

					// парсим
					var childLinks = HtmlAgilityPack(responseString).ToList();

				}
			}
			catch (WebException ex)
			{
				taskData.Link.ContentType = "failed";
			}
			catch (Exception ex)
			{
				taskData.Link.ContentType = "failed";
			}
			finally
			{
				taskData.IsDone = true;
			}
			if (result.Count > 0)
				OnChildLinkProcessed?.Invoke(result);

			OnLinkProcessed?.Invoke(taskData.Link);
			return result;
		}
		/// <summary>
		/// Extract all anchor tags using Regex
		/// Method from https://github.com/forcewake/Benchmarks
		/// </summary>

		public IEnumerable<string> RegexMethod(string Html)
		{

			Regex reHref = new Regex(@"(?inx)
				<a \s [^>]*
					href \s* = \s*
						(?<q> ['""] )
							(?<url> [^""]+ )
						\k<q>
				[^>]* >");
			foreach (Match match in reHref.Matches(Html))
			{
				string link = match.Groups["url"].Value;
				if (Uri.TryCreate(link, UriKind.RelativeOrAbsolute, out Uri uriResult))
					yield return link;
			}
		}
		/// <summary>
		/// Extract all anchor tags using HtmlAgilityPack
		/// Method from https://github.com/forcewake/Benchmarks
		/// </summary>
		public IEnumerable<string> HtmlAgilityPack(string Html)
		{
			HtmlDocument htmlSnippet = new HtmlDocument();
			htmlSnippet.LoadHtml(Html);
			List<string> hrefTags = new List<string>();

			foreach (HtmlNode link in htmlSnippet.DocumentNode.SelectNodes("//a[@href]"))
			{
				HtmlAttribute att = link.Attributes["href"];
				yield return att.Value;
			}

		}

		public IEnumerable<string> MakeAbsolutUrls(IEnumerable<string> uris, string BaseUrl)
		{
			foreach(var uri in uris)
			{
				yield return "";
			}
		}
	}
}
