using UnityEngine;
using UnityEditor;
using CHS.Input;

[InitializeOnLoad]
public static class PlayModeTracker
{
    static PlayModeTracker()
    {
        // Subscribe to the playmodeStateChanged event
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        switch (state)
        {
            case PlayModeStateChange.EnteredEditMode:
                Debug.Log("Exited Play Mode");
                // Add your code to handle exiting play mode
                break;
            case PlayModeStateChange.ExitingEditMode:
                Debug.Log("Entered Play Mode");
                // Add your code to handle entering play mode
                break;
        }
    }
}
