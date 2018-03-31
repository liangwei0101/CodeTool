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

        private string license;
        public string License
        {
            get { return license; }
            set
            {
                license = value;
                RaisePropertyChanged();
            }
        }

        private Visibility card_show;
        public Visibility CardShow
        {
            get { return card_show; }
            set
            {
                card_show = value;
                RaisePropertyChanged();
            }
        }

        private Visibility manual_generation_show;

        /// <summary>
        /// 手动生成
        /// </summary>
        public Visibility ManualGenerationShow
        {
            get { return manual_generation_show; }
            set
            {
                manual_generation_show = value;
                RaisePropertyChanged();
            }
        }

        private Visibility functionId_generation_show;
        /// <summary>
        /// 功能号生成
        /// </summary>
        public Visibility FunctionIdGenerationShow
        {
            get { return functionId_generation_show; }
            set
            {
                functionId_generation_show = value;
                RaisePropertyChanged();
            }
        }

        private Visibility excle_generation_show;
        /// <summary>
        /// excle生成
        /// </summary>
        public Visibility ExcleGenerationShow
        {
            get { return excle_generation_show; }
            set
            {
                excle_generation_show = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region 命令

        public ICommand WindowLoadedCommand
        {
            get
            {
                return new RelayCommand<MainWindow>(WindowLoadedAction);
            }
        }

        public ICommand CardIconClickCommand
        {
            get
            {
                return new RelayCommand<string>(CardIconClickAction);
            }
        }

        public ICommand RollbackCommand
        {
            get
            {
                return new RelayCommand(RollbackAction);
            }
        }

        public ICommand OpenFileCommand
        {
            get
            {
                return new RelayCommand(OpenFileAction);
            }
        }

        #endregion

        #region 函数

        private void WindowLoadedAction(MainWindow obj)
        {
            window = obj;
        }

        private void InitDate()
        {
            License = "lice";
            CardShow = Visibility.Visible;
            ManualGenerationShow = Visibility.Visible;
            FunctionIdGenerationShow = Visibility.Visible;
            ExcleGenerationShow = Visibility.Visible;
        }

        private void CardIconClickAction(string carIndexs)
        {
            CardShow = Visibility.Collapsed;
            if(carIndexs == "1")
            {
                ManualGenerationShow = Visibility.Visible;
                FunctionIdGenerationShow = Visibility.Collapsed;
                ExcleGenerationShow = Visibility.Collapsed;
            }
            else if(carIndexs == "2")
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

        #endregion
    }
}
