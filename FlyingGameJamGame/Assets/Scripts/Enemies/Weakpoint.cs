//  Copyright © Loui Eriksson
//  All Rights Reserved.

using UnityEngine;

public class Weakpoint : BHealth {

    [SerializeField] private GameObject m_Debris;

    public override void Die() {

        MusicManager.Instance.m_Intensity = 1.5f;

        Instantiate(m_Debris, transform.position, transform.rotation);

        base.Die();
    }
}