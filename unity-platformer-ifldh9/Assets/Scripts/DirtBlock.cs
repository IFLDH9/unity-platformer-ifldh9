using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class DirtBlock : Block
{
    private void Awake()
    {
        breakable = true;       
    }


#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a RoadTile Asset
    [MenuItem("Assets/Create/DirtBlock")]
    public static void CreateDirtBlock()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save DirtBlock", "New Dirt Black", "Asset", "Save Dirt Block", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<DirtBlock>(), path);
    }
#endif
}

