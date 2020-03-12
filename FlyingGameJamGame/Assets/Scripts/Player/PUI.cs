//  Copyright © Loui Eriksson
//  All Rights Reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PUI : MonoBehaviour {

    private PMain m_PMain;

    private Image[] m_WeaponCharges = new Image[2];

    // Start is called before the first frame update
    public void Init(PMain _pMain) {
        m_PMain = _pMain;

        m_WeaponCharges[0] = SceneCamera.Instance.transform.Find("UI/Reticles/Weapon_Charge_Left").GetComponent<Image>();
        m_WeaponCharges[1] = SceneCamera.Instance.transform.Find("UI/Reticles/Weapon_Charge_Right").GetComponent<Image>();
    }

    // Update is called once per frame
    private void Update() {
        AnimateWeaponCharge();

    }

    private void AnimateWeaponCharge() {

        for (int i = 0; i < 2; i++)
        {
            m_WeaponCharges[i].fillAmount = 1.0f - m_PMain.m_PShoot.m_WeaponCharge[i];

            if (m_WeaponCharges[i].fillAmount == 1) {
                m_WeaponCharges[i].fillAmount = 0.0f;
            }
        }
    }
    
}