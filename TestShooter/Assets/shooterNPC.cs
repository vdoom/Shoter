using UnityEngine;
using System.Collections;

public class shooterNPC : abstractNPC
{
    [SerializeField] abstractWeapon m_weapon = null;
    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Start();
    }

    public virtual void Shoot()
    { }
}
