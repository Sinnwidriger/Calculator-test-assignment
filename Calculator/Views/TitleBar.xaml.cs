using Calculator.ViewModels;
using ReactiveUI;

namespace Calculator.Views
{
    /// <summary>
    /// Interaction logic for TitleBar.xaml
    /// </summary>
    public partial class TitleBar : ReactiveUserControl<TitleBarViewModel>
    {
        public TitleBar()
        {
            InitializeComponent();
        }
    }
}
