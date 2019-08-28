using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AurelienRibon.Ui.SyntaxHighlightBox;
using MahApps.Metro.Controls;

namespace VNS {
    public partial class MainWindow : MetroWindow {
        private readonly MemoryContainer _memoryContainer = new MemoryContainer();
        private static readonly Regex NumberRegex = new Regex("[^0-9]+");
        private static readonly Color BlueColor = Color.FromRgb(135, 206, 255);
        private string[] _codeLines;
        public WindowBindings WindowBindings { get; set; }

        public MainWindow() {
            WindowBindings = new WindowBindings();
            InitializeComponent();
            CodeBox.CurrentHighlighter = HighlighterManager.Instance.Highlighters["ASM"];
            this.DataContext = _memoryContainer;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e) {
            var textBox = (TextBox)e.Source;
            MemoryData.SelectedItem = textBox.DataContext;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            e.Handled = NumberRegex.IsMatch(e.Text);
        }

        private void DelaySizer_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            try {
                MsLabel.Content = $"{(int)e.NewValue} ms";
            } catch { // ignored
            }
        }

        private async void StartBtn_Click(object sender, RoutedEventArgs e) {
            WindowBindings.IsRunning = true;
            await Start();
            WindowBindings.IsRunning = false;
        }

        public async Task ProcessLine(string line) {
            if (line.Length == 0) return;
            var comandArgs = line.Split(new[] { ' ' }, 2);
            if(comandArgs.Length != 2) return;
            WindowBindings.CurrentInstruction = comandArgs[0];
            WindowBindings.CurrentInstructionData = comandArgs[1];
            WindowBindings.CurrentInstructionBrush = new SolidColorBrush(BlueColor);
            await Task.Delay((int)DelaySizer.Value);
            WindowBindings.CurrentInstructionBrush = null;
            var args = comandArgs[1].Replace(" ", "").Split(',');
            if (args.Length == 1) {
                WindowBindings.DecoderBrush = new SolidColorBrush(BlueColor);
                await Task.Delay((int)DelaySizer.Value);
                WindowBindings.DecoderBrush = null;
                switch (comandArgs[0]) {
                    case "LOD":
                        WindowBindings.AluRegister1 = "";
                        WindowBindings.AluRegister2 = "";
                        WindowBindings.AluRegister1Brush = new SolidColorBrush(BlueColor);
                        WindowBindings.AluRegister2Brush = new SolidColorBrush(BlueColor);
                        await Task.Delay((int)DelaySizer.Value);
                        WindowBindings.AluRegister1Brush = null;
                        WindowBindings.AluRegister2Brush = null;
                        WindowBindings.AluMethod = "↓";
                        WindowBindings.AluMethodBrush = new SolidColorBrush(BlueColor);
                        await Task.Delay((int)DelaySizer.Value);
                        WindowBindings.AluMethodBrush = null;
                        WindowBindings.Accumulator = _memoryContainer.GetValue(args[0]);
                        WindowBindings.AccumulatorBrush = new SolidColorBrush(BlueColor);
                        await Task.Delay((int)DelaySizer.Value);
                        WindowBindings.AccumulatorBrush = null;
                        break;
                    case "STO":
                        WindowBindings.AluRegister1 = "";
                        WindowBindings.AluRegister2 = "";
                        WindowBindings.AluRegister1Brush = new SolidColorBrush(BlueColor);
                        WindowBindings.AluRegister2Brush = new SolidColorBrush(BlueColor);
                        await Task.Delay((int)DelaySizer.Value);
                        WindowBindings.AluRegister1Brush = null;
                        WindowBindings.AluRegister2Brush = null;
                        WindowBindings.AluMethod = "↑";
                        WindowBindings.AluMethodBrush = new SolidColorBrush(BlueColor);
                        await Task.Delay((int)DelaySizer.Value);
                        WindowBindings.AluMethodBrush = null;
                        _memoryContainer.SetValue(args[0], WindowBindings.Accumulator);
                        WindowBindings.AccumulatorBrush = new SolidColorBrush(BlueColor);
                        await Task.Delay((int)DelaySizer.Value);
                        WindowBindings.AccumulatorBrush = null;
                        break;
                    case "MOV":

                        break;
                    case "ADD":
                        WindowBindings.AluRegister1 = WindowBindings.Accumulator.ToString();
                        WindowBindings.AluRegister1Brush = new SolidColorBrush(BlueColor);
                        await Task.Delay((int)DelaySizer.Value);
                        WindowBindings.AluRegister1Brush = null;
                        WindowBindings.AluRegister2 = _memoryContainer.GetValue(args[0]).ToString();
                        WindowBindings.AluRegister2Brush = new SolidColorBrush(BlueColor);
                        await Task.Delay((int)DelaySizer.Value);
                        WindowBindings.AluRegister2Brush = null;
                        WindowBindings.AluMethod = "+";
                        WindowBindings.AluMethodBrush = new SolidColorBrush(BlueColor);
                        await Task.Delay((int)DelaySizer.Value);
                        WindowBindings.AluMethodBrush = null;
                        WindowBindings.Accumulator += _memoryContainer.GetValue(args[0]);
                        WindowBindings.AccumulatorBrush = new SolidColorBrush(BlueColor);
                        await Task.Delay((int)DelaySizer.Value);
                        WindowBindings.AccumulatorBrush = null;
                        break;
                    case "SUB":
                        WindowBindings.AluRegister1 = WindowBindings.Accumulator.ToString();
                        WindowBindings.AluRegister1Brush = new SolidColorBrush(BlueColor);
                        await Task.Delay((int)DelaySizer.Value);
                        WindowBindings.AluRegister1Brush = null;
                        WindowBindings.AluRegister2 = _memoryContainer.GetValue(args[0]).ToString();
                        WindowBindings.AluRegister2Brush = new SolidColorBrush(BlueColor);
                        await Task.Delay((int)DelaySizer.Value);
                        WindowBindings.AluRegister2Brush = null;
                        WindowBindings.AluMethod = "-";
                        WindowBindings.AluMethodBrush = new SolidColorBrush(BlueColor);
                        await Task.Delay((int)DelaySizer.Value);
                        WindowBindings.AluMethodBrush = null;
                        WindowBindings.Accumulator -= _memoryContainer.GetValue(args[0]);
                        WindowBindings.AccumulatorBrush = new SolidColorBrush(BlueColor);
                        await Task.Delay((int)DelaySizer.Value);
                        WindowBindings.AccumulatorBrush = null;
                        break;
                    case "MUL":
                        WindowBindings.AluRegister1 = WindowBindings.Accumulator.ToString();
                        WindowBindings.AluRegister1Brush = new SolidColorBrush(BlueColor);
                        await Task.Delay((int)DelaySizer.Value);
                        WindowBindings.AluRegister1Brush = null;
                        WindowBindings.AluRegister2 = _memoryContainer.GetValue(args[0]).ToString();
                        WindowBindings.AluRegister2Brush = new SolidColorBrush(BlueColor);
                        await Task.Delay((int)DelaySizer.Value);
                        WindowBindings.AluRegister2Brush = null;
                        WindowBindings.AluMethod = "*";
                        WindowBindings.AluMethodBrush = new SolidColorBrush(BlueColor);
                        await Task.Delay((int)DelaySizer.Value);
                        WindowBindings.AluMethodBrush = null;
                        WindowBindings.Accumulator *= _memoryContainer.GetValue(args[0]);
                        WindowBindings.AccumulatorBrush = new SolidColorBrush(BlueColor);
                        await Task.Delay((int)DelaySizer.Value);
                        WindowBindings.AccumulatorBrush = null;
                        break;
                    case "DIV":
                        WindowBindings.AluRegister1 = WindowBindings.Accumulator.ToString();
                        WindowBindings.AluRegister1Brush = new SolidColorBrush(BlueColor);
                        await Task.Delay((int)DelaySizer.Value);
                        WindowBindings.AluRegister1Brush = null;
                        WindowBindings.AluRegister2 = _memoryContainer.GetValue(args[0]).ToString();
                        WindowBindings.AluRegister2Brush = new SolidColorBrush(BlueColor);
                        await Task.Delay((int)DelaySizer.Value);
                        WindowBindings.AluRegister2Brush = null;
                        WindowBindings.AluMethod = "/";
                        WindowBindings.AluMethodBrush = new SolidColorBrush(BlueColor);
                        await Task.Delay((int)DelaySizer.Value);
                        WindowBindings.AluMethodBrush = null;
                        WindowBindings.Accumulator /= _memoryContainer.GetValue(args[0]);
                        WindowBindings.AccumulatorBrush = new SolidColorBrush(BlueColor);
                        await Task.Delay((int)DelaySizer.Value);
                        WindowBindings.AccumulatorBrush = null;
                        break;
                    default:
                        break;
                }
            }
            /* else if (comandArgs.Length == 2) {
                var args = comandArgs[1].Replace(" ", "").Split(',');
                switch (comandArgs[0]) {
                    case "LOD":

                        break;
                    case "STO":

                        break;
                    case "MOV":
                        _memoryContainer.SetValue(args[0], _memoryContainer.GetValue(args[1]));
                        break;
                    case "ADD":
                        _memoryContainer.SetValue(args[0], _memoryContainer.GetValue(args[0]) + _memoryContainer.GetValue(args[1]));
                        break;
                    case "SUB":
                        _memoryContainer.SetValue(args[0], _memoryContainer.GetValue(args[0]) - _memoryContainer.GetValue(args[1]));
                        break;
                    case "DIV":
                        _memoryContainer.SetValue(args[0], _memoryContainer.GetValue(args[0]) / _memoryContainer.GetValue(args[1]));
                        break;
                    case "MUL":
                        _memoryContainer.SetValue(args[0], _memoryContainer.GetValue(args[0]) * _memoryContainer.GetValue(args[1]));
                        break;
                    case "LDA":
                        break;
                    case "SCA":
                        break;
                    default:
                        break;
                }
            } else {

            }*/
        }

