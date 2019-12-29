using System;
using System.Collections.Generic;
using System.Text;

namespace Gexf
{
    public class GraphEdge<TNodeAttrModel> where TNodeAttrModel : class, new()
    {
        public readonly TNodeAttrModel Attvalues = new TNodeAttrModel();

        public GraphEdge(string id) => Id = id;

        public string Id { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public double Width { get; set; }
    }
}
