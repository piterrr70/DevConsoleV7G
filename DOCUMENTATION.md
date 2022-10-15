# Documentation
In-game Developer Console for Unity by Video7Games (v1.0.0b).</br>

![image](https://user-images.githubusercontent.com/33598734/196002765-fa8739e2-dcbe-4bf0-acce-6ae3f11a0a99.png)

## Setup
1. Add DevConsoleV7G Component
2. Setup Input Actions
* Search history Actions need to be 1D Axis (positive/negative)

![AC1](https://user-images.githubusercontent.com/33598734/196002917-6e889b2f-cc29-4223-b794-dbe7322c2886.png)

### Usage
When the game is running, press ``<Console Action>`` to toggle the dev console window. The window has an input field along the bottom where commands can be entered. Pressing ``<Confirm Action>`` will execute the typed command.
- Use the ``<Search Input>`` / ``<Search Input>`` to cycle through the command history or suggested commands
- Press ``<History Action>`` to open History Tab

#### On Console Show Action
Trigger Action when Console has been Shown/Closed.

#### Commands
Commands are in the form: <b>commandName param_1 ... param_3</b>. The command doesn't have to have parameters!


#### Scripting
The dev console can be accessed via the ``DevConsole`` static class in the ``V7G.DevConsoleV7G`` namespace.
- ``EnableConsole(bool value)``: enable/disable console entirely (disabled by default in release builds).
- ``Open/CloseConsole()``: open/close console window.
- ``Log()``: log a message to the console.

##### Example
```cs
using V7G.DevConsoleV7G;
DevConsole.EnableConsole(true);
DevConsole.Log("Hello world!");
```

### Custom On Console Show Action MonoBehavior Component
On Console Show Action is MonoBehavior Component that can be enabled/disabled.
How to add new on console show action component.
Create new script, add ``` ConsoleAttribute["name",ConsoleAttributeTarget.OnShowActions] ``` and inherit class from ```csharp OnConsoleShowActionsBase ```.

```csharp
[Console("Test On Console Show Action", "Test Component what to do when console is shown", ConsoleAttributeTarget.OnShowActions)]
public class TestOnConsoleShowAction : OnConsoleShowActionsBase {}
```

### Custom Commands MonoBehavior Component
Commands are based on MonoBehavior Component that can be enabled/disabled.
How to add new command component.
Create new script, add ``` ConsoleAttribute["name",ConsoleAttributeTarget.Command] ``` and inherit class from ```csharp ConsoleCommandBase ```.

```csharp
[Console("Test Command", "This is test command", ConsoleAttributeTarget.Command)]
public class TestCommand : ConsoleCommandBase {}
```

#### Parameters
Default supported parameter types 
* ```string```
* ```int```
* ```float``` example of correct float param: ```1.2```
* ```Vector3```
* ```bool``` coming soon...


#### Example of On Console Show Component
```cs
[Console("Test On Console Show Component", "Test On Console Show Behaviour", ConsoleAttributeTarget.OnShowActions)]
    public class SurvivalTemplateProConsoleAction : OnConsoleShowActionsBase
    {
      public override bool OnShow(bool value)
      {
        if(value)
        {
          
        }else if(!value)
        {
          
        }
      }
    }
```

#### Example of Command Component
```cs
    [Console("Test Command Component", "This is just test command component", ConsoleAttributeTarget.Command)]
    public class CommandTest : ConsoleCommandBase
    {
      private void Awake()
            {
                InitCommand("print_dual", "Print in console test command x2.", "print_test", () =>
                {
                    Debug.Log("This is test command");
                });
                InitCommand<int>("print_test", "Print in console test command with value.", "print_test <value>", (x) =>
                {
                    Debug.Log($"This is test command with some value :{x}");
                });
                //etc...
            }
    }
```
