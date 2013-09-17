using UnityEngine;
using System.Collections;

public class ColorPlane : PlaneBase
{
    public void Init(Vector2 size)
    {
        Shader shader = Shader.Find("submarine/ColorPlane/Alpha");
        if (shader == null)
        {
            shader = Shader.Find("GUI/Text Shader");
        }

        base.Init(shader);

        m_width = size.x;
        m_height = size.y;
        ReinitMesh();
    }

    static public ColorPlane Create(Vector2 size)
    {
        return Create("ColorPlane", size);
    }

    static public ColorPlane Create(string name, Vector2 size)
    {
        GameObject planeObject = new GameObject(name);
        ColorPlane plane = planeObject.AddComponent<ColorPlane>();
        plane.pivot = size / 2;
        plane.Init(size);
        plane.transform.position = new Vector3(800 / 2, 480 / 2, -100);

        return plane;
    }
}
