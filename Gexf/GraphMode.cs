using System;
using System.Collections.Generic;
using System.Text;

namespace Gexf
{
    public class GraphMode
    {
        public string Value { get; private set; }

        public GraphMode(string value)
        {
            Value = value;
        }

        public static GraphMode Dynamic => new GraphMode("dynamic");
        public static GraphMode Static => new GraphMode("static");
    }
}
