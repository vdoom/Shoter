using UnityEngine;
using System.Collections;
using UnityEditor;

public class edit_plane_base : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlaneBase plane = (PlaneBase)target;

        plane.visible = EditorGUILayout.Toggle("Visible", plane.visible);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Pivot to Center"))
        {
            ChangePivot(new Vector2(plane.width / 2, plane.height / 2));
        }
        if (GUILayout.Button("Hot Spot to Zero"))
        {
            ChangePivot(Vector2.zero);
        }

        EditorGUILayout.EndHorizontal();
    }

    public virtual void OnSceneGUI()
    {
        PlaneBase plane = (PlaneBase)target;

        if (Event.current.control)
        {
            Vector3 handlePos = plane.transform.TransformPoint(Vector3.zero);
            Vector3 newHandlePos = Handles.FreeMoveHandle(handlePos, Quaternion.identity,
                                                          HandleUtility.GetHandleSize(Vector3.zero) * 0.75f,
                                                          new Vector3(1.0f, 1.0f, 1.0f), Handles.SphereCap);

            Vector2 offset = plane.transform.InverseTransformPoint(newHandlePos);
            plane.pivot += offset;
            plane.transform.position += newHandlePos - handlePos;
            plane.ReinitMesh();
        }
    }

    public void ChangePivot(Vector2 vec)
    {
        PlaneBase plane = (PlaneBase)target;

        Vector2 oldPivot = plane.pivot;
        plane.pivot = vec;
        plane.transform.position += plane.transform.TransformPoint(plane.pivot) - plane.transform.TransformPoint(oldPivot);
    }
}
