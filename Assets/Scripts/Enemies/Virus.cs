using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : Worm
{
    public WeaponHolder myWeapon;

    public override void WeaponInit()
    {
        if (myWeapon != null)
            myWeapon.Initialization();
    }

    public override void WeaponRotateToPlayer()
    {
        if (myWeapon != null)
        {
            myWeapon.Rotate(player.transform.position);
            myWeapon.WeaponShot();
        }
    }

    public override void FreeWeaponRotate()
    {
        if (myWeapon != null)
            myWeapon.Rotate(targetPosition);
    }
}
