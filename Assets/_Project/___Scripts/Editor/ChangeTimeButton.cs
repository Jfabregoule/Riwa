#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;


[InitializeOnLoad]
public static class ChangeTimeButtonEditor
{
    private static GUIStyle ButtonStyle;
    
    private static int _present = 1;
    private static int _past = 0;

    static ChangeTimeButtonEditor()
    {
        Texture2D normalColor = MakeTexture(new Color(0.2196079f, 0.2196079f, 0.2196079f, 1f));
        Texture2D hoverColor = MakeTexture(new Color(0.2352941f, 0.2352941f, 0.2352941f, 1f));

        GUIStyleState normalState = new GUIStyleState() { background = normalColor, textColor = Color.white };
        GUIStyleState hoverState = new GUIStyleState() { background = hoverColor, textColor = Color.white };

        ButtonStyle = new GUIStyle("Command")
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleCenter,
            imagePosition = ImagePosition.ImageAbove,
            fixedWidth = 80,
            fixedHeight = 18,
            normal = normalState,
            hover = hoverState,
            onNormal = normalState,
            onHover = hoverState
        };
        
        UnityToolbarExtender.ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
    }

    private static void OnToolbarGUI()
    {
        if (ButtonStyle == null)
        {
            Texture2D normalColor = MakeTexture(new Color(0.2196079f, 0.2196079f, 0.2196079f, 1f));
            Texture2D hoverColor = MakeTexture(new Color(0.2352941f, 0.2352941f, 0.2352941f, 1f));

            GUIStyleState normalState = new GUIStyleState() { background = normalColor, textColor = Color.white };
            GUIStyleState hoverState = new GUIStyleState() { background = hoverColor, textColor = Color.white };

            ButtonStyle = new GUIStyle("Command")
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fixedWidth = 80,
                fixedHeight = 18,
                normal = normalState,
                hover = hoverState,
                onNormal = normalState,
                onHover = hoverState
            };
        }

        if (!GUILayout.Button(new GUIContent("SWITCH", "Switch between Past and Present"), ButtonStyle)) return;

        _present = 1 - _present;
        _past = 1 - _past;

        Shader.SetGlobalInt("_PresentEnum", _present);
        Shader.SetGlobalInt("_PastEnum", _past);
        Shader.SetGlobalFloat("_Radius", 0f);

        Debug.Log($"Time changed in editor mode. Present: {_present}, Past: {_past}");
    }

    private static Texture2D MakeTexture(Color col)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, col);
        texture.Apply();
        return texture;
    }
}

#endif
