//  Copyright © Loui Eriksson
//  All Rights Reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PShoot : MonoBehaviour {

    private PMain m_PMain;

    int[] m_PlayerAmmo = new int[6];
    int[] m_CurrWeapons = new int[2];
    bool[] m_PlayerWeapons = new bool[6];
    float[,] m_Cooldowns = new float[2,6];

    public bool[] m_FireInputLastFrame = new bool[2];

    // Start is called before the first frame update
    public void Init(PMain _pMain) {
        m_PMain = _pMain;

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
        for (int i = 0; i < 2; i++)
        {
            if (m_PMain.m_PInput.m_FireInput[i])
            {
                if (m_Cooldowns[i, m_CurrWeapons[i]] <= 0.0f)
                {

                    // Special conditions for single shot weapons.
                    bool canFire = true;

                    if (PlayerParameters.Instance.m_PlayerWeapons[m_CurrWeapons[i]].m_FullAuto == false)
                    {
                        if (m_FireInputLastFrame[i] == false) {
                            m_FireInputLastFrame[i] = true;
                        }
                        else {
                            canFire = false;
                        }
                    }

                    if (canFire)
                    {
                        StartCoroutine(Shoot(i, m_CurrWeapons[i]));

                        m_Cooldowns[i, m_CurrWeapons[i]] = 60.0f / PlayerParameters.Instance.m_PlayerWeapons[m_CurrWeapons[i]].m_RateOfFire;
                    }
                }
            }
            else {
                m_FireInputLastFrame[i] = false;
            }
        }

        // Subtract delta time from timers.
        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < 6; j++) {
                m_Cooldowns[i, j] -= Time.deltaTime;
            }
        }
    }

    private IEnumerator Shoot(int _index, int _weaponID) {

        BWeapon weapon = PlayerParameters.Instance.m_PlayerWeapons[m_CurrWeapons[_weaponID]];

        // Burst fire.
        for (int i = 0; i < weapon.m_Bursts; i++) {

            if (m_PlayerAmmo[_weaponID] > 0) {

                // Projectile starting position on left / right of camera.
                Vector3 pos = transform.position + (transform.rotation * new Vector3(((float)_index - 0.5f) * 8f, 0.0f, 0.0f));
                Quaternion rot;

                // Target the surface on the center of the screen.
                RaycastHit hit;
                if (Physics.Raycast(pos, SceneCamera.Instance.transform.forward, out hit, Mathf.Infinity, PlayerParameters.Instance.m_TargetingLayers, QueryTriggerInteraction.Ignore))
                {
                    rot = Quaternion.Euler(hit.point - pos);
                }
                else
                {
                    rot = SceneCamera.Instance.transform.rotation;
                }

                // Instantiate projectile and initiate stats.
                BProjectile projectile = Instantiate(weapon.m_Model, pos, rot).AddComponent<BProjectile>();
                projectile.Init((ProjectileWeapon)weapon, m_PMain.m_PMove.m_Velocity);

                m_PlayerAmmo[_weaponID]--;

            }

            // Case for delay between bursts.
            if (weapon.m_BurstDelay != 0.0f)
            {
                yield return new WaitForSeconds(weapon.m_BurstDelay);
            }
        }

    }
}