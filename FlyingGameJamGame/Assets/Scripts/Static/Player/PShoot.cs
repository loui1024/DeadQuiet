//  Copyright © Loui Eriksson
//  All Rights Reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PShoot : MonoBehaviour {

    private PMain m_PMain;

    bool[] m_PlayerWeapons = new bool[6];
    bool[] m_CurrWeapons = new bool[2];

    // Start is called before the first frame update
    public void Init(PMain _pMain) {
        m_PMain = _pMain;
    }

    public void AddWeapon(int _weaponID) {
        m_PlayerWeapons[_weaponID] = true;
    }

    // Update is called once per frame
    private void Update() {

        for (int i = 0; i < 2; i++) {
            if (m_PMain.m_PInput.m_FireInput[i]) {
                Shoot(i);
            }
        }
    }

    private void Shoot(int _index) {
        if (_index == 0) {
            Debug.Log("Shooting Left Weapon");
        }
        else {
            Debug.Log("Shooting Right Weapon");
        }
    }
}