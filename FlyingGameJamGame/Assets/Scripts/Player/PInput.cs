//  Copyright © Loui Eriksson
//  All Rights Reserved.

using UnityEngine;

public class PInput : MonoBehaviour {

    private PMain m_PMain;

    public Vector2 m_LookInput;
    public Vector3 m_MoveInput;

    public bool m_WeaponWheelInput;

    public bool[] m_FireInput = new bool[2];

    // Start is called before the first frame update
    public void Init(PMain _pMain) {
        m_PMain = _pMain;
    }

    // Update is called once per frame
    private void Update() {

        GetShootInput();

        GetWeaponWheelInput();
        SwitchWeapons();

        GetLookInput();
        GetMoveInput();

        if (Input.GetKeyDown(KeyCode.Escape)) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    private void GetShootInput() {

        for (int i = 0; i < 2; i++) {
            if (m_WeaponWheelInput) {
                m_FireInput[i] = false;
            }
            else {
                m_FireInput[i] = Input.GetMouseButton(i);
            }
        }
    }

    private void GetWeaponWheelInput() {
        m_WeaponWheelInput = Input.GetKey(KeyCode.Tab) || Input.GetMouseButton(3);
    }

    private void SwitchWeapons() {
        if (m_WeaponWheelInput) {
            for (int i = 0; i < 2; i++) {
                if (Input.GetMouseButtonDown(i)) {
                    m_PMain.m_PShoot.ChangeWeapon(i, (int)Mathf.Repeat(m_PMain.m_PShoot.m_CurrWeapons[i] + 1, 5));
                }
            }
        }
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
            Input.GetAxis("Vertical")).normalized * m_PMain.stats.m_MoveSpeed;
    }
}