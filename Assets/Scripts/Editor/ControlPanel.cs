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
        };
        // Add all assets in the "ScriptableObjects" folder to the "DATA" menu
        tree.AddAllAssetsAtPath("DATA", "Assets/ScriptableObjects", true);
        tree.SortMenuItemsByName();
        
                
        
        return tree;
    }
}

