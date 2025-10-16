using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UnityToolbarExtender
{
	[InitializeOnLoad]
	public static class ToolbarExtender
	{
		private static readonly int ToolCount;
		private static GUIStyle _commandStyle;

		public static readonly List<Action> LeftToolbarGUI = new();
		public static readonly List<Action> RightToolbarGUI = new();

		static ToolbarExtender()
		{
			Type toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");
			
			const string fieldName = "k_ToolCount";
			
			FieldInfo toolIcons = toolbarType.GetField(fieldName,
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
			
			ToolCount = toolIcons != null ? ((int) toolIcons.GetValue(null)) : 8;
	
			ToolbarCallback.OnToolbarGUI = OnGUI;
			ToolbarCallback.OnToolbarGUILeft = GUILeft;
			ToolbarCallback.OnToolbarGUIRight = GUIRight;
		}
		
		private const float SPACE = 8;
		public const float LARGE_SPACE = 20;
		private const float BUTTON_WIDTH = 32;
		private const float DROPDOWN_WIDTH = 80;
		private const float PLAY_PAUSE_STOP_WIDTH = 140;

		private static void OnGUI()
		{
			_commandStyle ??= new GUIStyle("CommandLeft");

			float screenWidth = EditorGUIUtility.currentViewWidth;
			
			float playButtonsPosition = Mathf.RoundToInt ((screenWidth - PLAY_PAUSE_STOP_WIDTH) / 2);

			Rect leftRect = new(0, 0, screenWidth, Screen.height);
			leftRect.xMin += SPACE;
			leftRect.xMin += BUTTON_WIDTH * ToolCount;
			leftRect.xMin += SPACE;
			leftRect.xMin += 64 * 2;
			leftRect.xMax = playButtonsPosition;

			Rect rightRect = new(0, 0, screenWidth, Screen.height)
			{
				xMin = playButtonsPosition
			};
			rightRect.xMin += _commandStyle.fixedWidth * 3;
			rightRect.xMax = screenWidth;
			rightRect.xMax -= SPACE;
			rightRect.xMax -= DROPDOWN_WIDTH;
			rightRect.xMax -= SPACE;
			rightRect.xMax -= DROPDOWN_WIDTH;
			rightRect.xMax -= SPACE;
			rightRect.xMax -= DROPDOWN_WIDTH;
			rightRect.xMax -= SPACE;
			rightRect.xMax -= BUTTON_WIDTH;
			rightRect.xMax -= SPACE;
			rightRect.xMax -= 78;
			
			leftRect.xMin += SPACE;
			leftRect.xMax -= SPACE;
			rightRect.xMin += SPACE;
			rightRect.xMax -= SPACE;
			
			leftRect.y = 4;
			leftRect.height = 22;
			rightRect.y = 4;
			rightRect.height = 22;
			leftRect.y = 5;
			leftRect.height = 24;
			rightRect.y = 5;
			rightRect.height = 24;

			if (leftRect.width > 0)
			{
				GUILayout.BeginArea(leftRect);
				GUILayout.BeginHorizontal();
				foreach (Action handler in LeftToolbarGUI)
				{
					handler();
				}

				GUILayout.EndHorizontal();
				GUILayout.EndArea();
			}

			if (!(rightRect.width > 0)) return;
			{
				GUILayout.BeginArea(rightRect);
				GUILayout.BeginHorizontal();
				foreach (Action handler in RightToolbarGUI)
				{
					handler();
				}

				GUILayout.EndHorizontal();
				GUILayout.EndArea();
			}
		}

		private static void GUILeft() {
			GUILayout.BeginHorizontal();
			foreach (Action handler in LeftToolbarGUI)
			{
				handler();
			}
			GUILayout.EndHorizontal();
		}

		private static void GUIRight() {
			GUILayout.BeginHorizontal();
			foreach (Action handler in RightToolbarGUI)
			{
				handler();
			}
			GUILayout.EndHorizontal();
		}
	}
}
