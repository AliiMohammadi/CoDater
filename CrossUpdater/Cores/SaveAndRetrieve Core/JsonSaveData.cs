using System;
using System.IO;
using System.Web.Script.Serialization;

namespace SaveAndRetrieve
    {
        /// <summary>
        /// ذخیره و یا بازیابی اطلاعات با استفاده از تکنولوژی جیسون
        /// </summary>
        public class JsonSaveData
        {
            public void SaveData(object value,string path)
            {
                File.WriteAllText(path, new JavaScriptSerializer().Serialize(value));
            }
            public void AppendData(object value, string path)
            {
                File.AppendAllText(path, new JavaScriptSerializer().Serialize(value));
            }
            public T LoadData<T>(string path)
            {
                if (File.Exists(path))
                    return new JavaScriptSerializer().Deserialize<T>(File.ReadAllText(path));
                else
                    throw new Exception("File not found.");
            }
            public string ReadFile(string path)
            {
                return File.ReadAllText(path);
            }
        }
    }
