using UnityEngine;
using System.Collections;

public class BotSpawner : MonoBehaviour 
{
	[SerializeField] BotType m_botType = BotType.none;

    private BoxCollider m_boxCollider = null;
	
	public enum BotType
	{
		Bot,
		Player,
		Zomby,
		none
	}
	
	public BotType botType
	{
		get{return m_botType;}
		private set{m_botType = value;}
	}

	void Start () 
	{
		if(!collider)
		{
            m_boxCollider = gameObject.AddComponent<BoxCollider>();
            m_boxCollider.size = new Vector3(10, 10, 10);
            m_boxCollider.isTrigger = true;
		}
	}
	
	void Update () 
	{
	}
}
