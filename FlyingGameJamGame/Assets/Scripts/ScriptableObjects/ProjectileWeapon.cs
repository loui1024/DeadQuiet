﻿//  Copyright © Loui Eriksson
//  All Rights Reserved.

using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileWeapon", menuName = "Projectile Weapon", order = 1)]
public class ProjectileWeapon : BWeapon {

    [Header("Projectile Parameters")]
    public float m_ProjectileSpeed;
    public float m_Lifetime;
    public float m_TrackSpeed;
}