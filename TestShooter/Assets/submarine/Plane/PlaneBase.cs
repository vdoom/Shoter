using UnityEngine;
using System.Collections;

public class PlaneBase : MouseProcessor
{
    public float m_width;
    public float m_height;
    public Vector2 m_pivot;
    public bool m_omitColliderAutoCreation = false;

    public virtual void Init(Shader shader)
    {
        Renderer rdr = gameObject.GetComponent<MeshRenderer>();
        if (rdr == null)
        {
            rdr = gameObject.AddComponent<MeshRenderer>();
            rdr.castShadows = false;
            rdr.receiveShadows = false;
        }

        if (shader != null)
        {
            if (rdr.sharedMaterial == null)
            {
                rdr.sharedMaterial = new Material(shader);
                rdr.sharedMaterial.color = Color.white;
            }
            else
            {
                rdr.sharedMaterial.shader = shader;
            }
        }

    }
    public Material material
    {
        get { return renderer != null ? renderer.sharedMaterial : null; }
    }
    public Shader shader
    {
        get { return material.shader; }
        set
        {
            if (material.shader == value || value == null)
            {
                return;
            }
            material.shader = value;
        }
    }
    public Color color
    {
        get { return material.color; }
        set { material.color = value; }
    }
    public float width
    {
        get { return m_width; }
        set { m_width = value; }
    }
    public float height
    {
        get { return m_height; }
        set { m_height = value; }
    }
    public Vector2 size
    {
        get { return new Vector2(m_width, m_height); }
        set { m_width = value.x; m_height = value.y; }
    }
    public Vector2 pivot
    {
        get { return m_pivot; }
        set { m_pivot = value; }
    }
    public bool omitColliderAutoCreation
    {
        set { m_omitColliderAutoCreation = value; }
    }
    public virtual void ReinitMesh()
    {
        MeshFilter filter = GetComponent<MeshFilter>();
        if (filter == null)
        {
            filter = gameObject.AddComponent<MeshFilter>();
        }

        if (filter.sharedMesh == null)
        {
            filter.sharedMesh = new Mesh();
            filter.sharedMesh.name = name + "Mesh";
        }

        filter.sharedMesh.Clear();
        filter.sharedMesh.subMeshCount = 1;

        float width = m_width * 0.5f;
        float height = m_height * 0.5f;

        filter.sharedMesh.vertices = new Vector3[]
		{
			new Vector3(0 - m_pivot.x,	0 - m_pivot.y,	0.0f),
			new Vector3(0 - m_pivot.x,	height * 2.0f - m_pivot.y,	0.0f),
			new Vector3( width * 2.0f - m_pivot.x,	height * 2.0f - m_pivot.y,	0.0f),
			new Vector3( width * 2.0f - m_pivot.x,	0 - m_pivot.y, 0.0f)
		};

        filter.sharedMesh.triangles = new int[]
		{
			0,1,2,2,3,0
		};

        filter.sharedMesh.RecalculateBounds();
        Bounds b = filter.sharedMesh.bounds;
        b.center = Vector3.zero;
        filter.sharedMesh.bounds = b;

        if (!m_omitColliderAutoCreation)
        {
            Collider collider = gameObject.GetComponent<Collider>();
            if (collider == null)
            {
                MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = filter.sharedMesh;
            }
            else
            {
                MeshCollider meshCollider = collider as MeshCollider;
                if (meshCollider)
                {
                    meshCollider.sharedMesh = null;
                    meshCollider.sharedMesh = filter.sharedMesh;
                }
            }
        }
    }
    public void ChangePivot(Vector3 newPivot)
    {
        Vector2 oldHotSpot = pivot;
        pivot = newPivot;
        transform.position += transform.TransformPoint(pivot) - transform.TransformPoint(oldHotSpot);

        ReinitMesh();
    }
    public void PivotToCenter()
    {
        ChangePivot(new Vector2(width / 2, height / 2));
    }
}
