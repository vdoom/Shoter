using UnityEngine;
using System.Collections;

public class LevelScene : BaseScene
{
    protected MainScene m_maineScene = null;
    public GUIText myGUItext;

    public override void Update()
    {

    }

    public override void Start()
    {
        base.Start();
        InitByApp();
        CreateGUIText();
        //need chek for hide LoadingFadePlane
        //if()
    }

    public void CreateGUIText()
    {
        //GameObject textObject = new GameObject("Text");
        //myGUItext = gameObject.AddComponent<GUIText>();
        //myGUItext.text = "Ahoy!!!";
    }

    public void InitByApp()
    {
        if (GameObject.Find("main"))
        {
            m_maineScene = GameObject.Find("main").GetComponent<MainScene>();
            if (m_maineScene != null)
            {
                m_maineScene.RegisterLevel(this);
            }
        }
    }

    public override void OnDestroy()
    {
        if (m_maineScene != null)
        {
            m_maineScene.UnRegisterLevel(this);
        }
        base.OnDestroy();
    }

    public void LoadLevel(string t_levelName)
    {
        if(m_maineScene!=null)
        {
            m_maineScene.LoadLevel(t_levelName);
        }
    }

    public void LoadLevel(int t_index)
    {
        if (m_maineScene != null)
        {
            m_maineScene.LoadLevel(t_index);
        }
    }

    public virtual void OnGUI()
    {
        if (GUI.Button(new Rect(20, 40, 80, 20), "Test Button"))
        {
            Debug.Log("Test!!!");
            //Application.LoadLevel(1);
        }
        //guiText.text = "Fuuck!!";
    }
}
