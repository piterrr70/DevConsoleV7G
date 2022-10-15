
using System;

namespace V7G.Console
{
    [System.Serializable]
    public class CommandBase : IConsole
    {
        public CommandBase(string commandID, string commandDescription, string commandFormat)
        {
            CommandID = commandID;
            CommandDescription = commandDescription;
            CommandFormat = commandFormat;
        }

        public string CommandID { get; internal set; }
        public string CommandDescription { get; internal set; }
        public string CommandFormat { get; internal set; }        
    }
}