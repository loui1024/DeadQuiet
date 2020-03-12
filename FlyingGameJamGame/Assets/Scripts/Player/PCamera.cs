//  Copyright © Loui Eriksson
//  All Rights Reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCamera : MonoBehaviour {

    private PMain m_PMain;

    private Camera m_Camera;
    private Vector2 m_LookValue;
    private Vector3 m_Momentum;

    private Transform m_WeaponsAnchor;

    // Start is called before the first frame update
    public void Init(PMain _pMain) {
        m_PMain = _pMain;

        m_Camera = SceneCamera.Instance.camera;
        m_LookValue = new Vector2(m_Camera.transform.localEulerAngles.y, m_Camera.transform.localEulerAngles.x);

        m_WeaponsAnchor = transform.Find("Weapons");

        SceneCamera.Instance.LockCursor(CursorLockMode.Locked);
    }

    private void Update() {
        Look();
        Move();
    }

    // Update is called once per frame
    private void LateUpdate() {

        SceneCamera.Instance.transform.SetPositionAndRotation(
            transform.position + transform.rotation * (PlayerParameters.Instance.cameraOffset + m_Momentum),
            Quaternion.Euler(new Vector3(m_LookValue.y, m_LookValue.x, 0))
            );

        m_WeaponsAnchor.transform.rotation = SceneCamera.Instance.transform.rotation;
    }

    private void Move() {
        m_Momentum = transform.rotation * m_PMain.m_PMove.m_Velocity / 200;
    }

    private void Look() {

        // Get look input.
        m_LookValue += m_PMain.m_PInput.m_LookInput * Time.deltaTime;

        // Clamp max Y angle.
        m_LookValue.y = Mathf.Clamp(m_LookValue.y, m_PMain.stats.m_MaxY, m_PMain.stats.m_MinY);
    }
}