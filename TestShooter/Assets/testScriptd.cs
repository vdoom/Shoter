using UnityEngine;
using System.Collections;

public class testScriptd : MonoBehaviour
{
    //[SerializeField] Animation m_anim = null;
    //[SerializeField] GameObject m_player = null;
    [SerializeField] GameObject playerPrefab;
    // Use this for initialization
    void Start()
    {
        Network.natFacilitatorIP = "89.252.4.131";
        MasterServer.ipAddress = "89.252.4.131";
        MasterServer.RequestHostList("MadBubbleSmashGame");
        //m_anim.clip.wrapMode = WrapMode.Loop;
        //m_anim.Play("run", AnimationPlayMode.Mix);
        //m_anim.clip.isLooping = true;
       // m_anim.clip.wrapMode = WrapMode.Loop;
        //Debug.Log("try To start Play Anim");
       // Timer m_timer = gameObject.AddComponent<Timer>();
        //m_timer.Init(25.0f, (t_timer) => { Debug.Log("Dead"); m_anim.Play("death"); });
        Timer m_refresher = gameObject.AddComponent<Timer>();
        m_refresher.Init(1.0f, (t_timer) => { MasterServer.RequestHostList("MadBubbleSmashGame"); }, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        { Application.Quit(); }
    }
    
    void OnGUI()
    {
        if (GUILayout.Button("Start Server"))
        {
            // Use NAT punchthrough if no public IP present
            //MasterServer.ipAddress = "192.168.1.3";
            Network.InitializeServer(32, 55370, !Network.HavePublicAddress());
            MasterServer.RegisterHost("MadBubbleSmashGame", "JohnDoes game", "l33t game for all");
        }
        HostData[] data = MasterServer.PollHostList();
	    // Go through all the hosts in the host list
	    foreach (HostData element in data)
	    {
	    	GUILayout.BeginHorizontal();	
	    	string name = element.gameName + " " + element.connectedPlayers + " / " + element.playerLimit;
	    	GUILayout.Label(name);	
	    	GUILayout.Space(5);
	    	string hostInfo;
	    	hostInfo = "[";
	    	foreach (var host in element.ip)
	    		hostInfo = hostInfo + host + ":" + element.port + " ";
	    	hostInfo = hostInfo + "]";
	    	GUILayout.Label(hostInfo);	
	    	GUILayout.Space(5);
	    	GUILayout.Label(element.comment);
	    	GUILayout.Space(5);
	    	GUILayout.FlexibleSpace();
	    	if (GUILayout.Button("Connect"))
	    	{
	    		// Connect to HostData struct, internally the correct method is used (GUID when using NAT).
	    		Network.Connect(element);			
	    	}
	    	GUILayout.EndHorizontal();	
	    }
    }

    //void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    //{
    //    Vector3 tmpPos = new Vector3(0, 0, 0);
    //    if (stream.isWriting)
    //    {
    //        if (Network.isClient)
    //        {
    //            Debug.Log("Try Serialyze Out");
    //            tmpPos = m_player.transform.position;
    //            stream.Serialize(ref tmpPos);
    //        }
    //    }
    //    else
    //    {
    //        stream.Serialize(ref tmpPos);
    //        if (Network.isServer)
    //        {
    //            Debug.Log("Try Serialyze In");
    //            m_player.transform.position = tmpPos;
    //        }
    //       // currentHealth = health;
    //    }
    //}

    //void OnConnectedToServer()
    //{
    //    Network.SetSendingEnabled(22, true);
    //    Debug.Log("Connected to server");
    //}
    void OnPlayerConnected(NetworkPlayer player)
    {
        //foreach (NetworkPlayer playerN in Network.connections)
        //{
        //    Network.SetReceivingEnabled(playerN, 22, true);
        //}
       // Network.SetReceivingEnabled(player, 22, true);
        
        Debug.Log("Player connected from " + player.ipAddress + ":" + player.port);
    }
    void OnFailedToConnect(NetworkConnectionError error)
    {
        Debug.Log("Could not connect to server: " + error);
    }

    //public GameObject playerPrefab;

    void OnServerInitialized()
    {
        SpawnPlayer();
    }

    void OnConnectedToServer()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        Network.Instantiate(playerPrefab, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
    }
}
