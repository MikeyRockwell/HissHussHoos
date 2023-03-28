using UnityEditor;
using Sirenix.OdinInspector.Editor;

public class ControlPanel : OdinMenuEditorWindow {
    
    [MenuItem("Tools/Control Panel")]
    private static void OpenWindow() {
        GetWindow<ControlPanel>().Show();
    }

    protected override OdinMenuTree BuildMenuTree() {
        
        OdinMenuTree tree = new() {
            {"DATA", new OdinMenuTree()},
        };

        tree.AddAllAssetsAtPath("DATA", "Assets/ScriptableObjects", true);
        tree.SortMenuItemsByName();

        return tree;
    }
}

