using NStandard;
using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace Gexf
{
    public class Gexf : Gexf<EmptyClass> { }

    public class Gexf<TNodeAttrModel> where TNodeAttrModel : class, new()
    {
        public string Creator { get; set; } = ".net Gexf";
        public string Description { get; set; }
        public GraphMode Mode { get; set; } = GraphMode.Static;
        public GraphEdgeType EdgeType { get; set; } = GraphEdgeType.Undirected;
        public GraphNode<TNodeAttrModel>[] Nodes { get; set; } = Array.Empty<GraphNode<TNodeAttrModel>>();
        public GraphAttr[] Attributes { get; private set; }

        public Gexf()
        {
            var props = typeof(TNodeAttrModel).GetProperties().Where(x => x.CanWrite && x.CanRead).ToArray();

            Attributes = props.Select(x => new GraphAttr(x.PropertyType)
            {
                Id = StringEx.KebabCase(x.Name),
                Title = x.Name,
            }).ToArray();
        }

        public XmlDocument Build()
        {
            var xml = new XmlDocument();
            var xmlDeclaration = xml.CreateXmlDeclaration("1.0", "utf-8", null);

            Func<string, XmlElement> CreateElement = xml.CreateElement;

            xml.InsertBefore(xmlDeclaration, xml.DocumentElement);
            xml.AppendChild(xml.CreateElement("gexf").With(_gexf =>
            {
                //_gexf.SetAttribute("xmlns", "http://www.gexf.net/1.2draft");
                _gexf.SetAttribute("version", "1.2");
                _gexf.SetAttribute("xmlns:viz", "http://www.w3.org/2001/XMLSchema-instance");
                _gexf.SetAttribute("xsi:schemaLocation", "http://www.gexf.net/1.2draft http://www.gexf.net/1.2draft/gexf.xsd");
                _gexf.AppendChild(CreateElement("meta").With(_meta =>
                {
                    _meta.SetAttribute("lastmodifieddate", DateTime.Now.Date.ToString("yyyy-MM-dd"));
                    _meta.AppendChild(CreateElement("creator").With(c => c.InnerText = ".net Gexf"));
                    _meta.AppendChild(CreateElement("description"));
                }));
                _gexf.AppendChild(CreateElement("graph").With(_graph =>
                {
                    _graph.SetAttribute("defaultedgetype", EdgeType.Value);
                    _graph.SetAttribute("mode", Mode.Value);
                    _graph.AppendChild(CreateElement("attributes").With(_attrs =>
                    {
                        _attrs.SetAttribute("class", "node");
                        _attrs.SetAttribute("class", "static");
                        foreach (var attribute in Attributes)
                        {
                            _attrs.AppendChild(CreateElement("attribute").With(_attr =>
                            {
                                _attr.SetAttribute("id", attribute.Id);
                                _attr.SetAttribute("title", attribute.Title);
                                _attr.SetAttribute("type", attribute.Type.Value);
                            }));
                        }
                    }));
                    _graph.AppendChild(CreateElement("nodes").With(_nodes =>
                    {
                        foreach (var node in Nodes)
                        {
                            _nodes.AppendChild(CreateElement("node").With(_node =>
                            {
                                _node.SetAttribute("id", node.Id);
                                _node.SetAttribute("label", node.Label);
                                _node.AppendChild(CreateElement("attvalues").With(_attvalues =>
                                {
                                    var props = node.Attvalues.GetType().GetProperties().Where(x => x.CanWrite && x.CanRead).ToArray();
                                    foreach (var prop in props)
                                    {
                                        _attvalues.AppendChild(CreateElement("attvalue").With(_attvalue =>
                                        {
                                            _attvalue.SetAttribute("for", node.Id);
                                            _attvalue.SetAttribute("value", prop.GetValue(node.Attvalues)?.ToString());
                                        }));
                                    }
                                }));
                                _node.AppendChild(CreateElement("viz:size").With(_size =>
                                {
                                    _size.SetAttribute("value", node.Size.ToString());
                                }));
                                _node.AppendChild(CreateElement("viz:position").With(_position =>
                                {
                                    _position.SetAttribute("x", node.Position.X.ToString());
                                    _position.SetAttribute("y", node.Position.Y.ToString());
                                    _position.SetAttribute("z", node.Position.Z.ToString());
                                }));
                                _node.AppendChild(CreateElement("viz:color").With(_color =>
                                {
                                    _color.SetAttribute("r", node.Color.R.ToString());
                                    _color.SetAttribute("g", node.Color.G.ToString());
                                    _color.SetAttribute("b", node.Color.B.ToString());
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
            if (settings is null) settings = new XmlWriterSettings { Indent = true, NewLineOnAttributes = true };

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
