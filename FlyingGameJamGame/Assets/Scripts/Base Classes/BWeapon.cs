using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BWeapon : ScriptableObject {

    public GameObject m_Model;
    public GameObject m_Debris;

    [Header("Base Stats")]
    public float m_Damage;
    public int m_Ammo;
    public int m_RateOfFire;
    public bool m_FullAuto;

    [Header("Burst Fire")]
    public int m_Bursts;
    public float m_BurstDelay;

}
