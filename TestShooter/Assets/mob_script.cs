using UnityEngine;
using System.Collections;

public class mob_script : MonoBehaviour
{
    [SerializeField] Texture2D m_cross = null;
    [SerializeField] abstractWeapon m_rifle = null;
    //----------attach-animation-for-rifle----------
    [SerializeField] Animation m_GunIdle = null;
    [SerializeField] Animation m_GunShot = null;
    [SerializeField] Animation m_GunWalk = null;
    [SerializeField] Animation m_GunRun = null;
    //----------------------------------------------
    public int health
    {
        get;
        private set;
    }
    public NetworkView netview { get { return GetComponent<NetworkView>(); } }
    public abstractWeapon rifle { get { return m_rifle; } }
    //public CharacterController charCtrl { get { return GetComponent<CharacterController>(); } }

    void Start()
    {
        if (networkView && networkView.isMine)
        {
            m_rifle.animation.AddClip(m_GunShot.clip, "Fire");
            m_rifle.animation.AddClip(m_GunIdle.clip, "Idle");
            m_rifle.animation.AddClip(m_GunWalk.clip, "Walk");
            m_rifle.animation.AddClip(m_GunRun.clip, "Run");
            health = 100;
            GameObject.Find("ShotButton").GetComponent<GUIButton>().OnStartPress = delegate { networkView.RPC("Shot",networkView.owner); m_rifle.SetAnimStates(abstractWeapon.WeaponAnimStates.shot); };
            GameObject.Find("ShotButton").GetComponent<GUIButton>().OnRelease = delegate { m_rifle.SetAnimStates(abstractWeapon.WeaponAnimStates.idle); };
        }
    }

    void Update()
    {

    }

    public void OnGUI()
    {
        if (networkView && networkView.isMine)
        {
            Rect rectLabel = new Rect(10, Screen.height - 30, 100, 50);
            GUI.Label(rectLabel, string.Concat("Helth: ", health.ToString()));
            Rect rectButtonShot = new Rect(Screen.width - 105, Screen.height - 205, 100, 100);
           // if (GUI.Button(rectButtonShot, "SHOT"))
           // { Shot(); }
            Rect rectCross = new Rect((Screen.width / 2) - 30, (Screen.height / 2) - 30, 60, 60);
            GUI.DrawTexture(rectCross, m_cross);
        }
    }
    [RPC]
    public void Shot()
    {
        m_rifle.SetAnimStates(abstractWeapon.WeaponAnimStates.shot);
        Debug.Log("Shot");
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));//(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Collider cldr = hit.collider;
            if (cldr != null && cldr.GetComponent<mob_script>() != null && cldr.GetComponent<mob_script>() != this)
            {
                cldr.GetComponent<NetworkView>().RPC("CalckHit", cldr.GetComponent<NetworkView>().owner);
            }
            else if (cldr != null && cldr.GetComponent<bot_script>() != null)
            {
                cldr.GetComponent<NetworkView>().RPC("Dead", cldr.GetComponent<NetworkView>().owner);//<bot_script>().Dead();
            }
                //hit.collider.enabled = false;
        }
    }

    [RPC] void CalckHit()
    {
        Debug.Log("hited");
        health -= 10;
    }
}
