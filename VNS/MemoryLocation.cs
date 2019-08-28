using System.ComponentModel;

namespace VNS {
    public class MemoryLocation : INotifyPropertyChanged {
        private string _memoryName;
        private int _memoryValue;

        public string MemoryName {
            get => _memoryName;
            set {
                _memoryName = value;
                RaisePropertyChanged("MemoryName");
            }
        }

        public int MemoryValue {
            get => _memoryValue;
            set {
                _memoryValue = value;
                RaisePropertyChanged("MemoryValue");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MemoryLocation(string memoryName, int memoryValue = 0) {
            MemoryName = memoryName;
            MemoryValue = 0;
        }

        public void RaisePropertyChanged(string propertyName) {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
