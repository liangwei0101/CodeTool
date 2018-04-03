using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTool.Util
{
    public class WriteFile
    {
        public void CreateDirectory()
        {
            Directory.CreateDirectory("Sdk/Command/");
            Directory.CreateDirectory("Sdk/Model");
            Directory.CreateDirectory("Sdk/Request");
            Directory.CreateDirectory("Sdk/Response");
        }

        /// <summary>
        /// 创建Model文件
        /// </summary>
        public void CreateModel(string projectName, string functionId, string strs)
        {
            if(!string.IsNullOrWhiteSpace(projectName) && !string.IsNullOrWhiteSpace(functionId))
            {
                var fileName = @"./Sdk/Model/" + "F" + functionId + "_Info" + ".cs";
                var sw = new StreamWriter(fileName, false, Encoding.UTF8);
                var oldStrList = OldStrsListSet(strs);
                var strList = StrsSet(strs);
                var newStrList = NewStrsListSet(strList);

                sw.WriteLine("using HSUCF.WPF.Core;");
                sw.WriteLine("namespace" + " " + projectName + ".Sdk.Model");
                sw.WriteLine("{");
                sw.WriteLine("public class" + " " + "F" + functionId + "_Info" + " " + ": ObservableObject");
                sw.WriteLine("{");
                for (var i = 0; i < oldStrList.Length; i++)
                {
                    var type = "";
                    var notes = "";
                    var obj = ReadFile.DictionaryList.FirstOrDefault(s => s.Name == oldStrList[i]);
                    if (obj == null)
                    {
                        type = "string";
                    }
                    else
                    {
                        type = obj.Type;
                        notes = obj.Cname;
                    }

                    sw.WriteLine("private" + " " + type + " " + oldStrList[i] + " " + ";");

                    sw.WriteLine("/// <summary>");
                    if (!string.IsNullOrWhiteSpace(notes))
                    {
                        sw.WriteLine("/// " + notes);
                    }
                    else
                    {
                        sw.WriteLine("/// " + " ");
                    }
                    sw.WriteLine("/// </summary>");

                    sw.WriteLine("public" + " " + type + " " + newStrList[i]);
                    sw.WriteLine("{");
                    sw.WriteLine("get { return" + " " + oldStrList[i] + "; }");
                    sw.WriteLine("set");
                    sw.WriteLine("{");
                    sw.WriteLine(oldStrList[i] + " = value;");
                    sw.WriteLine("  RaisePropertyChanged(() =>" + newStrList[i] + ");");
                    sw.WriteLine("}");
                    sw.WriteLine("}");
                    sw.WriteLine("");
                }
                sw.WriteLine("}");
                sw.WriteLine("}");
                sw.Close();
            }
            else
            {
                var fileName = @"./Sdk/Model/" + "F" + functionId + "_Info" + ".cs";
                var sw = new StreamWriter(fileName, false, Encoding.UTF8);
                var oldStrList = OldStrsListSet(strs);
                var strList = StrsSet(strs);
                var newStrList = NewStrsListSet(strList);

                for (var i = 0; i < oldStrList.Length; i++)
                {
                    var type = "";
                    var notes = "";
                    var obj = ReadFile.DictionaryList.FirstOrDefault(s => s.Name == oldStrList[i]);
                    if (obj == null)
                    {
                        type = "string";
                    }
                    else
                    {
                        type = obj.Type;
                        notes = obj.Cname;
                    }

                    sw.WriteLine("private" + " " + type + " " + oldStrList[i] + " " + ";");

                    sw.WriteLine("/// <summary>");
                    if (!string.IsNullOrWhiteSpace(notes))
                    {
                        sw.WriteLine("/// " + notes);
                    }
                    else
                    {
                        sw.WriteLine("/// " + " ");
                    }
                    sw.WriteLine("/// </summary>");

                    sw.WriteLine("public" + " " + type + " " + newStrList[i]);
                    sw.WriteLine("{");
                    sw.WriteLine("get { return" + " " + oldStrList[i] + "; }");
                    sw.WriteLine("set");
                    sw.WriteLine("{");
                    sw.WriteLine(oldStrList[i] + " = value;");
                    sw.WriteLine("  RaisePropertyChanged(() =>" + newStrList[i] + ");");
                    sw.WriteLine("}");
                    sw.WriteLine("}");
                    sw.WriteLine("");
                }
                sw.Close();
            }
          
        }

        private string[] StrsSet(string strs)
        {
            if (strs.Contains(','))
            {
                string[] strsList = strs.Split(',');
                return strsList;
            }
            else
            {
                return new[] { strs };
            }
        }

        private string[] OldStrsListSet(string strs)
        {
            if (strs.Contains(","))
            {
                var tempList = strs.Split(',');
                return tempList;
            }
            else
            {
                return new[] { strs };
            }

        }

        private string[] NewStrsListSet(string[] strs)
        {
            string[] newStr = new string[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                if (strs[i].Contains("_"))
                {
                    var tempStr = "";
                    var tempList = strs[i].Split('_');
                    for (var j = 0; j < tempList.Length; j++)
                    {
                        tempList[j] = char.ToUpper(tempList[j][0]) + tempList[j].Substring(1);
                        tempStr += tempList[j];
                    }
                    newStr[i] = tempStr;
                }
                else
                {
                    newStr[i] =  System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(strs[i]);
                }
            }
            return newStr;
        }
    }
}
