using UnityEngine;
using System.Collections;

public class BotSpawner : MonoBehaviour 
{
	[SerializeField] BotType m_botType = BotType.none;
	
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
			gameObject.AddComponent<BoxCollider>();
			GetComponent<BoxCollider>().size = new Vector3(10, 10, 10);
		}
	}
	
	void Update () 
	{
	}
}
