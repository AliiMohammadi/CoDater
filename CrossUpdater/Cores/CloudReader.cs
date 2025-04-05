using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// .کلاس خواندن یک صفحه وب
/// این برنامه رو اولین بار اسکریپت کید برای اپدیت کردن برنامه خودش استفاده کرد.
/// اطلاعات رو داخل سایت گیت لب میزاشت و با استفاده از این کلاس اطلاعات رو میخوند
/// </summary>
public class CloudReader
{
    public string URL;

    /// <summary>
    /// .خوندن اطلاعات از یک صفحه وب
    /// ادرس پیش فرض
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public string Read()
    {
        return Read(URL);
    }
    /// <summary>
    /// خوندن اطلاعات از یک صفحه وب
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public string Read(string url)
    {
        try
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(url);
            StreamReader reader = new StreamReader(stream);

            string line;
            string alltext = "";

            while ((line = reader.ReadLine()) != null)
            {
                alltext += line + "\n";
            }

            return alltext;

        }
        catch
        {
            return null;
        }
    }
}