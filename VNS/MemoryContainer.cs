using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace VNS {
    [Serializable]
    public class MemoryContainer : INotifyPropertyChanged {
        public ObservableCollection<MemoryLocation> MemoryData { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MemoryContainer() {
            MemoryData = new ObservableCollection<MemoryLocation> {
                new MemoryLocation("A"),
                new MemoryLocation("B"),
                new MemoryLocation("C"),
                new MemoryLocation("D"),
                new MemoryLocation("E"),
                new MemoryLocation("F"),
                new MemoryLocation("G"),
                new MemoryLocation("H"),
                new MemoryLocation("I"),
                new MemoryLocation("J"),
                new MemoryLocation("K"),
                new MemoryLocation("L"),
                new MemoryLocation("M"),
                new MemoryLocation("N"),
                new MemoryLocation("O"),
                new MemoryLocation("P"),
                new MemoryLocation("Q"),
                new MemoryLocation("R"),
                new MemoryLocation("S"),
                new MemoryLocation("T"),
                new MemoryLocation("U"),
                new MemoryLocation("V"),
                new MemoryLocation("W"),
                new MemoryLocation("X"),
                new MemoryLocation("Y"),
                new MemoryLocation("Z")
            };
            for (var i = 0; i < 50; i++) {
                MemoryData.Add(new MemoryLocation("0x" + i.ToString("X")));
            }
        }


        public int GetValue(string id) { return (from memory in MemoryData where memory.MemoryName.Equals(id, StringComparison.CurrentCultureIgnoreCase) select memory.MemoryValue).FirstOrDefault(); }

        public void SetValue(string id, int value) {
            foreach (var memory in MemoryData) {
                if (!memory.MemoryName.Equals(id, StringComparison.CurrentCultureIgnoreCase)) continue;
                memory.MemoryValue = value;
                return;
            }
            MemoryData.Add(new MemoryLocation(id, value));
        }

        private void OnMemoryAppenderPropertyChanged(object sender, PropertyChangedEventArgs e) {
            this.RaisePropertyChanged(e.PropertyName);
        }

        public void RaisePropertyChanged(string propertyName) {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
