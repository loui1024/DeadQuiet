//  Copyright © Loui Eriksson
//  All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.UI;

public class PUI : MonoBehaviour {

    private PMain m_PMain;

    private Image[] m_WeaponCharges = new Image[2];
    private Image m_HealthBar;
    private TextMesh[] m_AmmoCounters = new TextMesh[2];
    private TextMesh m_Clock;
    private float m_TextSize;

    private Mothership m_Mothership;

    // Start is called before the first frame update
    public void Init(PMain _pMain) {
        m_PMain = _pMain;

        m_Mothership = FindObjectOfType<Mothership>();

        m_WeaponCharges[0] = SceneCamera.Instance.transform.Find("UI/Reticles/Weapon_Charge_Left").GetComponent<Image>();
        m_WeaponCharges[1] = SceneCamera.Instance.transform.Find("UI/Reticles/Weapon_Charge_Right").GetComponent<Image>();

        m_HealthBar = SceneCamera.Instance.transform.Find("UI/Health").GetComponent<Image>();

        m_AmmoCounters[0] = transform.Find("Ammo_Left").GetComponent<TextMesh>();
        m_AmmoCounters[1] = transform.Find("Ammo_Right").GetComponent<TextMesh>();

        m_TextSize = m_AmmoCounters[0].characterSize;

        m_Clock = transform.Find("Clock").GetComponent<TextMesh>();
    }

    private void LateUpdate() {
        AnimateWeaponCharge();
        AnimateClock();
        AnimateAmmoCounters();
        AnimateHealth();
    }

    private void AnimateWeaponCharge() {

        for (int i = 0; i < 2; i++) {
            m_WeaponCharges[i].fillAmount = 1.0f - m_PMain.m_PShoot.m_WeaponCharge[i];

            if (m_WeaponCharges[i].fillAmount == 1) {
                m_WeaponCharges[i].fillAmount = 0.0f;
            }
        }
    }

    private void AnimateClock() {

        TimeSpan t = TimeSpan.FromSeconds(m_Mothership.m_TimeRemaining);

        char[] milli = t.Milliseconds.ToString().ToCharArray();
        if (milli.Length > 2) {
            milli = new char[2] { milli[0], milli[1] };
        }

        m_Clock.text = string.Format("{0,1:00}:{1,2:00}:{2,2:00}", t.Minutes, t.Seconds, new string(milli));
    }

    private void AnimateAmmoCounters() {
        for (int i = 0; i < 2; i++) {

            if (m_PMain.m_PShoot.m_PlayerAmmo[m_PMain.m_PShoot.m_CurrWeapons[i]] == 0) {
                m_AmmoCounters[i].text = "EMPTY";
                m_AmmoCounters[i].characterSize = m_TextSize * 0.75f;
            }
            else {
                m_AmmoCounters[i].text = m_PMain.m_PShoot.m_PlayerAmmo[m_PMain.m_PShoot.m_CurrWeapons[i]].ToString();
                m_AmmoCounters[i].characterSize = m_TextSize;
            }
        }
    }

    private void AnimateHealth() {
        m_HealthBar.fillAmount = (float)m_PMain.m_CurrHealth / (float)m_PMain.m_MaxHealth;
    }
}