﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Threading;

namespace Crawler
{
    public delegate void AnalyzePageFn(string html, out string nextUrl, out string data);
    public delegate void AnalyzeUrlsFn(string html, out List<string> urls, out string bookName, out string author, out string summer);

    class Program
    {

        public static readonly List<CrawlerConfig> MCrawlerParam = new List<CrawlerConfig>()
        {
             new CrawlerConfig(){Name="雪中悍刀行", MUrl=@"http://www.bxwx3.org/txt/48595/169891/htm", MPageFn =AnalyzeHtml.GetUrlAndData },
             new CrawlerConfig(){Name="雪中悍刀行-bqg", MUrl=@"http://www.biquge.info/2_2309/1157033.html", MPageFn =AnalyzeHtml.GetUrlAndData_biquge },
             new CrawlerConfig(){Name="雪中悍刀行-bqg2", MUrl=@"http://www.biquge.info/2_2309/", MPageFn =AnalyzeHtml.GetUrlAndData_biquge,MUrlsFn = AnalyzeHtml.GetAllUrls },
        };

        static void Main(string[] args)
        {
            Console.WriteLine("waiting command...");
            do
            {
                var str = Console.ReadLine().ToLower();
                switch (str)
                {
                    case "exit":
                        isStop = true;
                        break;
                    case "stop":
                        wc.Stop();
                        break;
                    case "run":
                        wc.Run();
                        break;
                    default:
                        Console.WriteLine("waiting command...");
                        break;
                }
                if (str == "exit")
                {
                    break;
                }
            } while (!isStop);
        }
        static WebCrawler wc;
        static bool isStop = false;
        static void Init()
        {
            string url = @"http://www.bxwx3.org/txt/48595/169891/htm";
            Console.WriteLine("Now loading " + url);
            wc = new WebCrawler(MCrawlerParam[1]);
            wc.OnDone += (obj, e) =>
            {
                isStop = true;
                wc.Stop();
                wc.Dispose();
            };
        }
    }
}
