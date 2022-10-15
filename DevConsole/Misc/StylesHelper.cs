#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace V7G.Console
{
    public class StylesHelper
    {
        static GUIStyle _labelStyleSettable;
        public static GUIStyle LabelStyleSettable(int fontSize = 11, FontStyle fontStyle = FontStyle.Normal, Color fontColor = new Color())
        {
            _labelStyleSettable = new GUIStyle("Label") { fontStyle = fontStyle, fontSize = fontSize };
            _labelStyleSettable.normal.textColor = fontColor != new Color() ? fontColor : Color.white;
            return _labelStyleSettable;
        }

        public static GUIStyle Foldout(int fontSize = 0, FontStyle fontStyle = FontStyle.Normal, bool isActive = true, int isFixedWidthValue = 0, Color? colorIfOpened = null, TextClipping textClipping = TextClipping.Overflow)
        {
            var FoldoutStyle = new GUIStyle(EditorStyles.foldout);
            FoldoutStyle.margin.left = 10;
            FoldoutStyle.normal.textColor = isActive ? Color.white : colorIfOpened ?? Color.red;   // when is closed
            FoldoutStyle.onNormal.textColor = isActive ? Color.white : colorIfOpened ?? Color.red; // when is open
            //FoldoutStyle.onActive.textColor = profileRef.mapGenerators[i].isActive ? Color.white : Color.red;
            FoldoutStyle.fixedWidth = isFixedWidthValue;
            FoldoutStyle.clipping = textClipping;
            FoldoutStyle.fontStyle = fontStyle;
            FoldoutStyle.fontSize = fontSize == 0 ? FoldoutStyle.fontSize : fontSize;
            return FoldoutStyle;
        }
    }
}
#endif