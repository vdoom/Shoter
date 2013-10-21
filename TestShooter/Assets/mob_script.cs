using UnityEngine;
using System.Collections;

public class mob_script : MonoBehaviour
{
    [SerializeField] Texture2D m_cross = null;
    [SerializeField] abstractWeapon m_rifle = null;

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
        health = 100;
        GameObject.Find("ShotButton").GetComponent<GUIButton>().OnStartPress = delegate { Shot(); m_rifle.SetAnimStates(abstractWeapon.WeaponAnimStates.shot); };
        GameObject.Find("ShotButton").GetComponent<GUIButton>().OnRelease = delegate { m_rifle.SetAnimStates(abstractWeapon.WeaponAnimStates.idle); };
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
                //hit.collider.enabled = false;
        }
    }

    [RPC] void CalckHit()
    {
        Debug.Log("hited");
        health -= 10;
    }
}
