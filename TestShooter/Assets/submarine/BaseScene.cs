using UnityEngine;
using System.Collections.Generic;

public class BaseScene : GameObjectEx
{
    private Camera m_mainCamer = null;//Need Refina: Kill Or Refine!!!
    [SerializeField] bool m_createCam = false;
    public override void Start()
    {
        base.Start();
        if (m_createCam)
        { CreateCam(); }
    }

    public override void Update()
    {

    }

    public override void Freeze()
    {
        base.Freeze();
        List<GameObjectEx> children = GetObjects<GameObjectEx>(true);
        foreach (GameObjectEx child in children)
        {
            child.UnFreeze();
        }
    }

    public override void UnFreeze()
    {
        base.UnFreeze();

        List<GameObjectEx> children = GetObjects<GameObjectEx>(true);
        foreach (GameObjectEx child in children)
        {
            child.UnFreeze();
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(Vector2.zero,
                        new Vector3(800, 0));
        Gizmos.DrawLine(new Vector3(800, 0),
                        new Vector3(800, 480));
        Gizmos.DrawLine(new Vector3(800, 480),
                        new Vector3(0, 480));
        Gizmos.DrawLine(new Vector3(0, 480),
                        Vector2.zero);
    }

    public void CreateCam()
    {
        GameObject cameraGO = new GameObject("Main_Camera");
        cameraGO.transform.position = new Vector3(400, 240, -10000);
        m_mainCamer = cameraGO.AddComponent<Camera>();
        m_mainCamer.orthographic = true;
        m_mainCamer.orthographicSize = 240;
        m_mainCamer.far = 110000;
    }
    // Undone: NEED FREEZ & UNFREEZ FOR PAUSE!!!
    // AND REFINE GAMEOBJECTEX WITH FREEZ & UNFREEZ!!!

    public void AddTimer(float t_time, System.Action<Timer> t_tickAction) 
    {
        Timer timer = gameObject.AddComponent<Timer>();
        timer.Init(t_time, t_tickAction);
    }
}
