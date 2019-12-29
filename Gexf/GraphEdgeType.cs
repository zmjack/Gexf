using System;
using System.Collections.Generic;
using System.Text;

namespace Gexf
{
    public class GraphEdgeType
    {
        public string Value { get; private set; }

        public GraphEdgeType(string value)
        {
            Value = value;
        }

        public static GraphEdgeType Undirected => new GraphEdgeType("undirected");
        public static GraphEdgeType Directed => new GraphEdgeType("directed");
    }
}
