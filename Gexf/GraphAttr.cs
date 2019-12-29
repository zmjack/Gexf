using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gexf
{
    public class GraphAttr
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public GraphAttributeType Type { get; private set; }
        public object Default { get; set; }

        public GraphAttr(Type ValueType)
        {
            switch (ValueType)
            {
                default:
                case Type type when type == typeof(VString): Type = GraphAttributeType.String; break;
                case Type type when new[] { typeof(int), typeof(uint) }.Contains(type): Type = GraphAttributeType.Integer; break;
                case Type type when new[] { typeof(long), typeof(ulong) }.Contains(type): Type = GraphAttributeType.Long; break;
                case Type type when type == typeof(float): Type = GraphAttributeType.Float; break;
                case Type type when type == typeof(double): Type = GraphAttributeType.Double; break;
                case Type type when type == typeof(bool): Type = GraphAttributeType.Boolean; break;
                case Type type when new[] { typeof(byte), typeof(sbyte) }.Contains(type): Type = GraphAttributeType.Byte; break;
                case Type type when new[] { typeof(short), typeof(ushort) }.Contains(type): Type = GraphAttributeType.Short; break;
                case Type type when type == typeof(char): Type = GraphAttributeType.Character; break;
            }
        }
    }

}
