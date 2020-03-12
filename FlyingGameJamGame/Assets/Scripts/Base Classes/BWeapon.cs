//  Copyright © Loui Eriksson
//  All Rights Reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BWeapon : ScriptableObject {

    public GameObject m_Model;
    public GameObject m_Debris;

    [Header("Collision")]
    public LayerMask m_CollisionLayers;

    [Header("Base Stats")]
    public float m_Damage;
    public int m_Ammo;
    public int m_RateOfFire;
    public bool m_FullAuto;
    public float m_ExplosionRadius;

    [Header("Burst Fire")]
    public int m_Bursts;
    public float m_BurstDelay;
}