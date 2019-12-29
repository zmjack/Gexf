using NStandard;
using System;
using System.Diagnostics;
using Xunit;

namespace Gexf.Test
{
    public class UnitTest1
    {
        public class GexfNodeAttrModel
        {
            public int ModularityClass { get; set; }
        }

        [Fact]
        public void Test1()
        {
            var gexf = new Gexf<GexfNodeAttrModel>();
            gexf.Nodes = new[]
            {
                new GraphNode<GexfNodeAttrModel>("0", "Myriel")
                {
                    Size = 28.685715,
                    Position = new VizPosition { X = -266.82776, Y = 299.6904, Z = 0.0 },
                    Color = new VizColor { R = 235, G = 81, B = 72 },
                }.With(x => x.Attvalues.ModularityClass = 0),
            };

            Debug.WriteLine(gexf.GetContent());
        }
    }
}
