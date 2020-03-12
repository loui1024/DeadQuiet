//  Copyright © Loui Eriksson
//  All Rights Reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingGround : MonoBehaviour {

    [SerializeField] private Transform[] m_GroundObjects;
    [SerializeField] private float m_ScrollSpeed;

    private void Update() {
        foreach (Transform t in m_GroundObjects) {
            if (t.position.z < -2500.0f) {
                t.position = new Vector3(t.position.x, t.position.y, 2500.0f * (m_GroundObjects.Length + 1));
            }

            t.position += Vector3.forward * -m_ScrollSpeed * Time.deltaTime;
        }
    }
}