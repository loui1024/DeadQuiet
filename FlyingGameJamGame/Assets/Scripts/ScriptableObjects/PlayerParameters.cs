﻿//  Copyright © Loui Eriksson
//  All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "PlayerParameters", menuName = "Player Parameters", order = 0)]
public class PlayerParameters : ScriptableObject {

    private static PlayerParameters m_Instance;

    public static PlayerParameters Instance {
        get {
            if (!m_Instance) {
                m_Instance = (PlayerParameters)Resources.Load("ScriptableObjects/PlayerParameters");
            }

            return m_Instance;
        }
    }

    [Header("Camera")]
    public Vector3 cameraOffset;
    public float m_MouseSensitivity;
    public float m_MinY;
    public float m_MaxY;
    public bool m_InvertYAxis;

    [Header("Movement")]
    public LayerMask m_CollisionLayers;
    public float m_MoveSpeed;
    public float m_TurnSpeed;
    [Range(0, 1)] public float m_SlowDownRate;

    [Header("Weapons")]
    public LayerMask m_TargetingLayers;
    public List<BWeapon> m_PlayerWeapons;
}