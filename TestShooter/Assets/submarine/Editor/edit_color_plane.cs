using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ColorPlane))]
public class edit_color_plane : edit_plane_base
{

    [MenuItem("SubEngine/Create Component/Color Plane")]
    static void CreateColorPlane()
    {
        GameObject parent = Selection.activeGameObject;
        GameObject planeObject = new GameObject("ColorPlane");
        ColorPlane plane = planeObject.AddComponent<ColorPlane>();
        plane.Init(new Vector2(100, 100));
        plane.transform.position = new Vector3(800 / 2, 480 / 2, -100);

        if (parent != null)
        {
            plane.transform.parent = parent.transform;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ColorPlane plane = (ColorPlane)target;

        plane.ReinitMesh();
    }

}