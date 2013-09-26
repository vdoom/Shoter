using UnityEngine;
using System.Collections;

public class GUIButton : MonoBehaviour
{
    [SerializeField] Texture2D m_maineTexture = null;
    [SerializeField] Vector2 m_pos = Vector2.zero;
    [SerializeField] Vector2 m_size = Vector2.zero;
    
    private Touch m_touch;
    private bool m_isTouched = false;
    private Rect m_rect;

    public bool visible = false;

    public System.Action OnStartPress = null; 
    public System.Action OnPress = null;
    public System.Action OnRelease = null;
    public System.Action OnClick = null;

    void Start()
    {
        m_rect = new Rect(Screen.width * m_pos.x - (m_size.x / 2), Screen.height * m_pos.y - (m_size.y / 2), m_size.x, m_size.y);
		Debug.Log("xMax: "+m_rect.xMax+"xMin: "+ m_rect.xMin+ "yMax" + m_rect.yMax+ "yMin"+ m_rect.yMin);
    }

    void OnGUI()
    {
        if (visible)
        {
            GUI.DrawTexture(m_rect, m_maineTexture);
        }
    }

    void Update()
    {
        if (visible)
        {
            bool finded = false;
            foreach (Touch touch in Input.touches)
            {
                if (m_isTouched && touch.fingerId == m_touch.fingerId) finded = true;

                if (!m_isTouched && touch.phase == TouchPhase.Began)
                {
                    if(touch.position.x > m_rect.xMin &&
						touch.position.x < m_rect.xMax &&
						touch.position.y < Screen.height-m_rect.yMin && 
						touch.position.y > Screen.height-m_rect.yMax)
                    {
						Debug.Log("Touched");
                        m_touch = touch;
                        m_isTouched = true;
                        if(OnStartPress != null) OnStartPress();
                    }
                }
                else if (m_isTouched)
                {
                    if (touch.fingerId == m_touch.fingerId)
                    {
                        m_touch = touch;
						 if(touch.position.x > m_rect.xMin &&
							touch.position.x < m_rect.xMax &&
							touch.position.y < Screen.height-m_rect.yMin && 
							touch.position.y > Screen.height-m_rect.yMax)
                        {
                            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                            {
                                if(OnPress != null) OnPress();
                            }
                            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                            {
                                if(OnClick != null) OnClick();
                                if(OnRelease != null) OnRelease();
                                m_isTouched = false;
                            }
                        }
                    }
                }
            }
            if (!finded && m_isTouched)
            {
                m_isTouched = false;
                if(OnRelease != null) OnRelease();
            }
        }
    }
}
