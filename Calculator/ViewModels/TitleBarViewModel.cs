using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Calculator.ViewModels
{
    public class TitleBarViewModel : ReactiveObject
    {
        public TitleBarViewModel()
        {
            MinimizeCommand = ReactiveCommand.Create(() =>
            {
                // TO DO correct minimize state detection
                // WindowState = WindowState.Minimized;
            });

            MaximizeCommand = ReactiveCommand.Create(() =>
            {
                WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
            });

            ExitCommand = ReactiveCommand.Create(() =>
            {
                Environment.Exit(0);
            });
        }
        public string Title { get; set; } = "Calculator";

        public ReactiveCommand<Unit, Unit> MinimizeCommand { get; }
        public ReactiveCommand<Unit, Unit> MaximizeCommand { get; }
        public ReactiveCommand<Unit, Unit> ExitCommand { get; }


        private WindowState _windowState = WindowState.Normal;
        public WindowState WindowState
        {
            get => _windowState;
            set => this.RaiseAndSetIfChanged(ref _windowState, value);
        }
    }
}
