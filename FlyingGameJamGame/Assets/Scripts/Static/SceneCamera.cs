//  Copyright © Loui Eriksson
//  All Rights Reserved.

using UnityEngine;

public class SceneCamera : MonoBehaviour {

    private static SceneCamera m_Instance;

    public static SceneCamera Instance {
        get {
            if (!m_Instance) {
                GameObject newCamera;

                if (newCamera = GameObject.FindGameObjectWithTag("MainCamera")) {
                    if ((m_Instance = newCamera.GetComponent<SceneCamera>()) == null) {
                        m_Instance = newCamera.AddComponent<SceneCamera>();
                    }

                    m_Instance.camera = newCamera.GetComponent<Camera>();
                }
                else {
                    newCamera = GameObject.Instantiate(new GameObject("Scene Camera"));

                    m_Instance = newCamera.AddComponent<SceneCamera>();
                    m_Instance.camera = newCamera.AddComponent<Camera>();
                }
            }

            return m_Instance;
        }
    }

    public new Camera camera;

    public void LockCursor(CursorLockMode _cursorLockMode) {

        Cursor.lockState = _cursorLockMode;
        Cursor.visible = _cursorLockMode != CursorLockMode.Locked;
    }
}