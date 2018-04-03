using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace CodeTool.Models
{
    public class ShowList : ViewModelBase
    {
        private List<string> name_list;
        public List<string> NameList
        {
            get { return name_list; }
            set
            {
                name_list = value;
                RaisePropertyChanged();
            }
        }

        private List<string>[] value_list;
        public List<string>[] ValueList
        {
            get { return value_list; }
            set
            {
                value_list = value;
                RaisePropertyChanged();
            }
        }

        public int ErrorNo = 0;
    }
}
