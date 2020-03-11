//  Copyright © Loui Eriksson
//  All Rights Reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileWeapon", menuName = "Projectile Weapon", order = 1)]
public class ProjectileWeapon : WeaponBase
{

    public int m_RateOfFire;
    public bool m_FullAuto;

    [Header("Projectile Parameters")]
    public float m_ProjectileSpeed;
    
    public GameObject m_ProjectilePrefab;
}

[CreateAssetMenu(fileName = "BeamWeapon", menuName = "Beam Weapon", order = 2)]
public class BeamWeapon : WeaponBase
{

    [Header("Beam Parameters")]
    public float m_BeamWidth;

    public GameObject m_BeamPrefab;
}

public class WeaponBase : ScriptableObject {

    public GameObject m_Debris;

    [Header("Base Stats")]
    public float m_Damage;
    public int m_Ammo;

    [Header("Burst Fire")]
    public int m_Bursts;
    public float m_BurstDelay;

}