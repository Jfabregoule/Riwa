using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace UnityToolbarExtender
{
	public static class ToolbarCallback
	{
		private static readonly Type ToolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");

		private static ScriptableObject _currentToolbar;

		/// <summary>
		/// Callback for toolbar OnGUI method.
		/// </summary>
		public static Action OnToolbarGUI;
		public static Action OnToolbarGUILeft;
		public static Action OnToolbarGUIRight;
		
		static ToolbarCallback()
		{
			EditorApplication.update -= OnUpdate;
			EditorApplication.update += OnUpdate;
		}

		private static void OnUpdate()
		{
			if (_currentToolbar != null) return;
			// Find toolbar
			Object[] toolbars = Resources.FindObjectsOfTypeAll(ToolbarType);
			_currentToolbar = toolbars.Length > 0 ? (ScriptableObject) toolbars[0] : null;
			
			if (_currentToolbar == null) return;
			FieldInfo root = _currentToolbar.GetType().GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);
			object rawRoot = root?.GetValue(_currentToolbar);
			VisualElement mRoot = rawRoot as VisualElement;
			RegisterCallback("ToolbarZoneLeftAlign", OnToolbarGUILeft);
			RegisterCallback("ToolbarZoneRightAlign", OnToolbarGUIRight);
			return;

			void RegisterCallback(string receivedRoot, Action cb) {
				VisualElement toolbarZone = mRoot.Q(receivedRoot);

				VisualElement parent = new()
				{
					style = {
						flexGrow = 1,
						flexDirection = FlexDirection.Row,
					}
				};
				IMGUIContainer container = new();
				container.style.flexGrow = 1;
				container.onGUIHandler += () => { 
					cb?.Invoke();
				}; 
				parent.Add(container);
				toolbarZone.Add(parent);
			}
		}
	}
}
