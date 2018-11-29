using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    class Prediction
    {
        public List<List<double>> classes { get; set; }
        public List<List<double>> scores { get; set; }
    }
}
