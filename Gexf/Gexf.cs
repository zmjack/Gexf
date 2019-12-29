using NStandard;
using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace Gexf
{
    public class Gexf : Gexf<EmptyAttrModel, EmptyAttrModel> { }

    public class Gexf<TNodeAttrModel, TEdgeAttrModel>
        where TNodeAttrModel : class, new()
        where TEdgeAttrModel : class, new()
    {
        public string Creator { get; set; } = ".net Gexf";
        public string Description { get; set; }
        public GraphMode Mode { get; set; } = GraphMode.Static;
        public GraphEdgeType EdgeType { get; set; } = GraphEdgeType.Undirected;
        public GraphAttr[] NodeAttributes { get; private set; }
        public GraphAttr[] EdgeAttributes { get; private set; }
        public GraphNode<TNodeAttrModel>[] Nodes { get; set; } = Array.Empty<GraphNode<TNodeAttrModel>>();
        public GraphEdge<TEdgeAttrModel>[] Edges { get; set; } = Array.Empty<GraphEdge<TEdgeAttrModel>>();

        public Gexf()
        {
            var nodeProps = typeof(TNodeAttrModel).GetProperties().Where(x => x.CanWrite && x.CanRead).ToArray();
            NodeAttributes = nodeProps.Select(x => new GraphAttr(x.PropertyType)
            {
                Id = StringEx.KebabCase(x.Name),
                Title = x.Name,
            }).ToArray();

            var edgeProps = typeof(TEdgeAttrModel).GetProperties().Where(x => x.CanWrite && x.CanRead).ToArray();
            EdgeAttributes = edgeProps.Select(x => new GraphAttr(x.PropertyType)
            {
                Id = StringEx.KebabCase(x.Name),
                Title = x.Name,
            }).ToArray();
        }

        public XmlDocument Build()
        {
            var xml = new XmlDocument();
            var ns = "http://www.gexf.net/1.2draft";
            var ns_viz = "http://www.gexf.net/1.2draft/viz";

            xml.InsertBefore(xml.CreateXmlDeclaration("1.0", "utf-8", null), xml.DocumentElement);
            xml.AppendChild(xml.CreateElement("gexf", ns).With(_gexf =>
            {
                _gexf.SetAttribute("version", "1.2");
                _gexf.SetAttribute("xmlns:viz", ns_viz);
                //_gexf.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                //_gexf.SetAttribute("xsi:schemaLocation", "http://www.gexf.net/1.2draft http://www.gexf.net/1.2draft/gexf.xsd");
                _gexf.AppendChild(xml.CreateElement("meta", ns).With(_meta =>
                {
                    _meta.SetAttribute("lastmodifieddate", DateTime.Now.Date.ToString("yyyy-MM-dd"));
                    _meta.AppendChild(xml.CreateElement("creator", ns).With(c => c.InnerText = ".net Gexf"));
                    _meta.AppendChild(xml.CreateElement("description", ns));
                }));
                _gexf.AppendChild(xml.CreateElement("graph", ns).With(_graph =>
                {
                    _graph.SetAttribute("defaultedgetype", EdgeType.Value);
                    _graph.SetAttribute("mode", Mode.Value);
                    _graph.AppendChild(xml.CreateElement("attributes", ns).With(_attrs =>
                    {
                        _attrs.SetAttribute("class", "node");
                        _attrs.SetAttribute("mode", "static");
                        foreach (var attribute in NodeAttributes)
                        {
                            _attrs.AppendChild(xml.CreateElement("attribute", ns).With(_attr =>
                            {
                                _attr.SetAttribute("id", attribute.Id);
                                _attr.SetAttribute("title", attribute.Title);
                                _attr.SetAttribute("type", attribute.Type.Value);
                            }));
                        }
                    }));
                    _graph.AppendChild(xml.CreateElement("attributes", ns).With(_attrs =>
                    {
                        _attrs.SetAttribute("class", "edge");
                        _attrs.SetAttribute("mode", "static");
                        foreach (var attribute in EdgeAttributes)
                        {
                            _attrs.AppendChild(xml.CreateElement("attribute", ns).With(_attr =>
                            {
                                _attr.SetAttribute("id", attribute.Id);
                                _attr.SetAttribute("title", attribute.Title);
                                _attr.SetAttribute("type", attribute.Type.Value);
                            }));
                        }
                    }));
                    _graph.AppendChild(xml.CreateElement("nodes", ns).With(_nodes =>
                    {
                        foreach (var node in Nodes)
                        {
                            _nodes.AppendChild(xml.CreateElement("node", ns).With(_node =>
                            {
                                _node.SetAttribute("id", node.Id);
                                _node.SetAttribute("label", node.Label);
                                _node.AppendChild(xml.CreateElement("attvalues", ns).With(_attvalues =>
                                {
                                    var props = node.Attvalues.GetType().GetProperties().Where(x => x.CanWrite && x.CanRead).ToArray();
                                    foreach (var prop in props)
                                    {
                                        _attvalues.AppendChild(xml.CreateElement("attvalue", ns).With(_attvalue =>
                                        {
                                            _attvalue.SetAttribute("for", StringEx.KebabCase(prop.Name));
                                            _attvalue.SetAttribute("value", prop.GetValue(node.Attvalues)?.ToString());
                                        }));
                                    }
                                }));
                                _node.AppendChild(xml.CreateElement("viz:size", ns_viz).With(_size =>
                                {
                                    _size.SetAttribute("value", node.Size.ToString());
                                }));
                                _node.AppendChild(xml.CreateElement("viz:position", ns_viz).With(_position =>
                                {
                                    _position.SetAttribute("x", node.Position.X.ToString());
                                    _position.SetAttribute("y", node.Position.Y.ToString());
                                    _position.SetAttribute("z", node.Position.Z.ToString());
                                }));
                                _node.AppendChild(xml.CreateElement("viz:color", ns_viz).With(_color =>
                                {
                                    _color.SetAttribute("r", node.Color.R.ToString());
                                    _color.SetAttribute("g", node.Color.G.ToString());
                                    _color.SetAttribute("b", node.Color.B.ToString());
                                }));
                            }));
                        }
                    }));
                    _graph.AppendChild(xml.CreateElement("edges", ns).With(_nodes =>
                    {
                        foreach (var edge in Edges)
                        {
                            _nodes.AppendChild(xml.CreateElement("edge", ns).With(_node =>
                            {
                                _node.SetAttribute("id", edge.Id);
                                _node.SetAttribute("source", edge.Source);
                                _node.SetAttribute("target", edge.Target);
                                _node.SetAttribute("width", edge.Width.ToString());
                                _node.AppendChild(xml.CreateElement("attvalues", ns).With(_attvalues =>
                                {
                                    var props = edge.Attvalues.GetType().GetProperties().Where(x => x.CanWrite && x.CanRead).ToArray();
                                    foreach (var prop in props)
                                    {
                                        _attvalues.AppendChild(xml.CreateElement("attvalue", ns).With(_attvalue =>
                                        {
                                            _attvalue.SetAttribute("for", StringEx.KebabCase(prop.Name));
                                            _attvalue.SetAttribute("value", prop.GetValue(edge.Attvalues)?.ToString());
                                        }));
                                    }
                                }));
                            }));
                        }
                    }));
                }));
            }));

            return xml;
        }

        public string GetContent(XmlWriterSettings settings = null)
        {
            if (settings is null) settings = new XmlWriterSettings { Indent = true };

            var xml = Build();
            using (var memory = new MemoryStream())
            using (var swriter = new StreamWriter(memory))
            using (var writer = XmlWriter.Create(swriter, settings))
            {
                xml.WriteContentTo(writer);
                writer.Flush();
                swriter.Flush();

                memory.Seek(0, SeekOrigin.Begin);
                return memory.ToArray().String();
            }
        }

    }
}
