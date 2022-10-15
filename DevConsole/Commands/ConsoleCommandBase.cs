using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace V7G.Console
{
    /// <summary>
    /// Base class for Console Commands
    /// </summary>
    public abstract class ConsoleCommandBase : MonoBehaviour, IConsoleCommand
    {
        private List<CommandBase> _commands = new();
        public List<CommandBase> Commands => _commands;

        /// <summary>
        /// Find and perform command.
        /// </summary>
        /// <param name="consoleInput">Console Input</param>
        /// <returns></returns>
        public Result<bool> TryPerformCommand(string consoleInput, ref Result<bool> result) 
        {
            string[] splitedInput = consoleInput.Split(' ');
            switch (splitedInput.Count())
            {
                case 1:
                    GetCommand(splitedInput[0], ref result);
                    
                    break;
                case 2:
                    GetCommand(splitedInput[0], splitedInput[1],ref result);
                    
                break;
                case 3:
                    GetCommand(splitedInput[0], splitedInput[1], splitedInput[2], ref result);

                    break;
                case 4:
                    GetCommand(splitedInput[0], splitedInput[1], splitedInput[2], splitedInput[3], ref result);

                    break;
            }
            return result;
        }

        public void GetCommand(string commandID, ref Result<bool> result)
        {
            foreach (var command in _commands)
            {
                if (command.CommandID.Equals(commandID))
                {
                    (command as Command).CommandAction.Invoke();
                    result = new(true);
                }
            }
        }
        public void GetCommand(string commandID, string value_1, ref Result<bool> result)
        {
            foreach (var (command, args) in from command in _commands
                                            where commandID.Equals(command.CommandID) && command.GetType().GetGenericArguments().Count() == 1
                                            let args = command.GetType().GetGenericArguments()
                                            select (command, args))
            {
                if (args[0].Equals(value_1.GetTypeOfThisValue()))
                {
                    if (args[0].Equals(typeof(Vector3)))
                    {
                        
                        (command as Command<Vector3>).CommandAction(value_1.StringToVector3(','));
                        result = new(true);
                        break;
                    }
                    else  if (args[0].Equals(typeof(int)))
                    {
                        (command as Command<int>).CommandAction(int.Parse(value_1));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(float)))
                    {
                        (command as Command<float>).CommandAction(float.Parse(value_1));
                        result = new(true);
                        break;
                    }
                    else // we assume this is string
                    {
                        (command as Command<string>).CommandAction(value_1);
                        result = new(true);
                        break;
                    }
                }
                else
                {
                    result = new(false, new($"Incorect command format. Command format: {command.CommandFormat}"));
                    continue;
                }
            }
        }
        
        public void GetCommand(string commandID, string value_1, string value_2, ref Result<bool> result)
        {
            foreach (var (command, args) in from command in _commands
                                            where commandID.Equals(command.CommandID) && command.GetType().GetGenericArguments().Count() == 2
                                            let args = command.GetType().GetGenericArguments()
                                            select (command, args))
            {
                if (args[0].Equals(value_1.GetTypeOfThisValue()) && args[1].Equals(value_2.GetTypeOfThisValue()))
                {
                    //Vector3
                    if (args[0].Equals(typeof(Vector3)) && args[1].Equals(typeof(Vector3)))
                    {
                        (command as Command<Vector3, Vector3>).CommandAction(value_1.StringToVector3(','), value_2.StringToVector3(','));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(Vector3)) && args[1].Equals(typeof(int)))
                    {
                        (command as Command<Vector3, int>).CommandAction(value_1.StringToVector3(','), int.Parse(value_2));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(Vector3)) && args[1].Equals(typeof(float)))
                    {
                        (command as Command<Vector3, float>).CommandAction(value_1.StringToVector3(','), float.Parse(value_2));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(Vector3)) && args[1].Equals(typeof(string)))
                    {
                        (command as Command<Vector3, string>).CommandAction(value_1.StringToVector3(','), value_2);
                        result = new(true);
                        break;
                    }//INT
                    else if (args[0].Equals(typeof(int)) && args[1].Equals(typeof(Vector3)))
                    {
                        (command as Command<int, Vector3>).CommandAction(int.Parse(value_1), value_2.StringToVector3(','));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(int))&& args[1].Equals(typeof(int)))
                    {
                        (command as Command<int,int>).CommandAction(int.Parse(value_1), int.Parse(value_2));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(int)) && args[1].Equals(typeof(float)))
                    {
                        (command as Command<int, float>).CommandAction(int.Parse(value_1), float.Parse(value_2));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(int)) && args[1].Equals(typeof(string)))
                    {
                        (command as Command<int, string>).CommandAction(int.Parse(value_1), value_2);
                        result = new(true);
                        break;
                    }   //FLOAT
                    else if (args[0].Equals(typeof(float)) && args[1].Equals(typeof(Vector3)))
                    {
                        (command as Command<float, Vector3>).CommandAction(float.Parse(value_1), value_2.StringToVector3(','));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(float)) && args[1].Equals(typeof(int)))
                    {
                        (command as Command<float, int>).CommandAction(float.Parse(value_1), int.Parse(value_2));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(float)) && args[1].Equals(typeof(float)))
                    {
                        (command as Command<float, float>).CommandAction(float.Parse(value_1), float.Parse(value_2));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(float)) && args[1].Equals(typeof(string)))
                    {
                        (command as Command<float, string>).CommandAction(float.Parse(value_1), value_2);
                        result = new(true);
                        break;
                    }   //STRING
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(Vector3)))
                    {
                        (command as Command<string, Vector3>).CommandAction(value_1, value_2.StringToVector3(','));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(string)))
                    {
                        (command as Command<string, string>).CommandAction(value_1, value_2);
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(int)))
                    {
                        (command as Command<string, int>).CommandAction(value_1, int.Parse(value_2));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(float)))
                    {
                        (command as Command<string, float>).CommandAction(value_1, float.Parse(value_2));
                        result = new(true);
                        break;
                    }
                }
                else
                {
                    result = new(false, new($"Incorect command format. Command format: {command.CommandFormat}"));
                    continue;
                }
            }
        }
        public void GetCommand(string commandID, string value_1, string value_2, string value_3, ref Result<bool> result)
        {
            foreach (var (command, args) in from command in _commands
                                            where commandID.Equals(command.CommandID) && command.GetType().GetGenericArguments().Count() == 3
                                            let args = command.GetType().GetGenericArguments()
                                            select (command, args))
            {
                if (args[0].Equals(value_1.GetTypeOfThisValue()) && args[1].Equals(value_2.GetTypeOfThisValue()) && args[2].Equals(value_3.GetTypeOfThisValue()))
                {
                    //Vector3
                    if (args[0].Equals(typeof(Vector3)) && args[1].Equals(typeof(Vector3)) && args[2].Equals(typeof(Vector3)))
                    {
                        (command as Command<Vector3, Vector3, Vector3>).CommandAction(value_1.StringToVector3(','), value_2.StringToVector3(','), value_3.StringToVector3(','));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(Vector3)) && args[1].Equals(typeof(Vector3)) && args[2].Equals(typeof(int)))
                    {
                        (command as Command<Vector3, Vector3, int>).CommandAction(value_1.StringToVector3(','), value_2.StringToVector3(','), int.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(Vector3)) && args[1].Equals(typeof(int)) && args[2].Equals(typeof(Vector3)))
                    {
                        (command as Command<Vector3, int, Vector3>).CommandAction(value_1.StringToVector3(','), int.Parse(value_2), value_3.StringToVector3(','));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(Vector3)) && args[1].Equals(typeof(int)) && args[2].Equals(typeof(int)))
                    {
                        (command as Command<Vector3, int, int>).CommandAction(value_1.StringToVector3(','), int.Parse(value_2), int.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(Vector3)) && args[1].Equals(typeof(Vector3)) && args[2].Equals(typeof(float)))
                    {
                        (command as Command<Vector3, Vector3, float>).CommandAction(value_1.StringToVector3(','), value_2.StringToVector3(','), float.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(Vector3)) && args[1].Equals(typeof(float)) && args[2].Equals(typeof(Vector3)))
                    {
                        (command as Command<Vector3, float, Vector3>).CommandAction(value_1.StringToVector3(','), float.Parse(value_2), value_3.StringToVector3(','));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(Vector3)) && args[1].Equals(typeof(float)) && args[2].Equals(typeof(float)))
                    {
                        (command as Command<Vector3, float, float>).CommandAction(value_1.StringToVector3(','), float.Parse(value_2), float.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(Vector3)) && args[1].Equals(typeof(Vector3)) && args[2].Equals(typeof(string)))
                    {
                        (command as Command<Vector3, Vector3, string>).CommandAction(value_1.StringToVector3(','), value_2.StringToVector3(','), value_3);
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(Vector3)) && args[1].Equals(typeof(string)) && args[2].Equals(typeof(Vector3)))
                    {
                        (command as Command<Vector3, string, Vector3>).CommandAction(value_1.StringToVector3(','), value_2, value_3.StringToVector3(','));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(Vector3)) && args[1].Equals(typeof(string)) && args[2].Equals(typeof(string)))
                    {
                        (command as Command<Vector3, string, string>).CommandAction(value_1.StringToVector3(','), value_2, value_3);
                        result = new(true);
                        break;
                    }//INT
                    else if (args[0].Equals(typeof(int)) && args[1].Equals(typeof(int)) && args[2].Equals(typeof(int)))
                    {
                        (command as Command<int, int, int>).CommandAction(int.Parse(value_1), int.Parse(value_2), int.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(int)) && args[1].Equals(typeof(Vector3)) && args[2].Equals(typeof(Vector3)))
                    {
                        (command as Command<int, Vector3, Vector3>).CommandAction(int.Parse(value_1), value_2.StringToVector3(','), value_3.StringToVector3(','));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(int)) && args[1].Equals(typeof(int)) && args[2].Equals(typeof(Vector3)))
                    {
                        (command as Command<int, int, Vector3>).CommandAction(int.Parse(value_1), int.Parse(value_2), value_3.StringToVector3(','));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(int)) && args[1].Equals(typeof(Vector3)) && args[2].Equals(typeof(int)))
                    {
                        (command as Command<int, Vector3, int>).CommandAction(int.Parse(value_1), value_2.StringToVector3(','), int.Parse(value_3));
                        result = new(true);
                        break;
                    }                    
                    else if (args[0].Equals(typeof(int)) && args[1].Equals(typeof(int)) && args[2].Equals(typeof(float)))
                    {
                        (command as Command<int, int, float>).CommandAction(int.Parse(value_1), int.Parse(value_2), float.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(int)) && args[1].Equals(typeof(float)) && args[2].Equals(typeof(int)))
                    {
                        (command as Command<int, float, int>).CommandAction(int.Parse(value_1), float.Parse(value_2), int.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(int)) && args[1].Equals(typeof(float)) && args[2].Equals(typeof(float)))
                    {
                        (command as Command<int, float, float>).CommandAction(int.Parse(value_1), float.Parse(value_2), float.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(int)) && args[1].Equals(typeof(int)) && args[2].Equals(typeof(string)))
                    {
                        (command as Command<int, int, string>).CommandAction(int.Parse(value_1), int.Parse(value_2), value_3);
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(int)) && args[1].Equals(typeof(string)) && args[2].Equals(typeof(int)))
                    {
                        (command as Command<int, string, int>).CommandAction(int.Parse(value_1), value_2, int.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(int)) && args[1].Equals(typeof(string)) && args[2].Equals(typeof(string)))
                    {
                        (command as Command<int, string, string>).CommandAction(int.Parse(value_1), value_2, value_3);
                        result = new(true);
                        break;
                    }   //FLOAT
                    else if (args[0].Equals(typeof(float)) && args[1].Equals(typeof(float)) && args[2].Equals(typeof(float)))
                    {
                        (command as Command<float, float, float>).CommandAction(float.Parse(value_1), float.Parse(value_2), float.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(float)) && args[1].Equals(typeof(Vector3)) && args[2].Equals(typeof(float)))
                    {
                        (command as Command<float, Vector3, float>).CommandAction(float.Parse(value_1), value_2.StringToVector3(','), float.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(float)) && args[1].Equals(typeof(float)) && args[2].Equals(typeof(Vector3)))
                    {
                        (command as Command<float, float, Vector3>).CommandAction(float.Parse(value_1), float.Parse(value_2), value_3.StringToVector3(','));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(float)) && args[1].Equals(typeof(Vector3)) && args[2].Equals(typeof(Vector3)))
                    {
                        (command as Command<float, Vector3, Vector3>).CommandAction(float.Parse(value_1), value_2.StringToVector3(','), value_3.StringToVector3(','));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(float)) && args[1].Equals(typeof(int)) && args[2].Equals(typeof(float)))
                    {
                        (command as Command<float, int, float>).CommandAction(float.Parse(value_1), int.Parse(value_2), float.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(float)) && args[1].Equals(typeof(float)) && args[2].Equals(typeof(int)))
                    {
                        (command as Command<float, float, int>).CommandAction(float.Parse(value_1), float.Parse(value_2), int.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(float)) && args[1].Equals(typeof(int)) && args[2].Equals(typeof(int)))
                    {
                        (command as Command<float, int, int>).CommandAction(float.Parse(value_1), int.Parse(value_2), int.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(float)) && args[1].Equals(typeof(float)) && args[2].Equals(typeof(float)))
                    {
                        (command as Command<float,string, float>).CommandAction(float.Parse(value_1), value_2, float.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(float)) && args[1].Equals(typeof(float)) && args[2].Equals(typeof(string)))
                    {
                        (command as Command<float, float, string>).CommandAction(float.Parse(value_1), float.Parse(value_2), value_3);
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(float)) && args[1].Equals(typeof(string)) && args[2].Equals(typeof(string)))
                    {
                        (command as Command<float, string, string>).CommandAction(float.Parse(value_1), value_2, value_3);
                        result = new(true);
                        break;
                    }   //STRING
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(string)) && args[2].Equals(typeof(string)))
                    {
                        (command as Command<string, string, string>).CommandAction(value_1, value_2, value_3);
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(Vector3)) && args[2].Equals(typeof(Vector3)))
                    {
                        (command as Command<string, Vector3, Vector3>).CommandAction(value_1, value_2.StringToVector3(','), value_3.StringToVector3(','));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(string)) && args[2].Equals(typeof(Vector3)))
                    {
                        (command as Command<string, string, Vector3>).CommandAction(value_1, value_2, value_3.StringToVector3(','));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(Vector3)) && args[2].Equals(typeof(string)))
                    {
                        (command as Command<string,Vector3, string>).CommandAction(value_1, value_2.StringToVector3(','), value_3);
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(int)) && args[2].Equals(typeof(int)))
                    {
                        (command as Command<string, int, int>).CommandAction(value_1, int.Parse(value_2), int.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(string)) && args[2].Equals(typeof(int)))
                    {
                        (command as Command<string, string, int>).CommandAction(value_1, value_2, int.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(int)) && args[2].Equals(typeof(string)))
                    {
                        (command as Command<string, int, string>).CommandAction(value_1, int.Parse(value_2), value_3);
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(float)) && args[2].Equals(typeof(float)))
                    {
                        (command as Command<string, float, float>).CommandAction(value_1, float.Parse(value_2), float.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(string)) && args[2].Equals(typeof(float)))
                    {
                        (command as Command<string, string, float>).CommandAction(value_1, value_2, float.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(float)) && args[2].Equals(typeof(string)))
                    {
                        (command as Command<string, float, string>).CommandAction(value_1, float.Parse(value_2), value_3);
                        result = new(true);
                        break;
                    }   //MIX - String
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(float)) && args[2].Equals(typeof(Vector3)))
                    {
                        (command as Command<string, float, Vector3>).CommandAction(value_1, float.Parse(value_2), value_3.StringToVector3(','));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(Vector3)) && args[2].Equals(typeof(float)))
                    {
                        (command as Command<string, Vector3, float>).CommandAction(value_1, value_2.StringToVector3(','), float.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(Vector3)) && args[2].Equals(typeof(int)))
                    {
                        (command as Command<string, Vector3, int>).CommandAction(value_1, value_2.StringToVector3(','), int.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(int)) && args[2].Equals(typeof(Vector3)))
                    {
                        (command as Command<string, int, Vector3>).CommandAction(value_1, int.Parse(value_2), value_3.StringToVector3(','));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(float)) && args[2].Equals(typeof(int)))
                    {
                        (command as Command<string, float, int>).CommandAction(value_1, float.Parse(value_2), int.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(int)) && args[2].Equals(typeof(float)))
                    {
                        (command as Command<string, int, float>).CommandAction(value_1, int.Parse(value_2), float.Parse(value_3));
                        result = new(true);
                        break;
                    }   //MIX - float
                    else if (args[0].Equals(typeof(float)) && args[1].Equals(typeof(string)) && args[2].Equals(typeof(int)))
                    {
                        (command as Command<float, string, int>).CommandAction(float.Parse(value_1), value_2, int.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(float)) && args[1].Equals(typeof(int)) && args[2].Equals(typeof(string)))
                    {
                        (command as Command<float, int, string>).CommandAction(float.Parse(value_1), int.Parse(value_2), value_3);
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(float)) && args[1].Equals(typeof(string)) && args[2].Equals(typeof(int)))
                    {
                        (command as Command<float, string, Vector3>).CommandAction(float.Parse(value_1), value_2, value_3.StringToVector3(','));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(float)) && args[1].Equals(typeof(Vector3)) && args[2].Equals(typeof(string)))
                    {
                        (command as Command<float, Vector3, string>).CommandAction(float.Parse(value_1), value_2.StringToVector3(','), value_3);
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(float)) && args[1].Equals(typeof(Vector3)) && args[2].Equals(typeof(int)))
                    {
                        (command as Command<float, Vector3, int>).CommandAction(float.Parse(value_1), value_2.StringToVector3(','), int.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(float)) && args[1].Equals(typeof(int)) && args[2].Equals(typeof(Vector3)))
                    {
                        (command as Command<float, int, Vector3>).CommandAction(float.Parse(value_1), int.Parse(value_2), value_3.StringToVector3(','));
                        result = new(true);
                        break;
                    }   //MIX - int
                    else if (args[0].Equals(typeof(int)) && args[1].Equals(typeof(string)) && args[2].Equals(typeof(float)))
                    {
                        (command as Command<int, string, float>).CommandAction(int.Parse(value_1), value_2, float.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(int)) && args[1].Equals(typeof(float)) && args[2].Equals(typeof(string)))
                    {
                        (command as Command<int, float, string>).CommandAction(int.Parse(value_1), float.Parse(value_2), value_3);
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(int)) && args[1].Equals(typeof(string)) && args[2].Equals(typeof(int)))
                    {
                        (command as Command<int, string, Vector3>).CommandAction(int.Parse(value_1), value_2, value_3.StringToVector3(','));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(int)) && args[1].Equals(typeof(Vector3)) && args[2].Equals(typeof(string)))
                    {
                        (command as Command<int, Vector3, string>).CommandAction(int.Parse(value_1), value_2.StringToVector3(','), value_3);
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(int)) && args[1].Equals(typeof(Vector3)) && args[2].Equals(typeof(float)))
                    {
                        (command as Command<int, Vector3, float>).CommandAction(int.Parse(value_1), value_2.StringToVector3(','), float.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(int)) && args[1].Equals(typeof(float)) && args[2].Equals(typeof(Vector3)))
                    {
                        (command as Command<int, float, Vector3>).CommandAction(int.Parse(value_1), float.Parse(value_2), value_3.StringToVector3(','));
                        result = new(true);
                        break;
                    }
                    //MIX - Vector3
                    else if (args[0].Equals(typeof(Vector3)) && args[1].Equals(typeof(string)) && args[2].Equals(typeof(float)))
                    {
                        (command as Command<Vector3, string, float>).CommandAction(value_1.StringToVector3(','), value_2, float.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(Vector3)) && args[1].Equals(typeof(float)) && args[2].Equals(typeof(string)))
                    {
                        (command as Command<Vector3, float, string>).CommandAction(value_1.StringToVector3(','), float.Parse(value_2), value_3);
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(Vector3)) && args[1].Equals(typeof(string)) && args[2].Equals(typeof(int)))
                    {
                        (command as Command<Vector3, string, int>).CommandAction(value_1.StringToVector3(','), value_2, int.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(Vector3)) && args[1].Equals(typeof(int)) && args[2].Equals(typeof(string)))
                    {
                        (command as Command<Vector3, int, string>).CommandAction(value_1.StringToVector3(','), int.Parse(value_2), value_3);
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(Vector3)) && args[1].Equals(typeof(int)) && args[2].Equals(typeof(float)))
                    {
                        (command as Command<Vector3, int, float>).CommandAction(value_1.StringToVector3(','), int.Parse(value_2), float.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(Vector3)) && args[1].Equals(typeof(float)) && args[2].Equals(typeof(int)))
                    {
                        (command as Command<Vector3, float, int>).CommandAction(value_1.StringToVector3(','), float.Parse(value_2), int.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    //MIX - string
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(Vector3)) && args[2].Equals(typeof(float)))
                    {
                        (command as Command<string, Vector3, float>).CommandAction(value_1, value_2.StringToVector3(','), float.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(float)) && args[2].Equals(typeof(Vector3)))
                    {
                        (command as Command<string, float, Vector3>).CommandAction(value_1, float.Parse(value_2), value_3.StringToVector3(','));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(Vector3)) && args[2].Equals(typeof(int)))
                    {
                        (command as Command<string, Vector3, int>).CommandAction(value_1, value_2.StringToVector3(','), int.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(int)) && args[2].Equals(typeof(Vector3)))
                    {
                        (command as Command<string, int, Vector3>).CommandAction(value_1, int.Parse(value_2), value_3.StringToVector3(','));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(int)) && args[2].Equals(typeof(float)))
                    {
                        (command as Command<string, int, float>).CommandAction(value_1, int.Parse(value_2), float.Parse(value_3));
                        result = new(true);
                        break;
                    }
                    else if (args[0].Equals(typeof(string)) && args[1].Equals(typeof(float)) && args[2].Equals(typeof(int)))
                    {
                        (command as Command<string, float, int>).CommandAction(value_1, float.Parse(value_2), int.Parse(value_3));
                        result = new(true);
                        break;
                    }
                }
                else
                {
                    result = new(false, new($"Incorect command format. Command format: {command.CommandFormat}"));
                    continue;
                }
            }
        }
        public virtual void InitCommand(string id, string description, string format, Action command)
        {
            _commands.Add(new Command(id, description, format, command));
        }
        public virtual void InitCommand<T1>(string id, string description, string format, Action<T1> command)
        {
            _commands.Add(new Command<T1>(id, description, format, command));
        }
        public virtual void InitCommand<T1, T2>(string id, string description, string format, Action<T1, T2> command)
        {
            _commands.Add(new Command<T1, T2>(id, description, format, command));
        }
        public virtual void InitCommand<T1,T2,T3>(string id, string description, string format, Action<T1,T2,T3> command)
        {
            _commands.Add(new Command<T1,T2,T3>(id, description, format, command));
        }
    }
}