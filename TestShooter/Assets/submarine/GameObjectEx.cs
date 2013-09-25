using UnityEngine;
using System.Collections.Generic;

public class GameObjectEx : MonoBehaviour
{
    //private bool m_isActivated = true;
    private float m_opacity = 1;
    public virtual float opacity
    {
        get { return m_opacity; }
        set
        {
            m_opacity = value;
            Renderer rdr = renderer;
            if (rdr != null)
            {
                float op = m_opacity;
                Color clr = rdr.sharedMaterial.color;
                clr.a = op;
                rdr.sharedMaterial.color = clr;
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                GameObjectEx child = transform.GetChild(i).GetComponent<GameObjectEx>();
                if (child != null && !child.ignoreParenOpacity)
                { child.opacity = opacity; }
            }
        }
    }
    public virtual bool isActive
    { get { return gameObject.active; } set { gameObject.SetActive(value); } }
    public virtual bool visible
    {
        get
        {
            if (renderer != null)
                return renderer.enabled;
            return false;
        }
        set
        {
            if (visible != value)
            {
                if (renderer)
                {
                    renderer.enabled = value;
                }
            }
        }
    }
    public Vector3 globalPos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }
    public Vector3 localPos
    {
        get { return transform.localPosition; }
        set { transform.localPosition = value; }
    }
    public Vector3 localScale
    {
        get { return transform.localScale; }
        set { transform.localScale = value; }
    }
    public float globalPosX
    { 
        get { return transform.position.x; }
        set { transform.position = new Vector3(value, transform.position.y, transform.position.z); } 
    }
    public float globalPosY
    {
        get { return transform.position.y; }
        set { transform.position = new Vector3(transform.position.x, value, transform.position.z); }
    }
    public float globalPosZ
    {
        get { return transform.position.z; }
        set { transform.position = new Vector3(transform.position.x, transform.position.y, value); }
    }
    public float localPosX
    {
        get { return transform.localPosition.x; }
        set { transform.localPosition = new Vector3(value, transform.localPosition.y, transform.localPosition.z); }
    }
    public float localPosY
    {
        get { return transform.localPosition.y; }
        set { transform.localPosition = new Vector3(transform.localPosition.x, value, transform.localPosition.z); }
    }
    public float localPosZ
    {
        get { return transform.localPosition.z; }
        set { transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, value); }
    }
    public float eulerAngleX
    {
        get { return transform.eulerAngles.x; }
        set { transform.eulerAngles = new Vector3(value, transform.eulerAngles.y, transform.eulerAngles.z); }
    }
    public float eulerAngleY
    {
        get { return transform.eulerAngles.y; }
        set { transform.eulerAngles = new Vector3(transform.eulerAngles.x, value, transform.eulerAngles.z);}
    }
    public float eulerAngleZ
    {
        get { return transform.eulerAngles.z; }
        set { transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, value); }
    }
    public bool ignoreParenOpacity
    {//Todo: need refine
        get;
        set;
    }
    //public virtual float Opacity
    //{
    //    get
    //    {
    //        if (renderer != null)
    //        {
    //            return renderer.material.color.a;
    //        }
    //        else
    //        {
    //            return -1;
    //        }
    //    }
    //    set
    //    {
    //        if (renderer != null)
    //        {
    //            Color tmp = renderer.material.color;
    //            tmp.a = value;
    //            renderer.material.color = tmp;
    //        }
    //        for (int i = 0; i < transform.childCount; i++)
    //        {
    //            GameObjectEx child = transform.GetChild(i).GetComponent<GameObjectEx>();
    //            if (child != null)
    //            { child.opacity; }
    //        }
    //    }
    //}

    public virtual void Show() { visible = true; }
    public virtual void Hide() { visible = false; }
    public bool freeze { get; private set; }

    public List<T> GetObjects<T>() where T : Component
    {
        return GameObjectEx.GetObjects<T>(this, false);
    }

    public List<T> GetObjects<T>(bool searchRecursively) where T : Component
    {
        return GameObjectEx.GetObjects<T>(this, searchRecursively);
    }

    private static List<T> GetObjects<T>(Component component, bool isRecursiveCheck) where T : Component
    {
        if (isRecursiveCheck)
        {
            T[] components = component.GetComponentsInChildren<T>();
            List<T> retList = new List<T>(components);
            retList.Remove(component.GetComponent<T>());
            return retList;
        }
        else
        {
            List<T> retList = new List<T>();
            foreach (Transform trans in component.transform)
            {
                T comp = trans.GetComponent<T>();
                if (comp != null)
                    retList.Add(comp);
            }
            return retList;
        }
    }

    public virtual void Freeze() { freeze = true; }
    public virtual void UnFreeze() { freeze = false; }

    public virtual void FaidOut()
    {
        gameObject.AddComponent<ChangerFloat>().Create("opacity", this, 0.4f, 1, 0);
    }
    public virtual void FaidIn()
    {
        gameObject.AddComponent<ChangerFloat>().Create("opacity", this, 0.3f, 0, 1);
    }

    public virtual void Start()
    {
        freeze = false;
        ignoreParenOpacity = false;
    }
    //TODO: MAYBE NEED REMAKE TO DoUpdate 
    public virtual void Update()
    { }

    public virtual void OnDestroy()
    { }
}
