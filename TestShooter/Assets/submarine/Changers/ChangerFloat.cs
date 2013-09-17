using UnityEngine;
using System.Collections;
using System.Reflection;

public class ChangerFloat : GameObjectEx
{
    private PropertyInfo m_property = null;
    private GameObjectEx m_invoker = null;
    private float m_time = 0;
    private float m_startVal = 0;
    private float m_endVal = 0;
    //private float m_curr = 0;
    private float m_timeCounter = 0;
    private bool m_activated = false;
    //private bool m_startUpdating = false;

    public System.Action<ChangerFloat> OnFinish = null;

    public bool activated
    { get { return m_activated; } }

    public ChangerFloat Create(string t_propertyName, GameObjectEx t_invoker, float t_time, float t_startVal, float t_endVal)
    {
        m_property = t_invoker.GetType().GetProperty(t_propertyName);
        m_time = t_time;
        m_startVal = t_startVal;
        m_endVal = t_endVal;
        m_activated = true;
        m_invoker = t_invoker;
        return this;
    }

    public override void Update()
    {
        base.Update();
        if (m_activated)
        {
            m_timeCounter+=Time.deltaTime;
            float percentageComplete = (m_timeCounter) / (m_time / 100);
            m_property.SetValue(m_invoker, m_startVal+((m_endVal-m_startVal)*(percentageComplete/100)), null);
            if (m_timeCounter > m_time)
            {
                m_property.SetValue(m_invoker, m_endVal, null);
                if (OnFinish != null)
                { OnFinish(this); }
                //Undone: need destroy
                m_activated = false;
                Destroy(this);
            }
            //Udone: Need check greter of start & end values
            //Todo: need refine good algorythm!!!!
        }
    }
}
