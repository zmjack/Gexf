using System;
using System.Collections.Generic;
using System.Text;

namespace Gexf
{
    public class GraphAttributeType
    {
        public string Value { get; private set; }

        public GraphAttributeType(string value)
        {
            Value = value;
        }

        public static GraphAttributeType String => new GraphAttributeType("string");
        public static GraphAttributeType Integer => new GraphAttributeType("integer");
        public static GraphAttributeType Long => new GraphAttributeType("long");
        public static GraphAttributeType Float => new GraphAttributeType("float");
        public static GraphAttributeType Double => new GraphAttributeType("double");
        public static GraphAttributeType Boolean => new GraphAttributeType("boolean");
        public static GraphAttributeType Byte => new GraphAttributeType("byte");
        public static GraphAttributeType Short => new GraphAttributeType("short");
        public static GraphAttributeType Character => new GraphAttributeType("character");
    }
}
