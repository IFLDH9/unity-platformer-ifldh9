using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Block : RuleTile
{
   public bool breakable;
   public Animator animation;


#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a RoadTile Asset
    [MenuItem("Assets/Create/Block")]
    public static void CreateRoadTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Block", "New Block", "Asset", "Save Block", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<Block>(), path);
        
    }
#endif

}
