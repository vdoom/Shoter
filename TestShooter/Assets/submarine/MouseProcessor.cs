using UnityEngine;
using System.Collections;

public class MouseProcessor : GameObjectEx
{
	public System.Action<MouseProcessor> OnMousePress = null;

	public Vector2 tmp = Vector2.zero;

	private RaycastHit m_hit;

	private bool m_hasMouse = false;

    public Vector3 mousePos
    {
        get
        {
            if (m_hasMouse)
            {
                return m_hit.point;
            }
            else
            {
                return new Vector3(0, 0, 0);
            }
        }
    }

    public override void Start()
	{
        base.Start();
	}

    public override void Update()
	{
        base.Update();

        if (SInput.GetMouseDown(0))
        {
            tmp = SInput.mousePosition;
        }
        else if(SInput.GetMousePress(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(SInput.mousePosition);
            if (collider != null)
            {
                if (collider.Raycast(ray, out m_hit, 100000.0f))
                {
                    m_hasMouse = true;
                    if (OnMousePress != null)
                    {
                        OnMousePress(this);
                    }
                }
                else
                {
                    m_hasMouse = false;
                }
            }
        }
	}

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
    
}
