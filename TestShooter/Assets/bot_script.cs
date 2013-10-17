using UnityEngine;
using System.Collections;

public class bot_script : MonoBehaviour
{
    public NavMeshAgent navMeshAgent { get { return GetComponent<NavMeshAgent>(); } }

    private Vector3 destPos = Vector3.zero;
    private GameObject destObject = null;
    void Start()
    {

    }

    void Update()
    {
        if (!destObject)
            destObject = GameObject.Find("Bot(Clone)");
        
        if (destObject)
        { destPos = destObject.transform.position; }
        else { destPos = transform.position; }
        
        navMeshAgent.SetDestination(destPos);
    }
}
