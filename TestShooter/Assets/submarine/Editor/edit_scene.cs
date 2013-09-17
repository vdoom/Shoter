using UnityEngine;
using UnityEditor;
using System.Collections;

public class edit_scene : Editor
{
    [MenuItem("SubEngine/Scene/Create LevelScene")]
    public static void Create()
    {
        if (EditorApplication.SaveCurrentSceneIfUserWantsTo())
        {
            EditorApplication.NewScene();

            UnityEngine.Object cam = GameObject.Find("Main Camera");
            if (cam)
            {
                UnityEngine.Object.DestroyImmediate(cam);
            }

            GameObject sceneRoot = new GameObject("level");
            sceneRoot.transform.position = new Vector3(800 / 2, 480 / 2, 0);

            GameObject background = new GameObject("background");
            Plane bgPlane = background.AddComponent<Plane>();
            bgPlane.Init(null, null);

            background.transform.parent = sceneRoot.transform;
            background.transform.position = new Vector3(800 / 2, 480 / 2, 0);
        }
    }

}
