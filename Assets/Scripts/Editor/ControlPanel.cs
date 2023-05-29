using UnityEditor;
using Sirenix.OdinInspector.Editor;

public class ControlPanel : OdinMenuEditorWindow {
    
    [MenuItem("Tools/Control Panel")]
    private static void OpenWindow() {
        GetWindow<ControlPanel>().Show();
    }
    
    // Build the menu tree
    protected override OdinMenuTree BuildMenuTree() {
        // Create a new tree
        OdinMenuTree tree = new() {
            {"DATA", new OdinMenuTree()},
            {"AUDIO", new OdinMenuTree()}
        };
        // Add all assets in the "ScriptableObjects" folder to the "DATA" menu
        tree.AddAllAssetsAtPath("DATA", "Assets/ScriptableObjects", true);
        // Add all assets in the "Audio" folder to the "AUDIO" menu
        tree.AddAllAssetsAtPath("AUDIO/AUDIOEVENTS", "Assets/Audio/AudioEvents", false);
        tree.SortMenuItemsByName();
        
                
        
        return tree;
    }
}

