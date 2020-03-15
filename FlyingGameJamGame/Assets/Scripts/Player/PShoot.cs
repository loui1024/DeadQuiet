//  Copyright © Loui Eriksson
//  All Rights Reserved.

using System.Collections;
using UnityEngine;

public class PShoot : MonoBehaviour {

    private PMain m_PMain;

    public int[] m_CurrWeapons { get; private set; } = new int[2];
    public int[] m_PlayerAmmo { get; private set; } = new int[6];
    public float[] m_WeaponCharge { get; private set; } = new float[2];
    public GameObject[] m_BeamEffects { get; private set; } = new GameObject[2];

    private bool[] m_PlayerWeapons = new bool[6];
    private bool[] m_FireInputLastFrame = new bool[2];
    private float[,] m_Cooldowns = new float[2, 6];
    private float[] m_MinigunTurnSpeed = new float[2];
    private float[] m_RecoilTargets = new float[2];
    private Vector3[] m_DefaultTargets = new Vector3[2];
    private GameObject[,] m_WeaponModels = new GameObject[2, 6];
    private AudioSource[] m_WeaponAudio = new AudioSource[2];
    private Transform[] m_WeaponAnchors = new Transform[2];

    // Start is called before the first frame update
    public void Init(PMain _pMain) {
        m_PMain = _pMain;

        m_WeaponAnchors[0] = transform.Find("Weapons/Left");
        m_WeaponAnchors[1] = transform.Find("Weapons/Right");

        m_BeamEffects[0] = transform.Find("Weapons/Laser_Beam_Left").gameObject;
        m_BeamEffects[1] = transform.Find("Weapons/Laser_Beam_Right").gameObject;

        m_DefaultTargets[0] = m_WeaponAnchors[0].position;
        m_DefaultTargets[1] = m_WeaponAnchors[1].position;

        for (int i = 0; i < 6; i++) {
            m_PlayerAmmo[i] = PlayerParameters.Instance.m_PlayerWeapons[i].m_Ammo;

            for (int j = 0; j < 2; j++) {
                try {
                    m_WeaponModels[j, i] = m_WeaponAnchors[j].GetChild(i).gameObject;
                }
                catch { }
            }
        }

        for (int i = 0; i < 2; i++) {
            m_WeaponAudio[i] = m_WeaponAnchors[i].GetComponent<AudioSource>();
        }

        ChangeWeapon(0, 0);
        ChangeWeapon(1, 0);
    }

    public void ChangeWeapon(int _index, int _weaponID) {

        m_CurrWeapons[_index] = _weaponID;

        for (int i = 0; i < 6; i++) {
            if (m_WeaponModels[_index, i]) {
                m_WeaponModels[_index, i].SetActive(i == m_CurrWeapons[_index]);
            }
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
                if (m_Cooldowns[i, m_CurrWeapons[i]] <= 0.0f && m_WeaponCharge[i] <= 0.0f) {

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

                m_WeaponCharge[i] -= Time.deltaTime / PlayerParameters.Instance.m_PlayerWeapons[m_CurrWeapons[i]].m_ChargeTime;
            }
            else {
                m_WeaponCharge[i] = 1;

                m_FireInputLastFrame[i] = false;

                m_BeamEffects[i].SetActive(false);
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

        //m_WeaponAnchors[_index].transform.localPosition = Vector3.Lerp(m_WeaponAnchors[_index].transform.localPosition, m_DefaultTargets[_index] + Vector3.forward * -m_RecoilTargets[_index] * 0.5f, Time.deltaTime * 15.0f);
    }

    private IEnumerator Shoot(int _index, int _weaponID) {

        BHealth target = null;
        BWeapon weapon = PlayerParameters.Instance.m_PlayerWeapons[m_CurrWeapons[_index]];

        // Burst fire.
        for (int i = 0; i < weapon.m_Bursts; i++) {

            if (m_PlayerAmmo[_weaponID] > 0) {

                RecoilWeapons(_index);
                m_PlayerAmmo[_weaponID]--;

                StartCoroutine(m_PMain.m_PCamera.Shake(weapon.m_ShakeMagnitude, weapon.m_ShakeFrequency, weapon.m_ShakeDuration));

                if (weapon.m_ShootSound) {
                    m_WeaponAudio[_index].PlayOneShot(weapon.m_ShootSound);
                }

                if (weapon.GetType() == typeof(ProjectileWeapon)) {

                    // Projectile starting position on left / right of camera.
                    Vector3 pos = transform.position + (transform.rotation * new Vector3(((float)_index - 0.5f) * 8f, -1.5f, 1.0f));
                    Quaternion rot;

                    // Target the surface on the center of the screen.
                    RaycastHit hit;
                    if (Physics.Raycast(pos, SceneCamera.Instance.transform.forward, out hit, Mathf.Infinity, PlayerParameters.Instance.m_TargetingLayers, QueryTriggerInteraction.Ignore)) {
                        rot = Quaternion.LookRotation(hit.point - pos);

                        target = hit.transform.GetComponent<BHealth>();
                    }
                    else {
                        rot = SceneCamera.Instance.transform.rotation;
                    }

                    // Instantiate projectile and initiate stats.
                    BProjectile projectile = Instantiate(weapon.m_Model, pos, rot).AddComponent<BProjectile>();

                    projectile.Init((ProjectileWeapon)weapon, m_PMain.m_PMove.m_Velocity, target, m_PMain);
                }
                else {

                    GameObject beam = m_BeamEffects[_index];

                    // Projectile starting position on left / right of camera.
                    Vector3 pos = beam.transform.position;
                    Quaternion rot;

                    RaycastHit hit;

                    float distance;

                    if (Physics.Raycast(pos, SceneCamera.Instance.transform.forward, out hit, Mathf.Infinity, PlayerParameters.Instance.m_TargetingLayers, QueryTriggerInteraction.Ignore)) {

                        beam.transform.rotation = Quaternion.LookRotation(hit.point - pos);

                        if (Physics.Raycast(pos, hit.point - pos, out hit, Mathf.Infinity, weapon.m_CollisionLayers, QueryTriggerInteraction.Ignore)) {

                            BHealth health;

                            if (health = hit.transform.GetComponent<BHealth>()) {
                                health.TakeDamage(weapon.m_Damage);
                            }
                        }

                        distance = (hit.point - pos).magnitude;
                    }
                    else {
                        distance = 10000.0f;
                    }

                    Vector3 beamScale = beam.transform.localScale;
                    beamScale.x = ((BeamWeapon)weapon).m_BeamWidth;
                    beamScale.y = ((BeamWeapon)weapon).m_BeamWidth;
                    beamScale.z = distance;

                    beam.transform.localScale = beamScale;

                    beam.SetActive(true);
                }
            }
            else {
                m_BeamEffects[_index].SetActive(false);
            }

            // Case for delay between bursts.
            if (weapon.m_BurstDelay != 0.0f) {
                yield return new WaitForSeconds(weapon.m_BurstDelay);
            }
        }
    }
}