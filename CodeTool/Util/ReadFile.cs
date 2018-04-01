using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodeTool.Util
{
    public class ReadFile
    {
        public static Dictionary<string, string> FieldNameList;

        public static List<string> ReadSdkModelFile(string functionId)
        {
            var tempList = new List<string>();
            string path = @"./Sdk/Model/"+ "F"+ functionId+ "_Info.cs";
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(fs);
            String line;
            while ((line = streamReader.ReadLine())!= null)
            {
                tempList.Add(line);
            }
            return tempList;
        }

        public static void LoadFiledNameFile()
        {
            FieldNameList = new Dictionary<string, string>();

            //将XML文件加载进来
            XDocument document = XDocument.Load("fieldname.xml");
            //获取根元素下的所有子元素
            XElement root = document.Root;
            IEnumerable<XElement> enumerable = root.Elements("FieldItem");
            foreach (var item in enumerable)
            {
                var key = item.Attribute("english_name")?.Value;
                var value = item.Attribute("entry_name")?.Value;
                if (key != null) FieldNameList.Add(key, value);
            }
        }
    }
}
