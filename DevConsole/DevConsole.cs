using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.InputSystem;

namespace V7G.Console
{
    [DefaultExecutionOrder(-100)]
    public class DevConsole : Singletone<DevConsole>
    {
        #region Fields

        public InputAction DevConsoleAction;
        public InputAction DevConsoleConfirmAction;
        public InputAction DevConsoleHistoryAction;
        public InputAction DevConsoleHistorySearchAction;


        [HideInInspector] [SerializeField] private bool showConsole;
        [HideInInspector] [SerializeField] private bool showConsoleHistory;
        string _consoleInput = "";

        [HideInInspector] [SerializeField] private List<string> _inputHistory = new();
        [HideInInspector] [SerializeField] private List<string> _consoleLogs = new();

        [HideInInspector]
        [SerializeField] private List<OnConsoleShowActionsBase> onConsoleShowActions = new List<OnConsoleShowActionsBase>();
        internal List<OnConsoleShowActionsBase> OnConsoleShowActions => onConsoleShowActions;
        
        [HideInInspector][SerializeField] private List<ConsoleCommandBase> CommandObjects = new List<ConsoleCommandBase>();
        internal List<ConsoleCommandBase> Commands => CommandObjects;
        #endregion

        private void Awake()
        {
            InitializeInternalCommands();
        }        

        private void Start()
        {
            RegisterInputActionsCallbacks();
        }

        private void InitializeInternalCommands()
        {
            var helpCommand = gameObject.AddComponent<InternalHelpCommand>();
            helpCommand.InitCommand("help", "Show all avaable commands", "help", () =>
            {
                foreach (var commandObject in CommandObjects)
                {
                    foreach (CommandBase command in commandObject.Commands)
                    {
                        _consoleLogs.Add(command.CommandFormat);
                    }
                }
            });
            CommandObjects.Add(helpCommand);
            var clearCommand = gameObject.AddComponent<InternalClearCommand>();
            clearCommand.InitCommand("clear", "Clear Console", "clear", () =>
            {
                _consoleLogs.Clear();
            });
            CommandObjects.Add(clearCommand);
        }

        private void RegisterInputActionsCallbacks()
        {
            if (DevConsoleAction != null)
            {
                DevConsoleAction.performed += DevConsoleAction_performed;
            }

            if (DevConsoleConfirmAction != null)
            {
                DevConsoleConfirmAction.performed += DevConsoleConfirmAction_performed;
            }
            if (DevConsoleHistoryAction != null)
            {
                DevConsoleHistoryAction.performed += DevConsoleHistoryAction_performed;
            }
            if (DevConsoleHistorySearchAction != null)
            {
                DevConsoleHistorySearchAction.performed += DevConsoleHistorySearchAction_performed;
            }
        }

        void OnEnable()
        {
            DevConsoleAction.Enable();
            DevConsoleConfirmAction.Enable();
            DevConsoleHistoryAction.Enable();
            DevConsoleHistorySearchAction.Enable();

        }

        void OnDisable()
        {
            DevConsoleAction.Disable();
            DevConsoleConfirmAction.Disable();
            DevConsoleHistoryAction.Disable();
            DevConsoleHistorySearchAction.Disable();
        }

        private void DevConsoleAction_performed(InputAction.CallbackContext callbackContext)
        {
            showConsole = !showConsole;

            OnConsoleShow(showConsole);
        }

        private void DevConsoleConfirmAction_performed(InputAction.CallbackContext callbackContext)
        {
            if (showConsole)
            {
                TryPerformCommandAction(_consoleInput);
                _consoleInput = "";
            }
        }

        private void DevConsoleHistoryAction_performed(InputAction.CallbackContext callbackContext)
        {
            if (showConsole)
            {
                showConsoleHistory = !showConsoleHistory;
            }
        }
        private void DevConsoleHistorySearchAction_performed(InputAction.CallbackContext callbackContext)
        {
            if (showConsole)
            {
                var value = callbackContext.action.ReadValue<float>();
                SearchInHistory(value);
            }
        }

        void OnConsoleShow(bool value)
        {
            foreach (IOnConsoleShowActions onConsoleShowAction in onConsoleShowActions)
            {
                var returnValue = onConsoleShowAction?.OnShow(value);
                showConsole = (bool)returnValue;
            }
        }

