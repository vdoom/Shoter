using UnityEngine;
using System.Collections;

public class Wheel : MouseProcessor
{
    private bool m_isDrag = false;
    private Vector3 m_center = Vector3.zero;

    Vector2 dragPos;
    Vector2 axis;
    float oldAngle = 0;
    float ttAngle = 0;
    float inertAngle = 0;
    float ang = 0;
    float Old = 0;
    //bool isDeadCenter = false;
    private Vector3 m_startAngle = Vector3.zero;

    public override void Start()
    {
        m_center = transform.position;//collider.bounds.center;
        m_startAngle = transform.eulerAngles;
    }

    public override void Update()
    {
        base.Update();
        if (SInput.GetMouseDown(0) && !m_isDrag && WheelCast(SInput.mousePosition))
        {
            //Debug.Log(SInput.mousePosition+ " "+ transform.position);
            dragPos = m_center;
            axis = new Vector2(SInput.mousePosition.x, SInput.mousePosition.y) - dragPos;
            oldAngle = Vector2.Angle(axis, new Vector2(0, 1));

            if (SInput.mousePosition.x > dragPos.x)
            {
                oldAngle += Old;
            }
            else
            {
                oldAngle += (Old * (-1));
            }

            if (SInput.mousePosition.x > dragPos.x)
            {
                oldAngle *= -1;
            }

            if (oldAngle > 360)
            {
                oldAngle = oldAngle - 360;
            }
            m_isDrag = true;
            //Debug.Log(oldAngle + " " + Old);
        }
        //else if (!isDrag)
        //{
        //    isDrag = false;
        //}

        if (SInput.GetMousePress(0) /*Input.touchCount > 0//Input.GetMouseButton(0) */ && m_isDrag)
        {
            Vector3 mousepose = SInput.mousePosition;//BACK.mousePos;
            // Debug.Log(mousepose+ " "+ SInput.mousePosition);
            dragPos = m_center;
            axis = new Vector2(mousepose.x, mousepose.y) - dragPos;//Vector2(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y) - dragPos;
            ang = Vector2.Angle(axis, new Vector2(0, 1));
            //guiText.text = "axis:" + axis.ToString();
            //GEAR.transform.position = mousepose;

            if (mousepose.x > dragPos.x)//if (UnityEngine.Input.mousePosition.x > dragPos.x)
            {
                ang *= -1;
            }

            //guiText.text += " angle:" + ang;
            //Debug.Log(ang+ " " + oldAngle);
            ang = ang - oldAngle;
            inertAngle = (ang - ttAngle);
           // guiText.text += " OLDangle:" + ang;
            // Debug.Log(ang);
            if (Old != ang)
            {
                Old = ang;
                transform.localRotation = Quaternion.AngleAxis(m_startAngle.z, Vector3.forward) * Quaternion.AngleAxis(ang, Vector3.forward); //Quaternion.AngleAxis(90, Vector3.left) * Quaternion.AngleAxis(ang, Vector3.down);
            }
            ttAngle = ang;
        }
        else if (SInput.GetMouseUp(0))
        {
            m_isDrag = false;

        }
        if (!m_isDrag)
        {
            transform.localRotation *= Quaternion.AngleAxis(inertAngle, Vector3.forward);//Quaternion.AngleAxis(inertAngle, Vector3.down);
            Old += inertAngle;
            if (inertAngle > 20f && inertAngle > 0)
            {
                inertAngle = 20f;
            }
            else if (inertAngle < -20f && inertAngle < 0)
            {
                inertAngle = -20f;
            }
            inertAngle -= inertAngle / 10;
        }

    }

    bool WheelCast(Vector3 mPos)//(Sprite castSprite, float alphaFactor = 0.1f)
	{
		Collider mcollider = collider;
        if (mcollider != null)
        {
            RaycastHit hit;
            Ray ray = new Ray(new Vector3(mPos.x, mPos.y, -10000), new Vector3(0, 0, 1));

            return (mcollider.Raycast(ray, out hit, 11000.0f));
        }
        else return false;
	}
}
