using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BProjectile : MonoBehaviour {

    public ProjectileWeapon stats;

    protected Vector3 m_TargetPosition;
    protected Quaternion m_TargetRotation;

    private float m_Lifetime;
    private Vector3 m_ConstantVelocity;

    // Start is called before the first frame update
    public virtual void Init(ProjectileWeapon _stats, Vector3 _inheritedVelocity) {

        stats = _stats;

        m_TargetPosition = transform.position;
        m_TargetRotation = transform.rotation;

        m_ConstantVelocity = _inheritedVelocity;
    }

    // Update is called once per frame
    void Update() {
        if (m_Lifetime <= stats.m_Lifetime)
        {

            Turn();
            Move();

            transform.SetPositionAndRotation(m_TargetPosition, m_TargetRotation);

            m_Lifetime += Time.deltaTime;
        }
        else {
            Hit(null);
        }
    }

    protected virtual void Turn() {
        m_TargetRotation = transform.rotation;
    }

    protected virtual void Move() {
        m_TargetPosition = transform.position + ((m_TargetRotation * new Vector3(0, 0, 1)) * stats.m_ProjectileSpeed * Time.deltaTime) + (m_ConstantVelocity * Time.deltaTime);
    }

    protected virtual void Hit(BHealth _other) {
        if (_other) {
            _other.TakeDamage(stats.m_Damage);
        }

        DestroyProjectile();
    }

    protected virtual void DestroyProjectile() {
        Destroy(gameObject);
    }
}
