using UnityEngine;
using System.Collections.Generic;

public class MainScene : BaseScene
{
    [SerializeField] ColorPlane m_fade = null;

    protected List<BaseScene> m_loadedScene = null;
    protected BaseScene m_activeScene = null;

    public BaseScene activeScene
    {
        get {return m_activeScene;}
        private set { m_activeScene = value;}
    }

    public override void Start()
    {
        base.Start();
        m_loadedScene = new List<BaseScene>();
        Screen.SetResolution(800, 480, true);
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Escape))
        { Application.Quit(); }
    }
    //TODO: NEED REFINE!!! ONLEVELlOAD NEED FREEZ & AFTER LOAD(PARHAPSE WHENE REGISTR LEVEL OR UFTER) NUUD UNFREEZ
    public void LoadLevel(int t_levelIndex)
    {
        StartLoadAnim();
        Timer timer = gameObject.AddComponent<Timer>();
        timer.Init(0.6f, t =>
        {
            LoadLevelWithoutAnim(t_levelIndex);
        });
    }

    public void LoadLevel(string t_levelName)
    {
        StartLoadAnim();
        Timer timer = gameObject.AddComponent<Timer>();
        timer.Init(0.6f, t =>
        {
            LoadLevelWithoutAnim(t_levelName);
        });
    }

    public void LoadLevelWithoutAnim(int t_levelIndex)
    {
        if (m_activeScene != null)
        {
            Destroy(m_activeScene.gameObject);
        }
        Application.LoadLevelAdditive(t_levelIndex);
    }

    public void LoadLevelWithoutAnim(string t_levelName)
    {
        if (m_activeScene != null)
        {
            Destroy(m_activeScene.gameObject);
        }
        Application.LoadLevelAdditive(t_levelName);
    }

    public void StartLoadAnim()
    {
        //m_fade.transform.localPosition = new Vector3(0, 0, -1000);
        if (m_loadedScene.Count > 0)
        {
            m_fade.FaidIn();
        }
        else
        {
            m_fade.Show();
        }
        //Debug.Log("Create Fade");
    }

    public void StopLoadAnim()
    {
        if (m_fade)
        {
            //m_fade.Hide();
            m_fade.FaidOut();
        }
    }

    public void RegisterLevel(BaseScene t_regScene)
    {
        if (!m_loadedScene.Contains(t_regScene))
        {
            m_loadedScene.Add(t_regScene);
        }
        activeScene = t_regScene;

        StopLoadAnim();
    }

    public void UnRegisterLevel(BaseScene t_regScene)
    {
        if (m_loadedScene.Contains(t_regScene))
        {
            m_loadedScene.Remove(t_regScene);
        }
        if (m_loadedScene.Count > 0)
        { activeScene = m_loadedScene[m_loadedScene.Count - 1]; }
        else { activeScene = null; }
    }

    public void OnApplicationPause(bool t_isPused)
    {
        if (t_isPused)
        { Application.Quit(); }
    }

}
