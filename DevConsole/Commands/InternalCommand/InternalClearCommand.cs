using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace V7G.Console
{
    public class InternalClearCommand : ConsoleCommandBase
    {
        protected void InternalInit(string id, string description, string format, Action command)
        {
            hideFlags = HideFlags.HideInInspector;

            InitCommand(id,description,format, command);
        }        
    }
}