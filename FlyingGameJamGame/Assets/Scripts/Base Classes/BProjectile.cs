//  Copyright © Loui Eriksson
//  All Rights Reserved.

using UnityEngine;

public class BProjectile : MonoBehaviour {

    public ProjectileWeapon stats;

    private Transform m_Instigator;
    private Transform m_Target;

    protected Vector3 m_TargetPosition;
    protected Quaternion m_TargetRotation;

    private Vector3 m_ConstantVelocity;

    private float m_Lifetime;

    // Start is called before the first frame update
    public virtual void Init(ProjectileWeapon _stats, Vector3 _velocity, BHealth _target, BHealth _instigator) {

        stats = _stats;

        if (_target) {
            m_Target = _target.transform;
        }

        m_TargetPosition = transform.position;
        m_TargetRotation = transform.rotation;

        m_ConstantVelocity = _velocity;

        m_Instigator = _instigator.transform;
    }

    // Update is called once per frame
    private void Update() {
        if (m_Lifetime <= stats.m_Lifetime) {

            if (m_Target && Vector3.Angle(transform.forward, m_Target.transform.position - transform.position) < 60.0f) {
                Turn();
            }

            Move();

            CheckCollision();

            transform.SetPositionAndRotation(m_TargetPosition, m_TargetRotation);

            m_Lifetime += Time.deltaTime;
        }
        else {
            Hit(null);
        }
    }

    protected virtual void CheckCollision() {

        RaycastHit hit;

        if (Physics.Linecast(transform.position, m_TargetPosition, out hit, stats.m_CollisionLayers, QueryTriggerInteraction.Ignore)) {

            if (hit.transform != m_Instigator) {
                transform.up = hit.normal;
                transform.position = hit.point;

                BHealth other = hit.transform.GetComponent<BHealth>();

                Hit(other);
            }
        }
    }

    protected virtual void Turn() {

        if (m_Target) {
            m_TargetRotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(m_Target.position - transform.position),
                Time.deltaTime * stats.m_TrackSpeed
                );
        }
        else {
            m_TargetRotation = transform.rotation;
        }
    }

    protected virtual void Move() {
        m_TargetPosition = transform.position + ((m_TargetRotation * new Vector3(0, 0, 1)) * stats.m_ProjectileSpeed * Time.deltaTime);
    }

    protected virtual void Hit(BHealth _other) {
        if (stats.m_ExplosionRadius == 0) {
            if (_other) {
                if (_other.transform != m_Instigator) {
                    _other.TakeDamage(stats.m_Damage);
                }
            }
        }

        DestroyProjectile();
    }

    protected virtual void DestroyProjectile() {

        if (stats.m_Debris) {
            Instantiate(stats.m_Debris, transform.position, transform.rotation);
        }

        if (stats.m_ExplosionRadius != 0) {

            foreach (Collider col in Physics.OverlapSphere(transform.position, stats.m_ExplosionRadius, stats.m_CollisionLayers)) {

                BHealth other;

                if (other = col.GetComponent<BHealth>()) {
                    if (other.transform != m_Instigator) {
                        other.TakeDamage(stats.m_Damage);
                    }
                }
            }
        }

        Destroy(gameObject);
    }
}