//  Copyright © Loui Eriksson
//  All Rights Reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCamera : MonoBehaviour {

    private PMain m_PMain;

    private Camera m_Camera;
    private Vector2 m_LookValue;

    // Start is called before the first frame update
    public void Init(PMain _pMain) {
        m_PMain = _pMain;

        m_Camera = SceneCamera.Instance.camera;
        m_LookValue = new Vector2(m_Camera.transform.localEulerAngles.y, m_Camera.transform.localEulerAngles.x);

        SceneCamera.Instance.LockCursor(CursorLockMode.Locked);
    }

    // Update is called once per frame
    private void Update() {
        m_LookValue += m_PMain.m_PInput.m_LookInput * Time.deltaTime;

        UpdateCameraRotation();
    }

    private void UpdateCameraRotation() {
        m_Camera.transform.rotation = Quaternion.Euler(new Vector3(m_LookValue.y, m_LookValue.x, 0));
    }
}