        private void TryPerformCommandAction(string consoleInput)
        {
            if (consoleInput.Length == 0) return;

            _inputHistory.Add(consoleInput);
            _consoleLogs.Add($"> {consoleInput}");
            Result<bool> result = new(false, new($"Command {consoleInput} not found."));

            foreach (var commandObject in CommandObjects)
            {
                commandObject.TryPerformCommand(consoleInput, ref result);


                if (result.result)
                {
                    break;
                }
            }

            if (result.error != null)
                _consoleLogs.Add($"-- {result.error.message}");
        }

        int currentSearchIndex = -1;
        private void SearchInHistory(float value)
        {
            if (_inputHistory.Count == 0) return;
            string valueToCopy = "";
            switch (value)
            {
                case 1:
                    if (currentSearchIndex == -1)
                    {
                        valueToCopy = _inputHistory[_inputHistory.Count - 1];
                    }
                    else
                    {
                        if (currentSearchIndex - 1 >= 0)
                        {
                            valueToCopy = _inputHistory[currentSearchIndex - 1];
                        }
                    }
                    _consoleInput = valueToCopy;
                    currentSearchIndex = _inputHistory.IndexOf(valueToCopy);
                    break;
                case -1:
                    if (currentSearchIndex == -1)
                    {
                        valueToCopy = _inputHistory[_inputHistory.Count - 1];
                    }
                    else if (currentSearchIndex == _inputHistory.Count - 1) return;
                    else
                    {
                        if (currentSearchIndex + 1 < _inputHistory.Count)
                        {
                            valueToCopy = _inputHistory[currentSearchIndex + 1];
                        }
                    }
                    _consoleInput = valueToCopy;
                    currentSearchIndex = _inputHistory.IndexOf(valueToCopy);
                    break;
            }
        }

        Vector2 scroll;
        private void OnGUI()
        {
            if (!showConsole) { return; }

            if (GUI.GetNameOfFocusedControl() != "input-field")
            {
                GUI.FocusControl("input-field");
            }

            float y = 0f;

            if (showConsoleHistory)
            {                
                GUI.Box(new Rect(0, y, Screen.width, 100), "");

                Rect viewport = new(0, 0, Screen.width - 30, 20 * _consoleLogs.Count);

                scroll = GUI.BeginScrollView(new(0, y + 5f, Screen.width, 90), scroll, viewport);

                int stringIndex = 0;
                for (int i = 0; i < _consoleLogs.Count; i++)
                {
                    Rect labelRect = new(5, 20 * stringIndex, viewport.width - 100, 20);
                    GUI.Label(labelRect, _consoleLogs[i]);
                    stringIndex++;
                }
                GUI.EndScrollView();

                y += 100;
            }
            GUI.Box(new Rect(0, y, Screen.width, 30), "");
            GUI.backgroundColor = new Color(0, 0, 0, 0);
            GUI.SetNextControlName("input-field");            
            _consoleInput = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), _consoleInput);

