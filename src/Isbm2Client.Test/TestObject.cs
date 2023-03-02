using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isbm2Client.Test
{
    public class TestObject
    {
        // TODO: Needs to be set back to int[] issue for server has been resolved
        public double[] Numbers { get; set; } = null!;
        public string Text { get; set; } = "";
        public Dictionary<string, double> Weather { get; set; } = null!;
    }
}
