//  Copyright © Loui Eriksson
//  All Rights Reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BHealth : MonoBehaviour {

    [Header("Health")]
    public float m_MaxHealth = 100.0f;
    public float m_CurrHealth { get; private set; }

    protected virtual void Awake() {
        Init();
    }

    private void Init() {
        m_CurrHealth = m_MaxHealth;
    }

    public virtual void AddHealth(float _amount) {
        if (_amount > 0) {
            m_CurrHealth = Mathf.Clamp(m_CurrHealth + _amount, 0, m_MaxHealth);
        }
    }

    public virtual void TakeDamage(float _amount) {
        if (_amount > 0) {
            m_CurrHealth = Mathf.Clamp(m_CurrHealth - _amount, 0, m_MaxHealth);
        }

        if (m_CurrHealth == 0) {
            Die();
        }
    }

    public virtual void Die() {

        /*
         *  Dying code goes in here!
         */

        Destroy(gameObject);
    }
}