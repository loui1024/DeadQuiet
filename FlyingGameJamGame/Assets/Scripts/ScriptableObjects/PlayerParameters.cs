//  Copyright © Loui Eriksson
//  All Rights Reserved.

using System.Collections;
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
}