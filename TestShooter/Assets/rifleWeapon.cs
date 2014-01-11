using UnityEngine;
using System.Collections;

public class rifleWeapon : abstractWeapon
{
    [SerializeField] Animation m_shotingFire = null;
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        animation.wrapMode = WrapMode.Loop;
        m_shotingFire.animation.wrapMode = WrapMode.Loop;
        m_shotingFire.Play();
        m_shotingFire.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void Shoot()
    { 
        base.Shoot();
        animation.Play("Fire", AnimationPlayMode.Stop);
        m_shotingFire.Play();
    }

    public override void SetAnimStates(WeaponAnimStates t_animStates)
    {
        base.SetAnimStates(t_animStates);
        //Debug.Log(t_animStates);
        switch (t_animStates)
        {
            case WeaponAnimStates.idle:
                m_shotingFire.gameObject.SetActive(false);
                animation.Play("Idle", AnimationPlayMode.Stop);
                break;
            case WeaponAnimStates.walk:
                m_shotingFire.gameObject.SetActive(false);
                animation.Play("Walk", AnimationPlayMode.Stop);
                break;
            case WeaponAnimStates.run:
                m_shotingFire.gameObject.SetActive(false);
                animation.Play("Run", AnimationPlayMode.Stop);
                break;
            case WeaponAnimStates.shot:
                m_shotingFire.gameObject.SetActive(true);
                animation.Play("Fire", AnimationPlayMode.Stop);
                break;
            default:
                break;
        }
    }
}
