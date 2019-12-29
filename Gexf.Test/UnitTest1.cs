using NStandard;
using System;
using System.Diagnostics;
using System.IO;
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
            var gexf = new Gexf<GexfNodeAttrModel, EmptyAttrModel>();
            gexf.Nodes = new[]
            {
                new GraphNode<GexfNodeAttrModel>("0", "Myriel")
                {
                    Size = 28.685715,
                    Position = new VizPosition { X = -266.82776, Y = 299.6904, Z = 0.0 },
                    Color = new VizColor { R = 235, G = 81, B = 72 },
                    Attvalues = new GexfNodeAttrModel { ModularityClass = 0 },
                },
                new GraphNode<GexfNodeAttrModel>("1", "Napoleon")
                {
                    Size = 14.0,
                    Position = new VizPosition { X = -418.08344, Y = 446.8853, Z = 0.0 },
                    Color = new VizColor { R = 236, G = 81, B = 72 },
                    Attvalues = new GexfNodeAttrModel { ModularityClass = 0 },
                },
            };
            gexf.Edges = new[]
            {
                new GraphEdge<EmptyAttrModel>("0") { Source = "0", Target = "1" },
            };

            Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<gexf version=""1.2"" xmlns:viz=""http://www.gexf.net/1.2draft/viz"" xmlns=""http://www.gexf.net/1.2draft"">
  <meta lastmodifieddate=""2019-12-29"">
    <creator>.net Gexf</creator>
    <description />
  </meta>
  <graph defaultedgetype=""undirected"" mode=""static"">
    <attributes class=""node"" mode=""static"">
      <attribute id=""modularity-class"" title=""ModularityClass"" type=""integer"" />
    </attributes>
    <attributes class=""edge"" mode=""static"" />
    <nodes>
      <node id=""0"" label=""Myriel"">
        <attvalues>
          <attvalue for=""modularity-class"" value=""0"" />
        </attvalues>
        <viz:size value=""28.685715"" />
        <viz:position x=""-266.82776"" y=""299.6904"" z=""0"" />
        <viz:color r=""235"" g=""81"" b=""72"" />
      </node>
      <node id=""1"" label=""Napoleon"">
        <attvalues>
          <attvalue for=""modularity-class"" value=""0"" />
        </attvalues>
        <viz:size value=""14"" />
        <viz:position x=""-418.08344"" y=""446.8853"" z=""0"" />
        <viz:color r=""236"" g=""81"" b=""72"" />
      </node>
    </nodes>
    <edges>
      <edge id=""0"" source=""0"" target=""1"" width=""0"">
        <attvalues />
      </edge>
    </edges>
  </graph>
</gexf>", gexf.GetContent());
        }
    }
}
