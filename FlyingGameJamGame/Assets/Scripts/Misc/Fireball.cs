//  Copyright © Loui Eriksson
//  All Rights Reserved.

using UnityEngine;

public class Fireball : MonoBehaviour {
    [SerializeField] private float m_GrowRate;

    private Vector3 m_LocalScale;

    private void Start() {
        m_LocalScale = transform.localScale.normalized;
    }

    private void Update() {
        transform.localScale += m_LocalScale * m_GrowRate * Time.deltaTime;
    }
}