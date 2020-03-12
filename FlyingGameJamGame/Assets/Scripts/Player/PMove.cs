//  Copyright © Loui Eriksson
//  All Rights Reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMove : MonoBehaviour {

    private PMain m_PMain;

    public Vector3 m_Velocity;

    private Quaternion m_TargetRotation;
    private Vector3 m_TargetPosition;
    private float m_TiltAmount;

    // Start is called before the first frame update
    public void Init(PMain _pMain) {
        m_PMain = _pMain;
    }

    // Update is called once per frame
    private void Update() {

        Turn();
        Move();

        transform.SetPositionAndRotation(
            m_TargetPosition,
            Quaternion.Slerp(transform.rotation, m_TargetRotation, Time.deltaTime * m_PMain.stats.m_TurnSpeed)
            );
    }

    private void Turn() {
        Vector3 eulerAngles = Quaternion.LookRotation(SceneCamera.Instance.transform.forward).eulerAngles;
        eulerAngles.z = Mathf.SmoothStep(eulerAngles.z, 200.0f * -m_PMain.m_PInput.m_MoveInput.x, Time.deltaTime * 2.0f);

        m_TargetRotation = Quaternion.Euler(eulerAngles);
    }

    private void Move() {
        m_Velocity += m_TargetRotation * m_PMain.m_PInput.m_MoveInput * Time.deltaTime;
        m_TargetPosition = transform.position + m_Velocity;

        m_Velocity *= 1.0f - Mathf.Clamp01(m_PMain.stats.m_SlowDownRate);
    }
}