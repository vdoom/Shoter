using UnityEngine;
using System.Collections;

public class Timer : GameObjectEx
{
    float m_fireDelay = 0;
	float m_timeToFire = 0;
	bool m_cyclic = false;
	System.Action<Timer> m_onFire;
	float m_lifeTime = -1;

    public bool cyclic
    {
        get { return m_cyclic; }
        set { m_cyclic = value; }
    }

    public void Init(float time, System.Action<Timer> onFire)
    {
        Init(time, onFire, false);
    }
	public void Init(float time, System.Action<Timer> onFire, bool cyclic)
	{
		m_fireDelay = time;
		m_timeToFire = time;
		m_onFire = onFire;
		m_cyclic = cyclic;
	}

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        if (m_lifeTime > 0)
        {
            m_lifeTime -= Time.deltaTime;
            if (m_lifeTime <= 0)
            {
                Destroy(this);
                return;
            }
        }

        m_timeToFire -= Time.deltaTime;
        if (m_timeToFire <= 0)
        {
            if (m_onFire != null)
            {
                m_onFire(this);
            }

            if (!m_cyclic)
            {
                Destroy(this);
            }
            else
            {
                m_timeToFire += m_fireDelay;
            }
        }
    }

    public override void Freeze()
    {
        base.Freeze();
    }

    public override void UnFreeze()
    {
        base.UnFreeze();
    }
}
