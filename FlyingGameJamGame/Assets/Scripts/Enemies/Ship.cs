//  Copyright © Loui Eriksson
//  All Rights Reserved.

using System.Collections;
using UnityEngine;

public class Ship : BHealth {

    public BHealth m_Target;

    [SerializeField] private BWeapon m_Weapon;
    [SerializeField] private float m_TurnSpeed;
    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_Range;
    [SerializeField] private LayerMask m_CollisionCheckLayers;

    private AudioSource m_Audio;

    private Quaternion m_TargetRotation;
    private Vector3 m_TargetPostition;

    private float m_ShootTimer;

    private void Start() {
        m_Audio = GetComponent<AudioSource>();

        m_TurnSpeed *= Random.Range(0.25f, 1f);
    }

    private void LateUpdate() {

        Move();

        if (m_Target) {

            Aim();

            transform.SetPositionAndRotation(m_TargetPostition, m_TargetRotation);

            if ((m_Target.transform.position - transform.position).sqrMagnitude <= m_Range * m_Range &&
                Vector3.Angle(transform.forward, (m_Target.transform.position - transform.position)) <= 45) {

                if (m_ShootTimer <= 0) {
                    if (Physics.Linecast(transform.position, m_Target.transform.position - transform.forward * 20.0f, m_Weapon.m_CollisionLayers) == false) {

                        StartCoroutine(Shoot());

                        m_ShootTimer = 60.0f / (float)m_Weapon.m_RateOfFire;
                    }
                }
            }
        }

        m_ShootTimer -= Time.deltaTime;
    }

    private void Aim() {
        if ((m_Target.transform.position - transform.position).sqrMagnitude > m_Range * m_Range ||
            Physics.Linecast(transform.position, transform.position + transform.forward * 500.0f, m_Weapon.m_CollisionLayers)) {

            m_TargetRotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(transform.right),
                Time.deltaTime * m_TurnSpeed * 0.25f
                );
        }
        else {
            m_TargetRotation = Quaternion.Slerp(
              transform.rotation,
              Quaternion.LookRotation(m_Target.transform.position - transform.position),
              Time.deltaTime * m_TurnSpeed * (m_Target.transform.position - transform.position).magnitude / m_Range
              );
        }
    }

    private void Move() {
        m_TargetPostition = transform.position + transform.forward * m_MoveSpeed * Time.deltaTime;
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
                Quaternion rot = transform.rotation;

                if (Vector3.Angle(transform.forward, m_Target.transform.position - transform.position) < 20.0f) {
                    target = m_Target;
                }

                // Instantiate projectile and initiate stats.
                BProjectile projectile = Instantiate(m_Weapon.m_Model, pos, rot).AddComponent<BProjectile>();

                projectile.Init((ProjectileWeapon)m_Weapon, new Vector3(0, 0, m_MoveSpeed), target, this);
            }

            // Case for delay between bursts.
            if (m_Weapon.m_BurstDelay != 0.0f) {
                yield return new WaitForSeconds(m_Weapon.m_BurstDelay);
            }
        }
    }
}