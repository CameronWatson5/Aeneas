using UnityEngine;
using UnityEditor;

public class TilemapMover : EditorWindow
{
    private float pixelsPerUnit = 16f; 

    [MenuItem("Window/Tilemap Mover")]
    public static void ShowWindow()
    {
        GetWindow<TilemapMover>("Tilemap Mover");
    }

    void OnGUI()
    {
        pixelsPerUnit = EditorGUILayout.FloatField("Pixels Per Unit", pixelsPerUnit);

        if (GUILayout.Button("Move Left (1 Pixel)"))
        {
            MoveSelectedObjects(Vector3.left / pixelsPerUnit);
        }

        if (GUILayout.Button("Move Right (1 Pixel)"))
        {
            MoveSelectedObjects(Vector3.right / pixelsPerUnit);
        }

        if (GUILayout.Button("Move Up (1 Pixel)"))
        {
            MoveSelectedObjects(Vector3.up / pixelsPerUnit);
        }

        if (GUILayout.Button("Move Down (1 Pixel)"))
        {
            MoveSelectedObjects(Vector3.down / pixelsPerUnit);
        }
    }

    void MoveSelectedObjects(Vector3 direction)
    {
        foreach (var obj in Selection.gameObjects)
        {
            Undo.RecordObject(obj.transform, "Move Tilemap");
            obj.transform.position += direction;
            EditorUtility.SetDirty(obj);
        }
    }
}