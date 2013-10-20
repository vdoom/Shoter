using UnityEngine;
using System.Collections;

public abstract class abstractWeapon : MonoBehaviour
{
    public WeaponAnimStates currentAnimState
    { get; protected set; }

    public enum WeaponAnimStates
    {
        idle,
        shot,
        walk,
        run
    }
    // Use this for initialization
    public virtual void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {

    }

    public virtual void SetAnimStates(WeaponAnimStates t_animStates)
    {
        currentAnimState = t_animStates;
    }

    public virtual void Shoot()
    {
    }
 
}
