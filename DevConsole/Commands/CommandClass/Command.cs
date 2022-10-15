using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace V7G.Console
{
    [System.Serializable]
    public class Command : CommandBase
    {
        public Command(string commandID, string commandDescription, string commandFormat, Action commandAction) : base(commandID, commandDescription, commandFormat)
        {
            this.CommandAction = commandAction;
        }

        public Action CommandAction { get; internal set; }
    }

    [System.Serializable]
    public class Command<T1> : CommandBase
    {
        public Command(string commandID, string commandDescription, string commandFormat, Action<T1> commandAction) : base(commandID, commandDescription, commandFormat)
        {
            this.CommandAction = commandAction;
        }

        public Action<T1> CommandAction { get; internal set; }
    }

    [System.Serializable]
    public class Command<T1,T2> : CommandBase
    {
        public Command(string commandID, string commandDescription, string commandFormat, Action<T1,T2> commandAction) : base(commandID, commandDescription, commandFormat)
        {
            this.CommandAction = commandAction;
        }

        public Action<T1,T2> CommandAction { get; internal set; }
    }

    [System.Serializable]
    public class Command<T1, T2, T3> : CommandBase
    {
        public Command(string commandID, string commandDescription, string commandFormat, Action<T1, T2, T3> commandAction) : base(commandID, commandDescription, commandFormat)
        {
            this.CommandAction = commandAction;
        }

        public Action<T1, T2, T3> CommandAction { get; internal set; }
    }
}