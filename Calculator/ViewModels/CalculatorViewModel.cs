using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Calculator.ViewModels
{
    public class CalculatorViewModel : ReactiveObject
    {
        public CalculatorViewModel()
        {
            #region Type Commands
            AppendNumberCommand = ReactiveCommand.Create<string>((number) =>
            {
                CleanUp();

                if (BottomString == "0")
                {
                    BottomString = number;
                    return;
                }
                // Make sure minus sign won't count as number in char sequence
                if (BottomString.Length < 16 + (_negative ? 1 : 0))
                {
                    BottomString += number;
                }
            });

            ChangeSignCommand = ReactiveCommand.Create(() =>
            {
                CleanUp();

                if (!_negative)
                {
                    BottomString = "-" + BottomString;
                    _negative = true;
                }
                else
                {
                    BottomString = BottomString.Substring(1);
                    _negative = false;
                }
            });

            AddFloatingPointCommand = ReactiveCommand.Create(() =>
            {
                CleanUp();

                if (!_floatingPoint)
                {
                    BottomString += ",";
                    _floatingPoint = true;
                }
            });
            #endregion

            #region Operation Commands
            OperationCommand = ReactiveCommand.Create<string>((op) =>
            {
                deferredTopFieldCleanUp = false;
                deferredBottomFieldCleanUp = true;

                if (BottomString != "0")
                {
                    double bottomNumber = double.Parse(BottomString);
                    if (_op != "" && _result != 0)
                    {
                        _result = _op switch
                        {
                            "+" => _result + bottomNumber,
                            "-" => _result - bottomNumber,
                            "✕" => _result * bottomNumber,
                            "÷" => _result / bottomNumber
                        };
                    }
                    else
                    {
                        _result = bottomNumber;
                    }
                }
                _op = op;
                TopString = $"{_result} {op}";
            });

            ResultCommand = ReactiveCommand.Create(() =>
            {
                if (_result != 0)
                {
                    double bottomNumber = double.Parse(BottomString);
                    _result = _op switch
                    {
                        "+" => _result + bottomNumber,
                        "-" => _result - bottomNumber,
                        "✕" => _result * bottomNumber,
                        "÷" => _result / bottomNumber
                    };

                    TopString = $"{TopString} {BottomString} =";
                    BottomString = _result.ToString();

                    _result = 0;
                    deferredBottomFieldCleanUp = true;
                    deferredTopFieldCleanUp = true;
                }
            });
            #endregion

            #region Clear Commands
            RemoveCharCommand = ReactiveCommand.Create(() =>
            {
                if (BottomString.EndsWith(","))
                    _floatingPoint = false;
                
                if (BottomString != "0")
                    BottomString = BottomString.Remove(BottomString.Length - 1);

                if (BottomString == "")
                    BottomString = "0";
            });

            ClearBottomStringCommand = ReactiveCommand.Create(() =>
            {
                ClearBottomField();
            });

            ClearAllCommand = ReactiveCommand.Create(() =>
            {
                ClearBottomField();
                ClearTopField();
            });
            #endregion
        }
        public ReactiveCommand<string, Unit> AppendNumberCommand { get; }
        public ReactiveCommand<Unit, Unit> ChangeSignCommand { get; }
        public ReactiveCommand<Unit, Unit> AddFloatingPointCommand { get; }

        public ReactiveCommand<string, Unit> OperationCommand { get; }
        public ReactiveCommand<Unit, Unit> ResultCommand { get; }

        public ReactiveCommand<Unit, Unit> RemoveCharCommand { get; }
        public ReactiveCommand<Unit, Unit> ClearBottomStringCommand { get; }
        public ReactiveCommand<Unit, Unit> ClearAllCommand { get; }

        private double _result = 0;
        private string _op = "";

        private bool _negative = false;
        private bool _floatingPoint = false;

        private bool deferredBottomFieldCleanUp = false;
        private bool deferredTopFieldCleanUp = false;

        private string _topString = "";
        public string TopString
        {
            get => _topString;
            set => this.RaiseAndSetIfChanged(ref _topString, value);
        }

        private string _bottomString = "0";
        public string BottomString
        {
            get => _bottomString;
            set => this.RaiseAndSetIfChanged(ref _bottomString, value);
        }

        private void ClearBottomField()
        {
            BottomString = "0";
            _negative = false;
            _floatingPoint = false;
        }

        private void ClearTopField()
        {
            _result = 0;
            TopString = "";
        }

        private void CleanUp()
        {
            if (deferredBottomFieldCleanUp)
            {
                ClearBottomField();
                deferredBottomFieldCleanUp = false;
            }

            if (deferredTopFieldCleanUp)
            {
                ClearTopField();
                deferredTopFieldCleanUp = false;
            }
        }
    }
}
