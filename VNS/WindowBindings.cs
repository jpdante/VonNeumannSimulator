using System.ComponentModel;
using System.Windows.Media;

namespace VNS {
    public class WindowBindings : INotifyPropertyChanged {
        private int _accumulator;
        private int _currentLine = 1;
        private string _aluRegister1;
        private string _aluRegister2;
        private string _aluMethod;
        private string _currentInstruction;
        private string _currentInstructionData;
        private Brush _linesBrush;
        private Brush _currentLineBrush;
        private Brush _accumulatorBrush;
        private Brush _aluRegister1Brush;
        private Brush _aluRegister2Brush;
        private Brush _aluMethodBrush;
        private Brush _currentInstructionBrush;
        private Brush _decoderBrush;
        private Brush _incrementLineBrush;
        private bool _isRunning = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public WindowBindings() {
            LinesBrush = new SolidColorBrush(Colors.Gray);
        }

        public int Accumulator {
            get => _accumulator;
            set {
                _accumulator = value;
                RaisePropertyChanged("Accumulator");
            }
        }
        public int CurrentLine {
            get => _currentLine;
            set {
                _currentLine = value;
                RaisePropertyChanged("CurrentLine");
            }
        }
        public string AluRegister1 {
            get => _aluRegister1;
            set {
                _aluRegister1 = value;
                RaisePropertyChanged("AluRegister1");
            }
        }
        public string AluRegister2 {
            get => _aluRegister2;
            set {
                _aluRegister2 = value;
                RaisePropertyChanged("AluRegister2");
            }
        }
        public string AluMethod {
            get => _aluMethod;
            set {
                _aluMethod = value;
                RaisePropertyChanged("AluMethod");
            }
        }
        public string CurrentInstruction {
            get => _currentInstruction;
            set {
                _currentInstruction = value;
                RaisePropertyChanged("CurrentInstruction");
            }
        }
        public string CurrentInstructionData {
            get => _currentInstructionData;
            set {
                _currentInstructionData = value;
                RaisePropertyChanged("CurrentInstructionData");
            }
        }
        public Brush LinesBrush {
            get => _linesBrush;
            set {
                _linesBrush = value;
                RaisePropertyChanged("LinesBrush");
            }
        }
        public Brush CurrentLineBrush {
            get => _currentLineBrush;
            set {
                _currentLineBrush = value;
                RaisePropertyChanged("CurrentLineBrush");
            }
        }
        public Brush AccumulatorBrush {
            get => _accumulatorBrush;
            set {
                _accumulatorBrush = value;
                RaisePropertyChanged("AccumulatorBrush");
            }
        }
        public Brush AluRegister1Brush {
            get => _aluRegister1Brush;
            set {
                _aluRegister1Brush = value;
                RaisePropertyChanged("AluRegister1Brush");
            }
        }
        public Brush AluRegister2Brush {
            get => _aluRegister2Brush;
            set {
                _aluRegister2Brush = value;
                RaisePropertyChanged("AluRegister2Brush");
            }
        }
        public Brush AluMethodBrush {
            get => _aluMethodBrush;
            set {
                _aluMethodBrush = value;
                RaisePropertyChanged("AluMethodBrush");
            }
        }
        public Brush CurrentInstructionBrush {
            get => _currentInstructionBrush;
            set {
                _currentInstructionBrush = value;
                RaisePropertyChanged("CurrentInstructionBrush");
            }
        }
        public Brush DecoderBrush {
            get => _decoderBrush;
            set {
                _decoderBrush = value;
                RaisePropertyChanged("DecoderBrush");
            }
        }
        public Brush IncrementLineBrush {
            get => _incrementLineBrush;
            set {
                _incrementLineBrush = value;
                RaisePropertyChanged("IncrementLineBrush");
            }
        }
        public bool IsRunning {
            get => _isRunning;
            set {
                _isRunning = value;
                RaisePropertyChanged("IsRunning");
            }
        }

        public void RaisePropertyChanged(string propertyName) {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
