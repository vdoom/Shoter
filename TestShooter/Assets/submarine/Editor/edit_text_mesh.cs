using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TextMesh))]
public class edit_text_mesh : Editor
{
    [MenuItem("SubEngine/Create Component/Text")]
    static void CreateSprite()
    {
        GameObject parent = Selection.activeGameObject;
        GameObject textObject = new GameObject("text");
        textObject.gameObject.AddComponent<MeshRenderer>();
        TextMesh text = textObject.AddComponent<TextMesh>();
        text.transform.position = new Vector3(800 / 2, 480 / 2, -100);

        if (parent != null)
        {
            text.transform.parent = parent.transform;
        }

        Selection.activeGameObject = textObject;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        TextMesh textMesh = target as TextMesh;

        Color preservedColor = Color.white;
        if (textMesh.renderer.sharedMaterial != null)
        {
            preservedColor = textMesh.renderer.sharedMaterial.color;
        }

        if (GUILayout.Button("Create Font Material"))
        {
            textMesh.renderer.sharedMaterial = textMesh.font.material;
            UnityEditorInternal.InternalEditorUtility.InstantiateMaterialsInEditMode(textMesh.renderer);
            textMesh.renderer.sharedMaterial.color = preservedColor;
        }
    }
}