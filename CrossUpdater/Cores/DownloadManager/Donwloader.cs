using System;
using System.Net;

namespace CoDater.DownloadManager
{
    internal class Donwloader
    {
        public static void DownloadFile(string url, string savePath)
        {
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    webClient.DownloadFile(url, savePath);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
