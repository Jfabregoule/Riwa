//#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace UnityToolbarExtender.Examples
{
    public abstract class ToolbarStyles
    {
        private static readonly GUIStyle CommandButtonStyle;
        private static readonly GUIStyle CommandButtonStyle2;

        static ToolbarStyles()
        {
            Texture2D normalColor = MakeTexture(new Color(0.2196079f, 0.2196079f, 0.2196079f, 1f));
            Texture2D hoverColor = MakeTexture(new Color(0.2352941f, 0.2352941f, 0.2352941f, 1f));

            GUIStyleState normalState = new()
            {
                background = normalColor,
                textColor = Color.white
            };

            GUIStyleState hoverState = new()
            {
                background = hoverColor,
                textColor = Color.white
            };

            CommandButtonStyle = new GUIStyle("Command")
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fixedWidth = 45,
                fixedHeight = 18,
                onNormal = normalState,
                normal = normalState,
                onHover = hoverState,
                hover = hoverState
            };

            CommandButtonStyle2 = new GUIStyle("Command")
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fixedWidth = 60,
                fixedHeight = 18,
                onNormal = normalState,
                normal = normalState,
                onHover = hoverState,
                hover = hoverState
            };
        }

        [InitializeOnLoad]
        public class SceneSwitchLeftButton
        {
            static SceneSwitchLeftButton()
            {
                ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
            }

            static void OnToolbarGUI()
            {
                GUILayout.FlexibleSpace();

                if (Application.isPlaying)
                {
                    if (GUILayout.Button(new GUIContent("REBOOT", "Stop and restart game from the Boot scene"), ToolbarStyles.CommandButtonStyle2))
                    {
                        EditorPrefs.SetBool("rebootBootScene", true);
                        EditorApplication.isPlaying = false;
                    }
                }
                else
                {
                    if (GUILayout.Button(new GUIContent("BOOT", "Start game from the Boot scene"), ToolbarStyles.CommandButtonStyle))
                    {
                        SceneHelper.StartScene("System");
                    }
                }
            }
        }

        private static Texture2D MakeTexture(Color col)
        {
            Texture2D texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            texture.SetPixel(0, 0, col);
            texture.Apply();
            return texture;
        }

        private static class SceneHelper
        {
            private static string _sceneToOpen;

            public static void StartScene(string sceneName)
            {
                if (EditorApplication.isPlaying)
                {
                    EditorApplication.isPlaying = false;
                }

                EditorSceneManager.sceneClosed += OnSceneClosed;

                EditorApplication.playModeStateChanged += PlayModeStateChanged;
                _sceneToOpen = sceneName;
                EditorApplication.update += OnUpdate;
            }

            private static void OnSceneClosed(Scene scene)
            {
                EditorPrefs.SetString("sceneToSave", scene.name);
                EditorSceneManager.sceneClosed -= OnSceneClosed;
            }

            private static void PlayModeStateChanged(PlayModeStateChange action)
            {
                if (action != PlayModeStateChange.EnteredEditMode) return;
                if (EditorPrefs.GetBool("rebootBootScene", false))
                {
                    EditorPrefs.SetBool("rebootBootScene", false);
                    StartScene("System");
                    return;
                }

                if (EditorPrefs.GetString("sceneToSave") == "null") return;
                string[] guids = AssetDatabase.FindAssets("t:scene " + EditorPrefs.GetString("sceneToSave"), null);

                if (guids.Length == 0)
                {
                    string[] boot = AssetDatabase.FindAssets("t:scene System");
                    EditorSceneManager.OpenScene(AssetDatabase.GUIDToAssetPath(boot[0]));
                    return;
                }

                string scenePath = "";
                foreach (string gui in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(gui);
                    if (!path.Contains($"{EditorPrefs.GetString("sceneToSave")}.")) continue;
                    scenePath = path;
                    break;
                }

                if (scenePath == "")
                {
                    string[] boot = AssetDatabase.FindAssets("t:scene System");
                    EditorSceneManager.OpenScene(AssetDatabase.GUIDToAssetPath(boot[0]));
                    return;
                }

                EditorSceneManager.OpenScene(scenePath);
                EditorPrefs.SetString("sceneToSave", "null");
            }

            private static void OnUpdate()
            {
                if (_sceneToOpen == null ||
                    EditorApplication.isPlaying || EditorApplication.isPaused ||
                    EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
                {
                    return;
                }

                EditorApplication.update -= OnUpdate;

                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    string[] guids = AssetDatabase.FindAssets("t:scene " + _sceneToOpen, null);
                    if (guids.Length == 0)
                    {
                        Debug.LogWarning("Couldn't find scene file");
                    }
                    else
                    {
                        string scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);
                        EditorSceneManager.OpenScene(scenePath);
                        EditorApplication.isPlaying = true;
                    }
                }
                _sceneToOpen = null;
            }
        }
    }
}
