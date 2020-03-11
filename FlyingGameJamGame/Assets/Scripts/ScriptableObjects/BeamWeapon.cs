//  Copyright © Loui Eriksson
//  All Rights Reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BeamWeapon", menuName = "Beam Weapon", order = 2)]
public class BeamWeapon : BWeapon {

    [Header("Beam Parameters")]
    public float m_BeamWidth;
}