using Calculator.ViewModels;
using ReactiveUI;
using System.Windows;
using System;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();

            (titleBar.DataContext as TitleBarViewModel)
                .WhenAnyValue(vm => vm.WindowState)
                .Subscribe((state) =>
                {
                    this.WindowState = state;
                });
        }
    }
}
