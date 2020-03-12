//  Copyright © Loui Eriksson
//  All Rights Reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PShoot : MonoBehaviour {

    private PMain m_PMain;

    private int[] m_PlayerAmmo = new int[6];
    private int[] m_CurrWeapons = new int[2];
    private bool[] m_PlayerWeapons = new bool[6];
    private bool[] m_FireInputLastFrame = new bool[2];
    private float[,] m_Cooldowns = new float[2, 6];
    private float[] m_MinigunTurnSpeed = new float[2];

    private Transform[] m_WeaponAnchors = new Transform[2];

    private float[] m_RecoilTargets = new float[2];
    private Vector3[] m_DefaultTargets = new Vector3[2];

    // Start is called before the first frame update
    public void Init(PMain _pMain) {
        m_PMain = _pMain;

        m_WeaponAnchors[0] = transform.Find("Weapons/Left");
        m_WeaponAnchors[1] = transform.Find("Weapons/Right");

        m_DefaultTargets[0] = m_WeaponAnchors[0].position;
        m_DefaultTargets[1] = m_WeaponAnchors[1].position;

        for (int i = 0; i < 6; i++) {
            m_PlayerAmmo[i] = PlayerParameters.Instance.m_PlayerWeapons[i].m_Ammo;
        }
    }

    public void AddWeapon(int _weaponID) {
        m_PlayerWeapons[_weaponID] = true;
    }

    public void AddAmmo(int _amount, int _weaponID) {
        if (_amount > 0) {
            m_PlayerAmmo[_weaponID] += _amount;
        }
    }

    // Update is called once per frame
    private void Update() {

        // Check weapon fire input and shoot.
        for (int i = 0; i < 2; i++) {
            if (m_PMain.m_PInput.m_FireInput[i]) {
                if (m_Cooldowns[i, m_CurrWeapons[i]] <= 0.0f) {

                    // Special conditions for single shot weapons.
                    bool canFire = true;

                    if (PlayerParameters.Instance.m_PlayerWeapons[m_CurrWeapons[i]].m_FullAuto == false) {
                        if (m_FireInputLastFrame[i] == false) {
                            m_FireInputLastFrame[i] = true;
                        }
                        else {
                            canFire = false;
                        }
                    }

                    if (canFire) {
                        StartCoroutine(Shoot(i, m_CurrWeapons[i]));

                        m_Cooldowns[i, m_CurrWeapons[i]] = 60.0f / PlayerParameters.Instance.m_PlayerWeapons[m_CurrWeapons[i]].m_RateOfFire;
                    }
                }
            }
            else {
                m_FireInputLastFrame[i] = false;
            }

            AnimateWeaponRecoil(i);
        }

        // Subtract delta time from timers.
        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < 6; j++) {
                m_Cooldowns[i, j] -= Time.deltaTime;
            }
        }
    }

    private void RecoilWeapons(int _index) {

        /*
         *  HARD CODED WEAPON RECOIL TO SAVE TIME ONLY!
         *  REPLACE WITH ACTUAL ANIMATIONS IF IMPLEMENTING PROPERLY!
         */

        if (m_CurrWeapons[_index] == 0) {
            m_MinigunTurnSpeed[_index] = Mathf.Lerp(m_MinigunTurnSpeed[_index], 200.0f, Time.deltaTime * 3.0f);
        }
        else {
            m_RecoilTargets[_index] = 1.0f;
        }
    }

    private void AnimateWeaponRecoil(int _index) {

        /*
         *  HARD CODED WEAPON RECOIL TO SAVE TIME ONLY!
         *  REPLACE WITH ACTUAL ANIMATIONS IF IMPLEMENTING PROPERLY!
         */

        // Miniguns.
        m_WeaponAnchors[_index].GetChild(0).Rotate(0, 0, m_MinigunTurnSpeed[_index]);
        m_MinigunTurnSpeed[_index] = Mathf.Lerp(m_MinigunTurnSpeed[_index], 0, Time.deltaTime);

        // Generic Recoil.
        if (m_RecoilTargets[_index] > 0.01f) {
            m_RecoilTargets[_index] = Mathf.Lerp(m_RecoilTargets[_index], 0, Time.deltaTime * 10.0f);
        }

        m_WeaponAnchors[_index].transform.localPosition = Vector3.Lerp(m_WeaponAnchors[_index].transform.localPosition, m_DefaultTargets[_index] + transform.forward * -m_RecoilTargets[_index] * 0.5f, Time.deltaTime * 15.0f);
    }

    private IEnumerator Shoot(int _index, int _weaponID) {

        BWeapon weapon = PlayerParameters.Instance.m_PlayerWeapons[m_CurrWeapons[_weaponID]];

        // Burst fire.
        for (int i = 0; i < weapon.m_Bursts; i++) {

            if (m_PlayerAmmo[_weaponID] > 0) {

                RecoilWeapons(_index);

                // Projectile starting position on left / right of camera.
                Vector3 pos = transform.position + (transform.rotation * new Vector3(((float)_index - 0.5f) * 8f, -1.5f, 1.0f));
                Quaternion rot;

                // Target the surface on the center of the screen.
                RaycastHit hit;
                if (Physics.Raycast(pos, SceneCamera.Instance.transform.forward, out hit, Mathf.Infinity, PlayerParameters.Instance.m_TargetingLayers, QueryTriggerInteraction.Ignore)) {
                    rot = Quaternion.LookRotation(hit.point - pos);
                }
                else {
                    rot = SceneCamera.Instance.transform.rotation;
                }

                // Instantiate projectile and initiate stats.
                BProjectile projectile = Instantiate(weapon.m_Model, pos, rot).AddComponent<BProjectile>();
                projectile.Init((ProjectileWeapon)weapon, m_PMain.m_PMove.m_Velocity);

                m_PlayerAmmo[_weaponID]--;
            }

            // Case for delay between bursts.
            if (weapon.m_BurstDelay != 0.0f) {
                yield return new WaitForSeconds(weapon.m_BurstDelay);
            }
        }
    }
}