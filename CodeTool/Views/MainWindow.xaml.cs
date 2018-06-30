namespace CodeTool.Views
{
    using ICSharpCode.AvalonEdit.Folding;
    using MaterialDesignThemes.Wpf;
    using System;
    using System.Windows;
    using System.Windows.Documents;
    using ViewModels;

    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        private readonly FoldingManager _foldingManager;
        private readonly MainWindowVm mainWindowVm;

        public MainWindow(MainWindowVm mainWindowVm)
        {
            this.mainWindowVm = mainWindowVm;

            DataContext = mainWindowVm;
            Closing += (sender, e) => { Dispose(); };
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

        }

        public void Dispose()
        {
            mainWindowVm.Dispose();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            card.Visibility = Visibility.Collapsed;
            FunctionGeneration.Visibility = Visibility.Collapsed;
            ExcleGeneration.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FunctionGeneration.Margin = new Thickness(18,0,3,0);
            card.Visibility = Visibility.Collapsed;
            ManualGeneration.Visibility = Visibility.Collapsed;
            ExcleGeneration.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ExcleGeneration.Margin = new Thickness(18, 0, 3, 0);
            card.Visibility = Visibility.Collapsed;
            ManualGeneration.Visibility = Visibility.Collapsed;
            FunctionGeneration.Visibility = Visibility.Collapsed;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            card.Visibility = Visibility.Visible;
            ManualGeneration.Visibility = Visibility.Visible;
            FunctionGeneration.Visibility = Visibility.Visible;
            ExcleGeneration.Visibility = Visibility.Visible;

            FunctionGeneration.Margin = new Thickness(8,0,0,0);
            ExcleGeneration.Margin = new Thickness(9,0,0,0);

            ((MainWindowVm)this.DataContext).SetFlagCommand.Execute(true);
        }
    }
}
