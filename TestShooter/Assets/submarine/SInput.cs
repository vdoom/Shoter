#if (UNITY_IOS || UNITY_ANDROID) //|| UNITY_EDITOR
#   define MOBILE_PLATFORM
#endif

using UnityEngine;
using System.Collections;

public static class SInput
{
#if MOBILE_PLATFORM
    static Vector3 s_mousePosition = Vector3.zero;
#endif
    public static Vector3 s_mouseScale
    {
        get
        {
            return new Vector3((1.0f * 800) / UnityEngine.Screen.width,
                                   (1.0f * 480) / UnityEngine.Screen.height,
                                   1);
        }
    }
    public static bool GetTapDown()
    {
        if (UnityEngine.Input.touchCount > 0)
        {
            if (UnityEngine.Input.touches[0].phase == TouchPhase.Began)
            {
                return true;
            }
        }
        return false;
    }
    public static bool GetTapPress()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Moved ||
                Input.touches[0].phase == TouchPhase.Stationary)
            {
                return true;
            }
        }
        return false;
    }
    public static bool GetTapUp()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Canceled ||
                Input.touches[0].phase == TouchPhase.Ended)
            {
                return true;
            }
        }
        return false;
    }
    public static bool GetMouseDown(int state)
    {
#if MOBILE_PLATFORM
        return GetTapDown();
#else
        return Input.GetMouseButtonDown(state);
#endif
    }
    public static bool GetMousePress(int state)
    {
#if MOBILE_PLATFORM
        return GetTapPress();
#else
        return Input.GetMouseButton(state);
#endif
    }
    public static bool GetMouseUp(int state)
    {
#if MOBILE_PLATFORM
        return GetTapUp();
#else
        return Input.GetMouseButtonUp(state);
#endif
    }

    public static Vector3 mousePosition
    {
        get
        {
#if MOBILE_PLATFORM
            if (UnityEngine.Input.touchCount > 0)
            {
                s_mousePosition = UnityEngine.Input.touches[0].position;
                s_mousePosition.Scale(s_mouseScale);
            }
            return s_mousePosition;
#else
            Vector3 tmp = UnityEngine.Input.mousePosition;
            tmp.Scale(s_mouseScale);
            return tmp;
#endif
        }
    }
}
