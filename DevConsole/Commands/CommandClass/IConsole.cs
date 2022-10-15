
using System;

namespace V7G.Console
{
    public interface IConsole
    {
        public string CommandID { get; }
        public string CommandDescription { get; }
        public string CommandFormat { get; }
    }
}