using UnityEngine;
using System.Collections;

public class Plane : PlaneBase
{
    //TODO: NEED REFINE SHOW & HIDE,  FADEOUT-IN 


    public static Plane Create(string name, Shader shader, Texture texture)
    {
        GameObject planeObject = new GameObject(name);
        Plane retPlane = planeObject.AddComponent<Plane>();
        retPlane.Init(shader, texture);
        return retPlane;
    }

    public static Plane Create(string name, Shader shader, Texture texture, Vector2 iSize)
    {
        return Create(name, shader, texture, iSize / 2, iSize);
    }

    public static Plane Create(string name, Shader shader, Texture texture, Vector2 iPivot, Vector2 iSize)
    {
        GameObject planeObject = new GameObject(name);
        Plane retPlane = planeObject.AddComponent<Plane>();
        retPlane.pivot = iPivot;
        retPlane.width = iSize.x;
        retPlane.height = iSize.y;
        retPlane.Init(shader, texture);
        return retPlane;
    }

    public void Init(Shader shader, Texture texture)
    {
        if (shader == null)
        {
            shader = Shader.Find("submarine/Planes/Normal");
        }
        if (shader == null)
        {
            shader = Shader.Find("Unlit/Texture");
        }

        base.Init(shader);
        Renderer rdr = gameObject.GetComponent<MeshRenderer>();

        if (rdr.sharedMaterial != null)
        {
            if (texture != null)
            {
                rdr.sharedMaterial.mainTexture = texture;
                ReinitMesh();
            }
            //todo: need default texture
            //else
            //{
            //}
        }
    }
    //todo: need create clone method

    public Plane Clone()
    {
        GameObject clone = new GameObject(name + "(clone)");
        clone.transform.parent = transform.parent;
        clone.transform.localPosition = transform.localPosition;
        clone.transform.localScale = transform.localScale;
        clone.transform.localRotation = transform.localRotation;

        Plane plane = clone.AddComponent<Plane>();
        plane.Init(material.shader, texture);
        plane.material.color = material.color;

        plane.width = width;
        plane.height = height;
        plane.pivot = pivot;
        plane.gameObject.layer = gameObject.layer;
        plane.opacity = opacity;
        plane.ReinitMesh();

        return plane;
    }

    public override void Start()
    {
    }

    public Texture texture
    {
        get
        {
            return material != null ? material.mainTexture : null;
        }
        set
        {
            if (material.mainTexture == value || value == null)
            {
                return;
            }

            material.mainTexture = value;
            ReinitMesh();
        }
    }

 
    //--------------------------------------------------------------------------
    public override void ReinitMesh()
    {
        if (texture == null)
        {
            return;
        }

        if (m_width == 0)
        {
            m_width = texture.width;
        }

        if (m_height == 0)
        {
            m_height = texture.height;
        }

        base.ReinitMesh();

        ResizeUV();
    }

    //--------------------------------------------------------------------------
    public void ResizeUV()
    {
        MeshFilter filter = GetComponent<MeshFilter>();

        filter.sharedMesh.uv = new Vector2[]
		{
			new Vector2(0, 0),
			new Vector2(0, 1.0f),
			new Vector2(1, 1.0f),
			new Vector2(1, 0)
		};
    }
}
