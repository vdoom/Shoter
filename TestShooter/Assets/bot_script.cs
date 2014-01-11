using UnityEngine;
using System.Collections;

public class bot_script : MonoBehaviour
{
    [SerializeField]
    Animation tmp = null;
    bool m_isDead = false;
    public NavMeshAgent navMeshAgent { get { return GetComponent<NavMeshAgent>(); } }
    NavMesh mm;
    public bool isDead { get { return m_isDead; } } 
    private Vector3 destPos = Vector3.zero;
    private GameObject destObject = null;
    public NetworkView netview { get { return GetComponent<NetworkView>(); } }

    public float timeOfDeath
    { get; private set; }
    public int health
    {
        get;
        private set;
    }
    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;
    private Vector3 syncStartRotation = Vector3.zero;
    private Vector3 syncEndRotation = Vector3.zero;

    void Start()
    {
        animation.AddClip(tmp.clip, "deading");
    }

    void Awake()
    {
        animation.AddClip(tmp.clip, "deading");
    }

    void Update()
    {
        if (!m_isDead && Network.isServer)
        {
            if (!destObject)
                destObject = GameObject.Find("Bot(Clone)");

            if (destObject && (transform.position - destObject.transform.position).magnitude > 5)
            {
                destPos = destObject.transform.position;
            }
            else
            {
                destPos = transform.position;
            }
            navMeshAgent.SetDestination(destPos);
        }
        if (!Network.isServer)
        {
            SyncedMovement();
        }
    }
    [RPC] void Dead()
    {
        if (!m_isDead)
        {
            Debug.Log("Dead");
            animation.Play("deading", AnimationPlayMode.Stop);
            animation.wrapMode = WrapMode.Once;
            m_isDead = true;
            timeOfDeath = Time.time;
        }
    }
    [RPC]
    public void Reset()
    {
        animation.Play("Take 001", AnimationPlayMode.Stop);
        animation.wrapMode = WrapMode.Loop;
        m_isDead = false;
    }

    [RPC]
    void CalckHit()
    {
        Debug.Log("hited");
        health -= 10;
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        Vector3 tmpPos = new Vector3(0, 0, 0);
        Vector3 tmpRot = new Vector3(0, 0, 0);
        if (stream.isWriting)
        {
            tmpPos = transform.position;
            tmpRot = transform.eulerAngles;
            stream.Serialize(ref tmpPos);
            stream.Serialize(ref tmpRot);
        }
        else
        {
            stream.Serialize(ref tmpPos);
            stream.Serialize(ref tmpRot);
            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;
            syncStartPosition = transform.position;
            syncEndPosition = tmpPos;
            syncStartRotation = transform.eulerAngles;
            syncEndRotation = tmpRot;
        }
    }

    private void SyncedMovement()
    {
        syncTime += Time.deltaTime;
        transform.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
        transform.eulerAngles = Vector3.Lerp(syncStartRotation, syncEndRotation, syncTime / syncDelay);
    }
}
