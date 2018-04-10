using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using CodeTool.Models;
using CodeTool.Util;

namespace CodeTool.ViewModels
{
    using System;
    using GalaSoft.MvvmLight;
    using System.Windows;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using MaterialDesignThemes.Wpf;
    using Views;
    using CodeTool.Service;
    using System.Windows.Controls;

    // Base class for MainWindow's ViewModels. All methods must be virtual. Default constructor must exist.
    //  Using this Base Class will allow xaml to bind variables to a concrete View Model at compile time
    public class MainWindowVm : ViewModelBase, IDisposable
    {
        #region 私有变量

        private ShowList _showList;
        private Dictionary<string, string> _FieldNameList;
        List<string> _strModelList;
        List<string> _strResponseList;
        WriteFile _writeFile = null;
        MainWindow window = null;
        Flipper flipper = null;
        #endregion

        #region 构造函数
        public MainWindowVm()
        {
            InitDate();
        }
        #endregion

        public virtual void Dispose() { }

        #region 属性

        private Visibility _dataGridContentShow;
        public Visibility DataGridContentShow
        {
            get { return _dataGridContentShow; }
            set
            {
                _dataGridContentShow = value;
                RaisePropertyChanged();
            }
        }

        private string _functionIdGenerationStr;
        public string FunctionIdGenerationStr
        {
            get { return _functionIdGenerationStr; }
            set
            {
                _functionIdGenerationStr = value;
                RaisePropertyChanged();
            }
        }

        private Visibility _contentShowFlag;

        public Visibility ContentShowFlag
        {
            get { return _contentShowFlag; }
            set
            {
                _contentShowFlag = value;
                RaisePropertyChanged();
            }
        }

        private string _contentShow;

        public string ContentShow
        {
            get { return _contentShow; }
            set
            {
                _contentShow = value;
                RaisePropertyChanged();
            }
        }

