using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace V7G.Console
{
    public static class AttributeHelper 
    {
        public static void GetCommandsByTarget(ref List<ConsoleAttribute> commandAttributes, ConsoleAttributeTarget target)
        {
            Type[] ActiveBehaviours = GetAllSubTypes(typeof(MonoBehaviour));

            foreach (Type Type in ActiveBehaviours)
            {
                object[] Commands = Type.GetCustomAttributes(typeof(ConsoleAttribute), false);

                if (Commands != null)
                {
                    for (int i = 0; i < Commands.Length; i++)
                    {
                        if ((ConsoleAttribute)Commands[i] != null)
                        {
                            if (((ConsoleAttribute)Commands[i]).Target == target)
                            {
                                ((ConsoleAttribute)Commands[i]).Behaviour = Type;

                                commandAttributes.Add((ConsoleAttribute)Commands[i]);
                            }
                        }
                    }
                }
            }
        }
        public static Type[] GetAllSubTypes(Type aBaseClass)
        {
            List<Type> Result = new List<Type>();

            Assembly[] Assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly Assembly in Assemblies)
            {
                Type[] Types = Assembly.GetTypes();

                foreach (Type T in Types)
                {
                    if (T.IsSubclassOf(aBaseClass))
                    {
                        Result.Add(T);
                    }
                }
            }
            return Result.ToArray();
        }
    }
}