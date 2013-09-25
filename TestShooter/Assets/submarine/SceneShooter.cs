using UnityEngine;
using System.Collections.Generic;

public class SceneShooter : MonoBehaviour 
{
	[SerializeField] List<BotSpawner> m_botSpawners = null;
	[SerializeField] List<mob_script> m_players = null;
	[SerializeField] List<mob_script> m_bots = null;
	
	void Start () 
	{
	}
	
	void Update () 
	{
	}
	
	public virtual void StartGameMechanics()
	{}
}