        private string _projectName;
        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                _projectName = value;
                RaisePropertyChanged();
            }
        }

        private string _functionId;
        public string FunctionId
        {
            get { return _functionId; }
            set
            {
                _functionId = value;
                RaisePropertyChanged();
            }
        }

        private string _inputStr;
        public string InputStr
        {
            get { return _inputStr; }
            set
            {
                _inputStr = value;
                RaisePropertyChanged();
            }
        }

        private string _license;
        public string License
        {
            get { return _license; }
            set
            {
                _license = value;
                RaisePropertyChanged();
            }
        }

        private Visibility _cardShow;
        public Visibility CardShow
        {
            get { return _cardShow; }
            set
            {
                _cardShow = value;
                RaisePropertyChanged();
            }
        }

        private Visibility _manualGenerationShow;

        /// <summary>
        /// 手动生成
        /// </summary>
        public Visibility ManualGenerationShow
        {
            get { return _manualGenerationShow; }
            set
            {
                _manualGenerationShow = value;
                RaisePropertyChanged();
            }
        }

        private Visibility _functionIdGenerationShow;
        /// <summary>
        /// 功能号生成
        /// </summary>
        public Visibility FunctionIdGenerationShow
        {
            get { return _functionIdGenerationShow; }
            set
            {
                _functionIdGenerationShow = value;
                RaisePropertyChanged();
            }
        }

        private Visibility _excleGenerationShow;
        /// <summary>
        /// excle生成
        /// </summary>
        public Visibility ExcleGenerationShow
        {
            get { return _excleGenerationShow; }
            set
            {
                _excleGenerationShow = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region 命令

        public ICommand ResponseClickCommand => new RelayCommand(ResponseClickAction);

        public ICommand ModelClickCommand => new RelayCommand(ModelClickAction);

        public ICommand FunctionColumsCommand => new RelayCommand(FunctionIdGenerationAction);

        public ICommand SetFlagCommand => new RelayCommand<bool>(SetFlagAction);

        public ICommand CopyContentShowCommand => new RelayCommand(CopyContentShowAction);

        public ICommand BtnClickCommand => new RelayCommand<string>(BtnClickAction);

        public ICommand WindowLoadedCommand => new RelayCommand<MainWindow>(WindowLoadedAction);

        public ICommand CardIconClickCommand => new RelayCommand<string>(CardIconClickAction);

        public ICommand RollbackCommand => new RelayCommand(RollbackAction);

        public ICommand OpenFileCommand => new RelayCommand(OpenFileAction);

        #endregion

        #region 函数

        private void BtnClickAction(string obj)
        {
            if (obj == "1")
            {
                ManualGeneration();
            }
            else if(obj == "2")
            {
                ContentShowFlag = Visibility.Collapsed;
                FunctionGeneration();
            }
        }

        private void WindowLoadedAction(MainWindow obj)
        {
            window = obj;
            _writeFile = new WriteFile();
            ReadFile.LoadStdfieldsFile();
            ReadFile.LoadDatatypesFile();
            _FieldNameList = ReadFile.FieldNameList;
        }

        private void InitDate()
        {
            ContentShow = "";
            ProjectName = "PBOX.User";
            FunctionId = "360516";
            _strModelList = new List<string>();
            InputStr = "branch_no,client_name";
            CardShow = Visibility.Visible;
            ContentShowFlag = Visibility.Collapsed;
            ManualGenerationShow = Visibility.Visible;
            FunctionIdGenerationShow = Visibility.Visible;
            ExcleGenerationShow = Visibility.Visible;
            DataGridContentShow = Visibility.Collapsed;
            FunctionIdGenerationStr = "branch_no=8888,operator_no=8888,menu_id=362201";
        }

        private void CardIconClickAction(string carIndexs)
        {
            CardShow = Visibility.Collapsed;
            if (carIndexs == "1")
            {
                ManualGenerationShow = Visibility.Visible;
                FunctionIdGenerationShow = Visibility.Collapsed;
                ExcleGenerationShow = Visibility.Collapsed;
            }
            else if (carIndexs == "2")
            {
                ManualGenerationShow = Visibility.Collapsed;
                FunctionIdGenerationShow = Visibility.Visible;
                ExcleGenerationShow = Visibility.Collapsed;
            }
            else if (carIndexs == "3")
            {
                ManualGenerationShow = Visibility.Collapsed;
                FunctionIdGenerationShow = Visibility.Collapsed;
                ExcleGenerationShow = Visibility.Visible;
            }
            flipper.OnApplyTemplate();
        }

        private void RollbackAction()
        {
            if (CardShow == Visibility.Collapsed)
            {
                CardShow = Visibility.Visible;
            }
        }

        private void OpenFileAction()
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = ".dat";
            if (ofd.ShowDialog() == true)
            {
                License = ofd.FileName;
            }
        }

        private void CopyContentShowAction()
        {
            Clipboard.SetText(ContentShow);
            SetTipInfo("代码拷贝成功！", "OK");
        }

        private void ManualGeneration()
        {
            _writeFile.CreateDirectory();
            _writeFile.CreateModel(ProjectName, FunctionId, InputStr);
            _writeFile.CreateResponse(ProjectName, FunctionId, InputStr);
            _strModelList = ReadFile.ReadSdkModelFile(FunctionId);
            _strResponseList = ReadFile.ReadSdkResponseFile(FunctionId);
            SetContentShow();
        }

        private void FunctionGeneration()
        {
            SetFunctionIdSendStr();
        }

        private void SetContentShow()
        {
            ContentShow = "";
            ContentShowFlag = Visibility.Visible;
            foreach (var item in _strModelList)
            {
                if(string.IsNullOrWhiteSpace(item))
                    continue;
                ContentShow += item + "\n";
            }
        }

        private void ModelClickAction()
        {
            SetContentShow();
        }

        private void ResponseClickAction()
        {
            ContentShow = "";
            ContentShowFlag = Visibility.Visible;
            foreach (var item in _strResponseList)
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;
                ContentShow += item + "\n";
            }
        }

        /// <summary>
        /// 设置功能号发送数据
        /// </summary>
        private void SetFunctionIdSendStr()
        {
            var menuId = Convert.ToInt32(FunctionId);
            var codeToolService = new CodeToolService();
            codeToolService.FuntionSend(menuId, FunctionIdGenerationStr, args=>
            {
                _showList = args;
                SetData(window.DataGrid);
                DataGridContentShow = Visibility.Visible;
            });
        }

        private void FunctionIdGenerationAction()
        {
            var strs = SetFunctionColumsShow();
            _writeFile.CreateDirectory();
            _writeFile.CreateModel(ProjectName, FunctionId, strs);
            _strModelList = ReadFile.ReadSdkModelFile(FunctionId);
            SetContentShow();
            DataGridContentShow = Visibility.Collapsed;
        }

        private string SetFunctionColumsShow()
        {
            var strs = "";
            foreach (var item in _showList.NameList)
            {
                strs += item + ",";
            }
            strs = strs.Substring(0, strs.Length - 1);
            return strs;
        }

        private void SetFlagAction(bool flag)
        {
            if (flag) // 正面
            {
                ContentShowFlag = Visibility.Collapsed;
            }
            else
            {
                ContentShowFlag = Visibility.Visible;
            }
        }

        private void SetData(DataGrid dataGrid)
        {
            if (_showList?.NameList == null)
                return;

            DataTable dt = new DataTable();

            foreach (var itme in _showList.NameList)
            {
                dt.Columns.Add(itme, typeof(string));
            }

            for (int i = 0; i < _showList.ValueList.Length; i++)
            {
                DataRow row = dt.NewRow();
                for (int j = 0; j < _showList?.ValueList?.FirstOrDefault()?.Count; j++)
                {
                    row[_showList.NameList[j]] = _showList.ValueList[i][j];
                }
                dt.Rows.Add(row);
            }

            dataGrid.ItemsSource = dt.DefaultView;
        }

        private void SetTipInfo(string content, string title)
        {
            window.SnackbarThree.MessageQueue.Enqueue(
                content,
                title,
                param => Trace.WriteLine("Actioned: " + param),
                title,
                true);
        }

        #endregion
    }
}
