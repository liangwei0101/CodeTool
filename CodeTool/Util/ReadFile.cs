using CodeTool.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace CodeTool.Util
{
    public class ReadFile
    {
        public static List<Dictionary> DictionaryList;

        public static Dictionary<string, string> FieldNameList;

        public static List<string> ReadSdkModelFile(string functionId)
        {
            var tempList = new List<string>();
            string path = @"./Sdk/Model/" + "F" + functionId + "_Info.cs";
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(fs);
            String line;
            while ((line = streamReader.ReadLine()) != null)
            {
                tempList.Add(line);
            }
            return tempList;
        }

        public static List<string> ReadSdkResponseFile(string functionId)
        {
            var tempList = new List<string>();
            string path = @"./Sdk/Response/" + "F" + functionId + "_Response.cs";
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(fs);
            String line;
            while ((line = streamReader.ReadLine()) != null)
            {
                tempList.Add(line);
            }
            return tempList;
        }

        public static void LoadStdfieldsFile()
        {
            DictionaryList = new List<Dictionary>();
            FieldNameList = new Dictionary<string, string>();

            try
            {
                XDocument document1 = XDocument.Load("stdfields.xml");
                document1 = null;
            }
            //将XML文件加载进来
            catch
            {
                MessageBox.Show("请先把标准字段文件：stdfields.xml放于bin目录下");
                return;
            }

            //将XML文件加载进来
            XDocument document = XDocument.Load("stdfields.xml");
            //获取根元素下的所有子元素
            XElement root = document.Root;
            IEnumerable<XElement> enumerable = root.Elements("stdfield");
            foreach (var item in enumerable)
            {
                var dictionary = new Dictionary
                {
                    Name = item.Attribute("name")?.Value,
                    Cname = item.Attribute("cname")?.Value,
                    HsType = item.Attribute("type")?.Value
                };
                DictionaryList.Add(dictionary);
            }
        }

        /// <summary>
        /// 加载标准字段
        /// </summary>
        public static void LoadDatatypesFile()
        {
            FieldNameList = new Dictionary<string, string>();
            try
            {
                XDocument document1 = XDocument.Load("datatypes.xml");
                document1 = null;
            }
            //将XML文件加载进来
            catch
            {
                MessageBox.Show("请先把标准字段文件：datatypes.xml放于bin目录下");
                return;
            }

            XDocument document = XDocument.Load("datatypes.xml");
            //获取根元素下的所有子元素
            XElement root = document.Root;
            IEnumerable<XElement> enumerable = root.Elements("userType");
            foreach (var item in enumerable)
            {
                var key = item.Attribute("name")?.Value;
                var enumerableTemp = root.Elements("userType").Elements("language");
                foreach (var item1 in enumerableTemp)
                {
                    var temp = item.Elements("language").Elements("map");
                    var value = temp.Attributes("value").FirstOrDefault().Value;
                    var flag = FieldNameList.ContainsKey(key);
                    if (!flag)
                    {
                        FieldNameList.Add(key, value);
                    }
                }
            }

            for (int i = 0; i < DictionaryList.Count; i++)
            {
                var obj = FieldNameList.FirstOrDefault(s => s.Key == DictionaryList[i].HsType);
                if(obj.Key != null)
                {
                    if (obj.Value != "int" && obj.Value != "double" && obj.Value != "char")
                    {
                        if (DictionaryList[i] != null)
                        {
                            DictionaryList[i].Type = "string";
                        }
                    }
                    else
                    {
                        if (DictionaryList[i] != null)
                        {
                            DictionaryList[i].Type = obj.Value;
                        }
                    }
                }
            }
        }
    }
}
