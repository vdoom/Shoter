using UnityEngine;
using System.Collections;

public abstract class abstractNPC : MonoBehaviour
{
    protected bool m_invincible;
    protected int m_health;

    public int health
    {
        get { return health; }
        protected set { health = value; }
    }

    // Use this for initialization
    public virtual void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {

    }

    [RPC] void CalckHit()
    {
        Debug.Log("hited");
        health -= 10;
    }
}
