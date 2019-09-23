using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNS {
    [Serializable]
    public class ProjectFile {
        public string projectCode;
        public Dictionary<string, int> Memory;
    }
}
