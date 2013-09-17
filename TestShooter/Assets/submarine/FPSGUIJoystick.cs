using UnityEngine;
using System.Collections;

public class FPSGUIJoystick : MonoBehaviour
{
    [SerializeField] FPSJoystick m_joystick = null;
    [SerializeField] GUITexture m_texture = null;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_joystick.startTouchPosLeft != Vector2.zero)
        {
            Rect tmp = m_texture.pixelInset;
            tmp.x = m_joystick.startTouchPosLeft.x;
            tmp.y = m_joystick.startTouchPosLeft.y;
            m_texture.pixelInset = tmp;
        }
    }
}
