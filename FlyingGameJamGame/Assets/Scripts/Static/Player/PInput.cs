//  Copyright © Loui Eriksson
//  All Rights Reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PInput : MonoBehaviour {

    private PMain m_PMain;

    public Vector2 m_LookInput;
    public Vector3 m_MoveInput;

    // Start is called before the first frame update
    public void Init(PMain _pMain) {
        m_PMain = _pMain;
    }

    // Update is called once per frame
    private void Update() {
        GetLookInput();
        GetMoveInput();
    }

    private void GetLookInput() {
        m_LookInput = new Vector2(
            Input.GetAxis("Mouse X"), 
            Input.GetAxis("Mouse Y")) * m_PMain.stats.m_MouseSensitivity;

        if (m_PMain.stats.m_InvertYAxis) {
            m_LookInput.y *= -1.0f;
        }
    }

    private void GetMoveInput() {
        m_MoveInput = new Vector3(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("ShipVertical"),
            Input.GetAxis("Vertical")) * m_PMain.stats.m_MoveSpeed;
    }
}