
using System.Collections.Generic;

namespace V7G.Console
{
    public interface IConsoleCommand
    {
        public List<CommandBase> Commands { get; }
    }
}