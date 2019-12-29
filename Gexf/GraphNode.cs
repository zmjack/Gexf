using System;
using System.Collections.Generic;
using System.Text;

namespace Gexf
{
    public class GraphNode<TNodeAttrModel> where TNodeAttrModel : class, new()
    {
        public TNodeAttrModel Attvalues = new TNodeAttrModel();

        public GraphNode(string id, string label = null)
        {
            Id = id;
            Label = label;
        }

        public string Id { get; set; }
        public string Label { get; set; }
        public string Interval { get; set; }

        public double Size { get; set; }
        public VizPosition Position { get; set; }
        public VizColor Color { get; set; }
    }
}
