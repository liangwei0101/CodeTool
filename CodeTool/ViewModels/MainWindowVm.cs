using System.Collections.Generic;
using System.Diagnostics;
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

    // Base class for MainWindow's ViewModels. All methods must be virtual. Default constructor must exist.
    //  Using this Base Class will allow xaml to bind variables to a concrete View Model at compile time
    public class MainWindowVm : ViewModelBase, IDisposable
    {
        #region 私有变量

        private Dictionary<string, string> _FieldNameList;
        List<string> _strList;
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
            if (string.IsNullOrWhiteSpace(ProjectName) || string.IsNullOrWhiteSpace(FunctionId) || string.IsNullOrWhiteSpace(InputStr))
                return;

            if (obj == "1")
            {
                ValiDation();
            }
        }

        private void WindowLoadedAction(MainWindow obj)
        {
            window = obj;
            _writeFile = new WriteFile();
            ReadFile.LoadFiledNameFile();
            _FieldNameList = ReadFile.FieldNameList;
        }

        private void InitDate()
        {
            ContentShow = "";
            ProjectName = "PBOX.User";
            FunctionId = "360516";
            InputStr = "branch_no,client_name";
            CardShow = Visibility.Visible;
            ContentShowFlag = Visibility.Collapsed;
            ManualGenerationShow = Visibility.Visible;
            FunctionIdGenerationShow = Visibility.Visible;
            ExcleGenerationShow = Visibility.Visible;
            _strList = new List<string>();
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

        private void ValiDation()
        {
            _writeFile.CreateDirectory();
            _writeFile.CreateModel(ProjectName, FunctionId, InputStr);
            _strList = ReadFile.ReadSdkModelFile(FunctionId);
            SetContentShow();
        }

        private void SetContentShow()
        {
            ContentShow = "";
            ContentShowFlag = Visibility.Visible;
            foreach (var item in _strList)
            {
                if(string.IsNullOrWhiteSpace(item))
                    continue;
                ContentShow += item + "\n";
            }
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
