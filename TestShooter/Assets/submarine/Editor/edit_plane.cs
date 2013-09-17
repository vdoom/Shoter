using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Plane))]
public class edit_plane : edit_plane_base
{
    private Texture m_prevTexture = null;
    private bool m_prevTextureReaded = false;

    [MenuItem("SubEngine/Create Component/Plane")]
    static void CreateSprite()
    {
        //ClearConsole();
        GameObject parent = Selection.activeGameObject;
        GameObject planeObject = new GameObject("Plane");

        Plane plane = planeObject.AddComponent<Plane>();
        plane.Init(Shader.Find("submarine/Planes/Normal"), null);
        plane.transform.position = new Vector3(800 / 2, 480 / 2, -100);

        if (parent != null)
        {
            plane.transform.parent = parent.transform;
        }

        Selection.activeGameObject = planeObject;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Plane plane = (Plane)target;
        if (!m_prevTextureReaded && plane.material != null)
        {
            m_prevTexture = plane.material.mainTexture;
            m_prevTextureReaded = true;
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Clone"))
        {
            Selection.activeGameObject = plane.Clone().gameObject;
        }
        if (GUILayout.Button("Reinit renderer"))
        {
            Renderer rdr = plane.gameObject.GetComponent<MeshRenderer>();
            Texture texture = null;
            if (rdr == null)
            {
                rdr = plane.gameObject.AddComponent<MeshRenderer>();
            }
            else if (rdr.sharedMaterial)
            {
                texture = rdr.sharedMaterial.mainTexture;
            }

            rdr.sharedMaterial = new Material(Shader.Find("submarine/Planes/Normal"));
            rdr.sharedMaterial.color = Color.white;
            rdr.sharedMaterial.mainTexture = texture;
        }

       // EditorGUILayout.EndHorizontal();


        plane.ReinitMesh();

        if (plane.texture != null)
        {
            plane.ResizeUV();
        }

       // base.OnInspectorGUI();

        if (plane.material != null)
        {
            Texture newtexture = plane.material.mainTexture;

            if (m_prevTexture == null && newtexture != m_prevTexture)
            {
                ChangePivot(new Vector2(newtexture.width / 2.0f, newtexture.height / 2.0f));

                Vector3 spPos = plane.transform.localPosition;
                plane.transform.localPosition = new Vector3(spPos.x - (plane.width / 2), spPos.y - (plane.height / 2), spPos.z);
                plane.ReinitMesh();
            }
            else if (newtexture != m_prevTexture)
            {
                if (newtexture.width != m_prevTexture.width || newtexture.height != m_prevTexture.height)
                {
                    plane.width = 0;
                    plane.height = 0;
                }

            }

            m_prevTexture = newtexture;
        }

    }

}