            //help tips
            List<CommandBase> commandsToShow = new();
            if (_consoleInput.Length > 0)
            {
                y += 30f;
                

                foreach (var commands in Commands)
                {
                    foreach (var command in commands.Commands)
                    {
                        bool canShow = true;
                        int inputCharIndex = 0;
                        for (int i = 0; i < _consoleInput.Length; i++)
                        {
                            try
                            {
                                if (command.CommandID.Length > i)
                                {
                                    if (!_consoleInput[i].Equals(command.CommandID[i]))
                                    {
                                        canShow = false;
                                    }
                                }
                                else
                                {
                                    canShow = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.Log($"Current index {i}, CommandID: {command.CommandID}, command lenght: {command.CommandID.Length}, ERROR {ex}");
                            }
                            
                        }

                        if (canShow)
                        {
                            if (!commandsToShow.Exists(c => c.Equals(command)))
                            {
                                commandsToShow.Add(command);
                            }
                        }
                        else
                        {
                            if (commandsToShow.Exists(c => c.Equals(command)))
                            {
                                commandsToShow.Remove(command);
                            }
                        }
                    }
                }
                
            }

            if (commandsToShow.Count > 0)
            {
                GUI.backgroundColor = Color.black;
                GUI.Box(new Rect(0, y, Screen.width, 20*commandsToShow.Count),"");
                int stringCommandsIndex = 0;
                foreach (var command in commandsToShow)
                {
                    Rect labelRect = new(5, y + 20 * stringCommandsIndex, Screen.width - 20f, 20);
                    GUI.Label(labelRect, command.CommandFormat);
                    stringCommandsIndex++;
                }
            }
            //if (GUI.GetNameOfFocusedControl().Equals("input-field"))
            //{
            //    Debug.Log("FieldFocus");
            //}
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(DevConsole))]
    public class DevConsoleV7GInspector : Editor
    {
        private DevConsole Target;

        #region Commands
        List<ConsoleAttribute> Commands = new List<ConsoleAttribute>();
        List<ConsoleAttribute> OnConsoleShowActions = new List<ConsoleAttribute>();

        protected readonly List<Editor> OnConsoleShowActionsEditors = new List<Editor>();
        protected readonly List<Editor> CommandsEditors = new List<Editor>();
        #endregion

        private static bool ActionInputFoldout = true; 
        private static bool OnConsoleShowActionsFoldout = true; 
        private static bool AddonsFoldout = true; 
        private void OnEnable()
        {
            Target = (DevConsole)target;
            #region GetOnConsoleShowActions
            AttributeHelper.GetCommandsByTarget(ref OnConsoleShowActions, ConsoleAttributeTarget.OnShowActions);


            foreach (OnConsoleShowActionsBase OnShowAction in Target.GetComponentsInChildren<OnConsoleShowActionsBase>())
            {
                if (OnShowAction != null)
                {
                    OnShowAction.hideFlags = HideFlags.HideInInspector;
                }
            }
            #endregion
            #region GetCommands
            AttributeHelper.GetCommandsByTarget(ref Commands, ConsoleAttributeTarget.Command);


            foreach (ConsoleCommandBase Comp in Target.GetComponentsInChildren<ConsoleCommandBase>())
            {
                if (Comp != null)
                {
                    Comp.hideFlags = HideFlags.HideInInspector;
                }
            }
            #endregion
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Input Actions", StylesHelper.LabelStyleSettable(14, FontStyle.BoldAndItalic, fontColor: Color.red), GUILayout.ExpandHeight(true));
            ActionInputFoldout = EditorGUILayout.Foldout(ActionInputFoldout, new GUIContent("Input Actions"), true, StylesHelper.Foldout(fontStyle: FontStyle.Italic));
            if (ActionInputFoldout)
            {
                base.OnInspectorGUI();
            }
            DrawOnConsolShow();
            DrawCommands();
        }

        private void DrawOnConsolShow()
        {
            EditorGUILayout.LabelField("On Console Show Actions", StylesHelper.LabelStyleSettable(14, FontStyle.BoldAndItalic, fontColor: Color.red), GUILayout.ExpandHeight(true));
            OnConsoleShowActionsFoldout = EditorGUILayout.Foldout(OnConsoleShowActionsFoldout, new GUIContent("On Console Show Actions"), true, StylesHelper.Foldout(fontStyle: FontStyle.Italic));
            if (OnConsoleShowActionsFoldout)
            {
                DrawOnConsoleShowActionsList();
            }
        }

        private void DrawOnConsoleShowActionsList()
        {
            EditorGUILayout.BeginVertical();

            if (Commands.Count == 0)
            {
                GUILayout.BeginVertical("box");
                GUILayout.Label($"No On Console Show Action found for this component.", EditorStyles.miniLabel);
                GUILayout.EndVertical();
            }

            foreach (var onConsoleShowAction in OnConsoleShowActions)
            {
                GUILayout.BeginVertical("box");

                GUILayout.BeginHorizontal();

                GUILayout.Label(onConsoleShowAction.Name, EditorStyles.largeLabel);

                GUILayout.FlexibleSpace();

                if (Target.gameObject.GetComponent(onConsoleShowAction.Behaviour) != null)
                {  
                    GUI.color = Color.white;

                    GUI.color = new Color(1f, 0f, 0f);
                    if (GUILayout.Button("Disable", GUILayout.Width(120)))
                    {
                        try
                        {
                            DestroyImmediate(Target.gameObject.GetComponent(onConsoleShowAction.Behaviour), true);
                            HandleNullObjects();
                            break;
                        }
                        catch { }
                    }
                    GUI.color = Color.white;
                }
                else
                {
                    GUI.color = new Color(0f, 1.5f, 0f);
                    if (GUILayout.Button("Enable", GUILayout.Width(120)))
                    {
                        if (Target.gameObject.GetComponent(onConsoleShowAction.Behaviour) != null)
                        {
                            return;
                        }

                        Component Comp = Target.gameObject.AddComponent(onConsoleShowAction.Behaviour);
                        Comp.hideFlags = HideFlags.HideInInspector;

                        Target.OnConsoleShowActions.Add((OnConsoleShowActionsBase)Comp);

                        EditorUtility.SetDirty(target);
                        Repaint();
                    }
                    GUI.color = Color.white;
                }

                GUILayout.EndHorizontal();

                GUI.color = Color.white;

                GUILayout.Label(onConsoleShowAction.Description, EditorStyles.wordWrappedMiniLabel);

                GUILayout.BeginHorizontal();
                if (Target.gameObject.GetComponent(onConsoleShowAction.Behaviour) != null)
                {
                    GUILayout.Space(13f);
                    GUILayout.BeginVertical();
                    UnityEditor.Editor OnConsoleShowActionEditor = CreateEditor(Target.gameObject.GetComponent(onConsoleShowAction.Behaviour));
                    OnConsoleShowActionEditor.OnInspectorGUI();
                    OnConsoleShowActionsEditors.Add(OnConsoleShowActionEditor);
                    GUILayout.EndVertical();
                    GUILayout.Space(13f);
                }
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();

        }

        void DrawCommands()
        {
            EditorGUILayout.LabelField("Console Commands", StylesHelper.LabelStyleSettable(14, FontStyle.BoldAndItalic, fontColor: Color.red), GUILayout.ExpandHeight(true));
            AddonsFoldout = EditorGUILayout.Foldout(AddonsFoldout, new GUIContent("Console Commands"), true, StylesHelper.Foldout(fontStyle: FontStyle.Italic));
            if (AddonsFoldout)
            {
                DrawCommandsList();
            }
        }
        private void DrawCommandsList()
        {

            EditorGUILayout.BeginVertical();

            if (Commands.Count == 0)
            {
                GUILayout.BeginVertical("box");
                GUILayout.Label($"No Console Commands found for this component.", EditorStyles.miniLabel);
                GUILayout.EndVertical();
            }

            foreach (var command in Commands)
            {
                GUILayout.BeginVertical("box");

                GUILayout.BeginHorizontal();

                GUILayout.Label(command.Name, EditorStyles.largeLabel);

                GUILayout.FlexibleSpace();

                if (Target.gameObject.GetComponent(command.Behaviour) != null)
                {
                    GUI.color = Color.white;

                    GUI.color = new Color(1f, 0f, 0f);
                    if (GUILayout.Button("Disable", GUILayout.Width(120)))
                    {
                        try
                        {
                            DestroyImmediate(Target.gameObject.GetComponent(command.Behaviour), true);
                            HandleNullObjects();
                            break;
                        }
                        catch { }
                    }
                    GUI.color = Color.white;
                }
                else
                {
                    GUI.color = new Color(0f, 1.5f, 0f);
                    if (GUILayout.Button("Enable", GUILayout.Width(120)))
                    {
                        if (Target.gameObject.GetComponent(command.Behaviour) != null)
                        {
                            return;
                        }

                        Component Comp = Target.gameObject.AddComponent(command.Behaviour);
                        Comp.hideFlags = HideFlags.HideInInspector;

                        Target.Commands.Add((ConsoleCommandBase)Comp);

                        EditorUtility.SetDirty(target);
                        Repaint();
                    }
                    GUI.color = Color.white;
                }

                GUILayout.EndHorizontal();

                GUI.color = Color.white;

                GUILayout.Label(command.Description, EditorStyles.wordWrappedMiniLabel);

                GUILayout.BeginHorizontal();
                if (Target.gameObject.GetComponent(command.Behaviour) != null)
                {
                    GUILayout.Space(13f);
                    GUILayout.BeginVertical();
                    UnityEditor.Editor CommandEditor = CreateEditor(Target.gameObject.GetComponent(command.Behaviour));
                    CommandEditor.OnInspectorGUI();
                    CommandsEditors.Add(CommandEditor);
                    GUILayout.EndVertical();
                    GUILayout.Space(13f);
                }
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();

        }

        private void HandleNullObjects()
        {
            Target.Commands.RemoveAll(o => o == null);
            Target.OnConsoleShowActions.RemoveAll(o => o == null);

            EditorUtility.SetDirty(target);
            Repaint();
        }


    }
#endif
}
