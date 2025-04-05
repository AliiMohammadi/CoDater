using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
