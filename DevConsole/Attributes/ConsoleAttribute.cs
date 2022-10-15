using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace V7G.Console
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConsoleAttribute : Attribute
    {
        #region Fields

        public readonly string Name;
        public readonly string Description;

        public ConsoleAttributeTarget Target;
        public Type Behaviour;

        #endregion Fields

        #region Methods

        public ConsoleAttribute(string name, string description, ConsoleAttributeTarget target)
        {
            Name = name;
            Description = description;
            Target = target;
        }

        #endregion Methods
    }
}