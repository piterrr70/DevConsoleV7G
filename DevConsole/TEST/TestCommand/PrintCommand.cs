using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V7G.Console;

namespace V7G
{
    [Console("Print Test", "Print in console test command", ConsoleAttributeTarget.Command)]
    public class PrintCommand : ConsoleCommandBase
    {

        private void Awake()
        {
            InitCommand("print_dual", "Print in console test command x2.", "print_test", () =>
            {
                Debug.Log("This is test command");
            });
            InitCommand("print_testy", "Print in console test command.", "print_test", () =>
            {
                Debug.Log("This is test command");
            });
            InitCommand("print_test", "Print in console test command.", "print_test", () =>
               {
                   Debug.Log("This is test command");
               });
            InitCommand<int>("print_test", "Print in console test command with value.", "print_test <value>", (x) =>
            {
                Debug.Log($"This is test command with some value :{x}");
            });
            InitCommand<float>("print_test", "Print in console test command with float value.", "print_test <value>", (x) =>
            {
                Debug.Log($"This is test command with some float value :{x}");
            });
            InitCommand<string, int>("print_test", "Print in console test command with value.", "print_test <string> <int>", (x, y) =>
             {
                 Debug.Log($"This is test command with some value2 :<string> {x}, <int> {y}");
             });
            InitCommand<string, float>("print_test", "Print in console test command with float value.", "print_test <string> <float>", (x, y) =>
            {
                Debug.Log($"This is test command with some value2 :<string> {x}, <float> {y}");
            });

            InitCommand<Vector3>("move", "Move to position", "move <Vector3>", (y) =>
            {
                   Debug.Log($"This is test command. Move player to position: {y}");
            });
            InitCommand<int, Vector3>("move", "Move player with ID to position", "move <ID> <Vector3>", (x,y) =>
            {
                Debug.Log($"This is test command. Move player with ID {x} to position: {y}");
            });
            InitCommand<string, Vector3>("move", "Move to position", "move <player_name> <Vector3>", (x, y) =>
            {
                Debug.Log($"This is test command. Move player with name {x} to position: {y}");
            });
        }
    }
}