        public async Task Start() {
            if (_codeLines == null) {
                MessageBox.Show("No valid code has been found.", "CPU Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (WindowBindings.CurrentLine >= _codeLines.Length) WindowBindings.CurrentLine = 1;
            WindowBindings.LinesBrush = new SolidColorBrush(Colors.LawnGreen);
            foreach (var t in _codeLines) {
                if (!WindowBindings.IsRunning) {
                    WindowBindings.IsRunning = false;
                    WindowBindings.LinesBrush = new SolidColorBrush(Colors.Red);
                    await Task.Delay(500);
                    WindowBindings.LinesBrush = new SolidColorBrush(Colors.Gray);
                    return;
                }
                await NextStep();
                if (WindowBindings.CurrentLine > _codeLines.Length  + 1) {
                    WindowBindings.CurrentInstruction = "";
                    WindowBindings.AluRegister1 = "";
                    WindowBindings.AluRegister2 = "";
                    WindowBindings.AluMethod = "";
                    return;
                }
                await Task.Delay((int)DelaySizer.Value);
            }
            WindowBindings.LinesBrush = new SolidColorBrush(Colors.Gray);
        }

        public async Task NextStep() {
            if (_codeLines == null) {
                MessageBox.Show("No valid code has been found.", "CPU Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (WindowBindings.CurrentLine >= _codeLines.Length + 1) {
                return;
            }
            await IncreasePc();
            await ProcessLine(_codeLines[WindowBindings.CurrentLine - 2]);
        }

        public async Task IncreasePc() {
            WindowBindings.IncrementLineBrush = new SolidColorBrush(BlueColor);
            await Task.Delay((int)DelaySizer.Value);
            WindowBindings.IncrementLineBrush = null;
            WindowBindings.CurrentLine++;
            WindowBindings.CurrentLineBrush = new SolidColorBrush(BlueColor);
            await Task.Delay((int)DelaySizer.Value);
            WindowBindings.CurrentLineBrush = null;
        }

        private void CodeBox_TextChanged(object sender, TextChangedEventArgs e) {
            _codeLines = CodeBox.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }

        private async void NextBtn_Click(object sender, RoutedEventArgs e) {
            WindowBindings.IsRunning = true;
            WindowBindings.LinesBrush = new SolidColorBrush(Colors.DodgerBlue);
            await NextStep();
            WindowBindings.LinesBrush = new SolidColorBrush(Colors.Gray);
            WindowBindings.IsRunning = false;
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e) {
            WindowBindings.IsRunning = false;
        }

        private void NewProject_Click(object sender, RoutedEventArgs e) {
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void Exit_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void OpenProject_Click(object sender, RoutedEventArgs e) {
            throw new NotImplementedException();
        }

        private void SaveProject_Click(object sender, RoutedEventArgs e) {
            throw new NotImplementedException();
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e) {
            throw new NotImplementedException();
        }

        private void ViewHelp_Click(object sender, RoutedEventArgs e) {
            throw new NotImplementedException();
        }

        private void AboutVNS_Click(object sender, RoutedEventArgs e) {
            throw new NotImplementedException();
        }
    }
}
