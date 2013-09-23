using UnityEngine;
using System.Collections;

public class FPSJoystick : MonoBehaviour
{
    [SerializeField] Texture2D m_leftTextureStatic = null;
    [SerializeField] Texture2D m_leftTextureActive = null;

    private Touch m_leftTouch;// = null;
    private Touch m_rightTouch;// = null;
    private bool m_blockLeftSide = false;
    private bool m_blockRightSide = false;
    private bool m_takedLeftTouch = false;
    private bool m_takedRightTouch = false;
    private Vector2 m_leftDiffVector = Vector2.zero;
    private Vector2 m_rightDiffVector = Vector2.zero;
    private int m_leftFingerId = -1;
    private int m_rightFingerId = -1;

    public Vector2 startTouchPosLeft
    {
        get;
        private set;
    }
    public Vector2 startTouchPosRight
    {
        get;
        private set;
    }
    public Vector2 currTouchPosLeft
    {
        get;
        private set;
    }
    public Vector2 currTouchPosRight
    {
        get;
        private set;
    }
    public Vector2 leftVector
    {
        get
        { 
            return currTouchPosLeft - startTouchPosLeft; 
        }
    }
    public Vector2 leftDiffVector
    { 
        get
        {
            return m_leftDiffVector;
            //if (m_takedLeftTouch)
            //{
            //    return m_leftTouch.deltaPosition;
            //}
            //else return Vector2.zero;
        } 
    }
    public Vector2 rightVector
    {
        get
        {
            return currTouchPosRight - startTouchPosRight;
        }
    }
    public Vector2 rightDiffVector
    {
        get
        {
            return m_rightDiffVector;
            //if (m_takedRightTouch)
            //{
            //    return m_rightTouch.deltaPosition;
            //}
            //else return Vector2.zero;
        }
    }

    public bool blockLeftSide
    {
        get { return m_blockLeftSide; }
        set
        {
            if (value)
            {
                startTouchPosLeft = new Vector2(0,0);
                currTouchPosLeft = new Vector2(0,0);
                m_takedLeftTouch = false;
            }
            m_blockLeftSide = value;
        }
    }
    public bool blockRightSide
    {
        get { return m_blockRightSide; }
        set
        {
            if (value)
            {
                startTouchPosRight = new Vector2(0,0);
                currTouchPosRight = new Vector2(0,0);
                m_takedLeftTouch = false;
            }
            m_blockRightSide = value;
        }
    }
    
    bool hasLeft = false;
    bool hasRight = false;

    void Start()
    {
        startTouchPosLeft = new Vector2(0,0);
        startTouchPosRight = new Vector2(0,0);
        currTouchPosLeft = new Vector2(0,0);
        currTouchPosRight = new Vector2(0,0);
    }

    void DrawLeft(Vector2 t_pos)
    {
        Rect rect = new Rect(t_pos.x, t_pos.y, 100, 100);
        //GUI.DrawTexture(rect, m_leftTexture,ScaleMode.ScaleAndCrop, true);
    }

    void OnGUI()
    {
        if (startTouchPosLeft != Vector2.zero)
        {
            Rect rect1 = new Rect(startTouchPosLeft.x - 100, ((Screen.height / 2) - startTouchPosLeft.y) + 100, 200, 200);
            GUI.DrawTexture(rect1, m_leftTextureStatic, ScaleMode.ScaleAndCrop, true);
        }
        if (currTouchPosLeft != Vector2.zero)
        {
            Vector2 activePos = currTouchPosLeft;
            if ((activePos - startTouchPosLeft).magnitude > 75)
            {
               activePos = startTouchPosLeft + ((activePos - startTouchPosLeft).normalized * 75);
            }
            Rect rect2 = new Rect(activePos.x - 25, ((Screen.height / 2) - activePos.y) + 175, 50, 50);
            GUI.DrawTexture(rect2, m_leftTextureActive, ScaleMode.ScaleAndCrop, true);
        }

    }

    void Update()
    {
        hasRight = false;
        hasLeft = false;
        foreach(Touch touch in Input.touches)
        {
            if (!blockLeftSide && touch.position.x < (Screen.width / 2))
            {
                if (!m_takedLeftTouch && touch.phase == TouchPhase.Began)
                {
                    m_takedLeftTouch = true;
                    m_leftTouch = touch;
                    m_leftFingerId = touch.fingerId;
                    startTouchPosLeft = m_leftTouch.position;
                    currTouchPosLeft = m_leftTouch.position;
                    DrawLeft(currTouchPosLeft);
                }
                else if (m_takedLeftTouch && m_leftFingerId == touch.fingerId)
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        currTouchPosLeft = touch.position;
                        DrawLeft(currTouchPosLeft);
                        m_leftDiffVector = touch.deltaPosition;
                    }
                    else if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                    {
                        m_takedLeftTouch = false;
                        startTouchPosLeft = new Vector2(0,0);
                        currTouchPosLeft = new Vector2(0,0);
                        m_leftDiffVector = new Vector2(0, 0);
                        m_leftFingerId = -1;
                    }
                }
            }
            if (!blockRightSide && touch.position.x > (Screen.width / 2))
            {
                if (!m_takedRightTouch && touch.phase == TouchPhase.Began)
                {
                    m_takedRightTouch = true;
                    m_rightTouch = touch;
                    m_rightFingerId = touch.fingerId;
                    startTouchPosRight = m_rightTouch.position;
                    currTouchPosRight = m_rightTouch.position;
                }
                else if (m_takedRightTouch && m_rightFingerId == touch.fingerId)
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        m_rightDiffVector = touch.deltaPosition;
                        currTouchPosRight = touch.position;
                    }
                    else if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                    {
                        m_takedRightTouch = false;
                        startTouchPosRight = new Vector2(0, 0);
                        currTouchPosRight = new Vector2(0, 0);
                        m_rightDiffVector = new Vector2(0, 0);
                        m_rightFingerId = -1;
                    }
                }
            }
            if (touch.fingerId == m_leftFingerId)
            { hasLeft = true; }
            if (touch.fingerId == m_rightFingerId)
            { hasRight = true; }
        }
        if (!hasLeft || (currTouchPosLeft.x > (Screen.width / 2)))
        {
            hasLeft = false; 
            m_leftDiffVector = Vector2.zero;
            m_takedLeftTouch = false;
            startTouchPosLeft = new Vector2(0, 0);
            currTouchPosLeft = new Vector2(0, 0);
            m_leftDiffVector = new Vector2(0, 0);
            m_leftFingerId = -1;
        }

        if (!hasRight || (currTouchPosRight.x < (Screen.width / 2)))
        {
            hasRight = false;
            m_rightDiffVector = Vector2.zero;
            m_takedRightTouch = false;
            startTouchPosRight = new Vector2(0, 0);
            currTouchPosRight = new Vector2(0, 0);
            m_rightDiffVector = new Vector2(0, 0);
            m_rightFingerId = -1;
        }
    }
}
