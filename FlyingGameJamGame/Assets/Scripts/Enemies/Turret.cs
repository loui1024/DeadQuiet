//  Copyright © Loui Eriksson
//  All Rights Reserved.

using System.Collections;
using UnityEngine;

public class Turret : BHealth {

    public BHealth m_Target;

    [SerializeField] private Transform m_TopSegment;

    [SerializeField] private BWeapon m_Weapon;
    [SerializeField] private float m_TrackSpeed;
    [SerializeField] private float m_Range;

    private AudioSource m_Audio;
    private Quaternion m_TargetRotation;
    private float m_ShootTimer;

    private void Start() {
        m_Audio = GetComponent<AudioSource>();
    }

    private void LateUpdate() {

        if (m_Target && (m_Target.transform.position - transform.position).sqrMagnitude <= m_Range * m_Range) {

            Aim();

            if (m_ShootTimer <= 0) {
                if (Physics.Linecast(m_TopSegment.position, m_Target.transform.position - m_TopSegment.forward * 20.0f, m_Weapon.m_CollisionLayers) == false) {

                    StartCoroutine(Shoot());

                    m_ShootTimer = 60.0f / (float)m_Weapon.m_RateOfFire;
                }
            }
        }
        m_ShootTimer -= Time.deltaTime;
    }

    private void Aim() {
        m_TargetRotation = Quaternion.LookRotation(m_Target.transform.position - transform.position);
        m_TopSegment.rotation = Quaternion.Slerp(m_TopSegment.rotation, m_TargetRotation, Time.deltaTime * m_TrackSpeed);
    }

    private IEnumerator Shoot() {

        BHealth target = null;

        // Burst fire.
        for (int i = 0; i < m_Weapon.m_Bursts; i++) {

            if (m_Weapon.m_ShootSound) {
                m_Audio.PlayOneShot(m_Weapon.m_ShootSound);
            }

            if (m_Weapon.GetType() == typeof(ProjectileWeapon)) {

                // Projectile starting position on left / right of camera.
                Vector3 pos = transform.position + transform.forward;
                Quaternion rot = m_TopSegment.rotation;

                if (Vector3.Angle(m_TopSegment.forward, m_Target.transform.position - transform.position) < 20.0f) {
                    target = m_Target;
                }

                // Instantiate projectile and initiate stats.
                BProjectile projectile = Instantiate(m_Weapon.m_Model, pos, rot).AddComponent<BProjectile>();

                projectile.Init((ProjectileWeapon)m_Weapon, new Vector3(), target, this);
            }

            // Case for delay between bursts.
            if (m_Weapon.m_BurstDelay != 0.0f) {
                yield return new WaitForSeconds(m_Weapon.m_BurstDelay);
            }
        }
    }
}