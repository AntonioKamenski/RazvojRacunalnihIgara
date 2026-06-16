using UnityEditor;

[InitializeOnLoad]
public static class PlayModeSelectionFix
{
    static PlayModeSelectionFix()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode ||
            state == PlayModeStateChange.ExitingPlayMode)
        {
            Selection.activeObject = null;
        }
    }
}
