//  Copyright © Loui Eriksson
//  All Rights Reserved.

using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public static Menu Instance;

    [SerializeField] private GameObject m_Credits;

    private Camera m_MainCamera;
    private Quaternion m_CameraTargetRotation;

    private void Awake() {
        Instance = this;

        m_MainCamera = SceneCamera.Instance.camera;
    }

    private void Start() {

        Time.timeScale = 1f;

        SceneCamera.Instance.LockCursor(CursorLockMode.None);
    }

    private void Update() {
        m_MainCamera.transform.rotation = Quaternion.Slerp(m_MainCamera.transform.rotation, m_CameraTargetRotation, Time.deltaTime * 2.0f);
    }

    public void MenuButtonClicked(int _ButtonID) {

        if (m_Credits.activeSelf == false) {
            m_Credits.SetActive(_ButtonID == 1);

            switch (_ButtonID) {
                case 0: {
                        SceneManager.LoadScene(1);

                        break;
                    }
                case 2: {
                        Application.Quit();

                        break;
                    }
            }
        }
        else {
            m_CameraTargetRotation = Quaternion.LookRotation(Vector3.forward);
        }
    }

    public void SetCameraTarget(Vector3 _direction) {
        if (m_Credits.activeSelf == false) {
            m_CameraTargetRotation = Quaternion.LookRotation(Vector3.Slerp(Vector3.forward, _direction, 0.5f));
        }
        else {
            m_CameraTargetRotation = Quaternion.LookRotation(Vector3.forward);
        }
    }
}