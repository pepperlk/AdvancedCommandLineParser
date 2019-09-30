using System;

namespace AdvancedCommandLineParser
{
    [AttributeUsage(AttributeTargets.Class)]
    public class VerbGroupAttribute : Attribute
    {
        public VerbGroupAttribute(string group)
        {
            this.Group = group;
        }

        public string Group { get; }
    }
}