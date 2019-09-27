using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AurelienRibon.Ui.SyntaxHighlightBox;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace VNS {
    public partial class MainWindow : MetroWindow {
        private readonly MemoryContainer _memoryContainer = new MemoryContainer();
        private static readonly Regex NumberRegex = new Regex("[^0-9-]+");
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
            StartBtn.IsEnabled = false;
            NextBtn.IsEnabled = false;
            WindowBindings.IsRunning = true;
            await Start();
            WindowBindings.IsRunning = false;
            StartBtn.IsEnabled = true;
            NextBtn.IsEnabled = true;
        }

        public async Task ProcessLine(string line) {
            CodeBox.Focus();
            var lineIndex = WindowBindings.CurrentLine - 2;
            var superIndex = 0;
            for (var i = 0; i < lineIndex + 1; i++) {
                if (i == 0) {
                    CodeBox.Select(0, _codeLines[i].Length + 2);
                    superIndex += _codeLines[i].Length + 2;
                }
                else {
                    CodeBox.Select(superIndex, _codeLines[i].Length + 2);
                    superIndex += _codeLines[i].Length + 2;
                }
            }
            if (line.Length == 0) return;
            var commandArgs = line.Split(new[] { ' ' }, 2);
            WindowBindings.CurrentInstruction = commandArgs[0];
            if(commandArgs.Length >= 2) WindowBindings.CurrentInstructionData = commandArgs[1];
            WindowBindings.CurrentInstructionBrush = new SolidColorBrush(BlueColor);
            if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
            WindowBindings.CurrentInstructionBrush = null;
            var args = new string[] { };
            if (commandArgs.Length >= 2) args = commandArgs[1].Replace(" ", "").Split(',');
            WindowBindings.DecoderBrush = new SolidColorBrush(BlueColor);
            if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
            WindowBindings.DecoderBrush = null;
            switch (commandArgs[0]) {
                case "LOD":
                    WindowBindings.AluRegister1 = "";
                    WindowBindings.AluRegister2 = "";
                    WindowBindings.AluRegister1Brush = new SolidColorBrush(BlueColor);
                    WindowBindings.AluRegister2Brush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AluRegister1Brush = null;
                    WindowBindings.AluRegister2Brush = null;
                    WindowBindings.AluMethod = "↓";
                    WindowBindings.AluMethodBrush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AluMethodBrush = null;
                    WindowBindings.Accumulator = _memoryContainer.GetValue(args[0]);
                    WindowBindings.AccumulatorBrush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AccumulatorBrush = null;
                    break;
                case "STO":
                    WindowBindings.AluRegister1 = "";
                    WindowBindings.AluRegister2 = "";
                    WindowBindings.AluRegister1Brush = new SolidColorBrush(BlueColor);
                    WindowBindings.AluRegister2Brush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AluRegister1Brush = null;
                    WindowBindings.AluRegister2Brush = null;
                    WindowBindings.AluMethod = "↑";
                    WindowBindings.AluMethodBrush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AluMethodBrush = null;
                    _memoryContainer.SetValue(args[0], WindowBindings.Accumulator);
                    WindowBindings.AccumulatorBrush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AccumulatorBrush = null;
                    break;
                case "ADD":
                    WindowBindings.AluRegister1 = WindowBindings.Accumulator.ToString();
                    WindowBindings.AluRegister1Brush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AluRegister1Brush = null;
                    if (args.Length > 0 && args[0][0] == '#') WindowBindings.AluRegister2 = args[0].Remove(0, 1);
                    else WindowBindings.AluRegister2 = _memoryContainer.GetValue(args[0]).ToString();
                    WindowBindings.AluRegister2Brush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AluRegister2Brush = null;
                    WindowBindings.AluMethod = "+";
                    WindowBindings.AluMethodBrush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AluMethodBrush = null;
                    if (args.Length > 0 && args[0][0] == '#' && int.TryParse(args[0].Remove(0, 1), out var result))
                        WindowBindings.Accumulator += result;
                    else WindowBindings.Accumulator += _memoryContainer.GetValue(args[0]);
                    WindowBindings.AccumulatorBrush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AccumulatorBrush = null;
                    break;
                case "SUB":
                    WindowBindings.AluRegister1 = WindowBindings.Accumulator.ToString();
                    WindowBindings.AluRegister1Brush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AluRegister1Brush = null;
                    if (args.Length > 0 && args[0][0] == '#') WindowBindings.AluRegister2 = args[0].Remove(0, 1);
                    else WindowBindings.AluRegister2 = _memoryContainer.GetValue(args[0]).ToString();
                    WindowBindings.AluRegister2Brush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AluRegister2Brush = null;
                    WindowBindings.AluMethod = "-";
                    WindowBindings.AluMethodBrush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AluMethodBrush = null;
                    if (args.Length > 0 && args[0][0] == '#' && int.TryParse(args[0].Remove(0, 1), out var result2))
                        WindowBindings.Accumulator -= result2;
                    else WindowBindings.Accumulator -= _memoryContainer.GetValue(args[0]);
                    WindowBindings.AccumulatorBrush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AccumulatorBrush = null;
                    break;
                case "MUL":
                    WindowBindings.AluRegister1 = WindowBindings.Accumulator.ToString();
                    WindowBindings.AluRegister1Brush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AluRegister1Brush = null;
                    if (args.Length > 0 && args[0][0] == '#') WindowBindings.AluRegister2 = args[0].Remove(0, 1);
                    else WindowBindings.AluRegister2 = _memoryContainer.GetValue(args[0]).ToString();
                    WindowBindings.AluRegister2Brush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AluRegister2Brush = null;
                    WindowBindings.AluMethod = "*";
                    WindowBindings.AluMethodBrush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AluMethodBrush = null;
                    if (args.Length > 0 && args[0][0] == '#' && int.TryParse(args[0].Remove(0, 1), out var result3))
                        WindowBindings.Accumulator *= result3;
                    else WindowBindings.Accumulator *= _memoryContainer.GetValue(args[0]);
                    WindowBindings.AccumulatorBrush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AccumulatorBrush = null;
                    break;
                case "DIV":
                    WindowBindings.AluRegister1 = WindowBindings.Accumulator.ToString();
                    WindowBindings.AluRegister1Brush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AluRegister1Brush = null;
                    if (args.Length > 0 && args[0][0] == '#') WindowBindings.AluRegister2 = args[0].Remove(0, 1);
                    else WindowBindings.AluRegister2 = _memoryContainer.GetValue(args[0]).ToString();
                    WindowBindings.AluRegister2Brush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AluRegister2Brush = null;
                    WindowBindings.AluMethod = "/";
                    WindowBindings.AluMethodBrush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AluMethodBrush = null;
                    try {
                        if (args.Length > 0 && args[0][0] == '#' && int.TryParse(args[0].Remove(0, 1), out var result4))
                            WindowBindings.Accumulator /= result4;
                        else WindowBindings.Accumulator /= _memoryContainer.GetValue(args[0]);
                    }
                    catch {
                        MessageBox.Show(
                            $"Failed to execute line {WindowBindings.CurrentLine - 1}:{Environment.NewLine}Attempting to divide a number by 0{Environment.NewLine}\"{line}\"",
                            "Runtime exception", MessageBoxButton.OK, MessageBoxImage.Error
                        );
                    }
                    WindowBindings.AccumulatorBrush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AccumulatorBrush = null;
                    break;
                case "JMP":
                    WindowBindings.AluRegister1 = "";
                    WindowBindings.AluRegister2 = "";
                    WindowBindings.AluRegister1Brush = new SolidColorBrush(BlueColor);
                    WindowBindings.AluRegister2Brush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AluRegister1Brush = null;
                    WindowBindings.AluRegister2Brush = null;
                    WindowBindings.AluMethod = "↷";
                    WindowBindings.AluMethodBrush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AluMethodBrush = null;
                    WindowBindings.CurrentLine = int.Parse(args[0]);
                    break;
                case "JMZ":
                    WindowBindings.AluRegister1 = "";
                    WindowBindings.AluRegister2 = "";
                    WindowBindings.AluRegister1Brush = new SolidColorBrush(BlueColor);
                    WindowBindings.AluRegister2Brush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AluRegister1Brush = null;
                    WindowBindings.AluRegister2Brush = null;
                    WindowBindings.AluMethod = "↷";
                    WindowBindings.AluMethodBrush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AluMethodBrush = null;
                    if (WindowBindings.Accumulator == 0) WindowBindings.CurrentLine = int.Parse(args[0]);
                    break;
                case "HLT":
                    WindowBindings.AluRegister1 = "";
                    WindowBindings.AluRegister2 = "";
                    WindowBindings.AluMethod = "❌";
                    WindowBindings.LinesBrush = new SolidColorBrush(Colors.Red);
                    WindowBindings.AluMethodBrush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AluMethodBrush = null;
                    WindowBindings.IsRunning = false;
                    break;
                case "NOP":
                    WindowBindings.AluRegister1 = "";
                    WindowBindings.AluRegister2 = "";
                    WindowBindings.AluMethod = "";
                    break;
                default:
                    MessageBox.Show(
                        $"Failed to execute line {WindowBindings.CurrentLine - 1}:{Environment.NewLine}This command is not recognized{Environment.NewLine}\"{line}\"",
                        "Runtime exception", MessageBoxButton.OK, MessageBoxImage.Error
                        );
                    WindowBindings.AluMethod = "❌";
                    WindowBindings.LinesBrush = new SolidColorBrush(Colors.Red);
                    WindowBindings.AluMethodBrush = new SolidColorBrush(BlueColor);
                    if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
                    WindowBindings.AluMethodBrush = null;
                    WindowBindings.IsRunning = false;
                    break;
            }
        }

        public async Task Start() {
            if (_codeLines == null) {
                MessageBox.Show("No valid code has been found.", "CPU Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (WindowBindings.CurrentLine >= _codeLines.Length) WindowBindings.CurrentLine = 1;
            WindowBindings.LinesBrush = new SolidColorBrush(Colors.LawnGreen);
            while (WindowBindings.CurrentLine < _codeLines.Length + 1) {
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
                    WindowBindings.IsRunning = false;
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
            if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
            WindowBindings.IncrementLineBrush = null;
            WindowBindings.CurrentLine++;
            WindowBindings.CurrentLineBrush = new SolidColorBrush(BlueColor);
            if((int)DelaySizer.Value > 10) await Task.Delay((int)DelaySizer.Value);
            WindowBindings.CurrentLineBrush = null;
        }

        private void CodeBox_TextChanged(object sender, TextChangedEventArgs e) {
            _codeLines = CodeBox.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }

        private async void NextBtn_Click(object sender, RoutedEventArgs e) {
            NextBtn.IsEnabled = false;
            WindowBindings.IsRunning = true;
            WindowBindings.LinesBrush = new SolidColorBrush(Colors.DodgerBlue);
            await NextStep();
            WindowBindings.LinesBrush = new SolidColorBrush(Colors.Gray);
            WindowBindings.IsRunning = false;
            NextBtn.IsEnabled = true;
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
            var openFileDialog = new System.Windows.Forms.OpenFileDialog { Filter = @"VNS Project|*.vns" };
            var result = openFileDialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK) return;
            try {
                using (var fileStream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read,
                    FileShare.Read)) {
                    using (var streamReader = new StreamReader(fileStream)) {
                        var data = streamReader.ReadToEnd();
                        var objectData = JsonConvert.DeserializeObject<ProjectFile>(data);
                        CodeBox.Text = objectData.projectCode;
                        foreach (var i in objectData.Memory) {
                            _memoryContainer.SetValue(i.Key, i.Value);
                        }
                        //_memoryContainer = objectData.Memory;
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Failed to save project!{Environment.NewLine}{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveProject_Click(object sender, RoutedEventArgs e) {
            var saveFileDialog = new System.Windows.Forms.SaveFileDialog {Filter = @"VNS Project|*.vns"};
            var result = saveFileDialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK) return;
            try {
                var objectData = new ProjectFile() {
                    projectCode = CodeBox.Text,
                    Memory = new Dictionary<string, int>()
                };
                foreach (var keyValue in _memoryContainer.MemoryData) {
                    objectData.Memory.Add(keyValue.MemoryName, keyValue.MemoryValue);
                }
                var data = JsonConvert.SerializeObject(objectData, Formatting.Indented);
                using (var fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.ReadWrite,
                    FileShare.Read)) {
                    using (var streamWriter = new StreamWriter(fileStream)) {
                        streamWriter.Write(data);
                    }
                }
                MessageBox.Show("Project saved successfully.", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) {
                MessageBox.Show($"Failed to save project!{Environment.NewLine}{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e) {
            var saveFileDialog = new System.Windows.Forms.SaveFileDialog() { Filter = @"VNS Project|*.vns|Json File|*.json|All File|*.*" };
            var result = saveFileDialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK) return;
            try {
                var objectData = new ProjectFile() {
                    projectCode = CodeBox.Text,
                    Memory = new Dictionary<string, int>()
                };
                foreach (var keyValue in _memoryContainer.MemoryData) {
                    objectData.Memory.Add(keyValue.MemoryName, keyValue.MemoryValue);
                }
                var data = JsonConvert.SerializeObject(objectData, Formatting.Indented);
                using (var fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.ReadWrite,
                    FileShare.Read)) {
                    using (var streamWriter = new StreamWriter(fileStream)) {
                        streamWriter.Write(data);
                    }
                }
                MessageBox.Show("Project saved successfully.", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) {
                MessageBox.Show($"Failed to save project!{Environment.NewLine}{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewHelp_Click(object sender, RoutedEventArgs e) {
            Process.Start("https://github.com/jpdante/VonNeumannSimulator/wiki");
        }

        private void AboutVNS_Click(object sender, RoutedEventArgs e) {
            var about = "This project was developed by:" + Environment.NewLine +
                        "João Pedro Dante" + Environment.NewLine +
                        Environment.NewLine +
                        "Github:" + Environment.NewLine +
                        "https://github.com/jpdante/VonNeumannSimulator" + Environment.NewLine +
                        Environment.NewLine +
                        "Version: 1.1";
                        MessageBox.Show(about, "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
