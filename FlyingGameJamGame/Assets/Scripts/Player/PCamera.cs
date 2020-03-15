//  Copyright © Loui Eriksson
//  All Rights Reserved.

using System.Collections;
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

    public IEnumerator Shake(float magnitude = 1.0f, float frequency = 10.0f, float duration = 1.0f) {

        float time = 0.0f;
        float progress = 0.0f;
        float modifier = 0.0f;

        Vector3 shake = new Vector3();

        float offsetX = Random.Range(0.0f, magnitude);
        float offsetY = Random.Range(0.0f, magnitude);

        while (time < duration) {
            progress = (time / duration);
            modifier = 1.0f - progress;

            float valX = Mathf.PerlinNoise((time * frequency) + offsetX, 0.5f);
            float valY = Mathf.PerlinNoise(0.5f, (time * frequency) + offsetY);

            shake = new Vector3(
                (valX - 0.33f) * 1.0f,
                (valY - 0.5f) * 1.0f,
                0);

            m_LookValue += (Vector2)shake * modifier;

            time += Time.deltaTime;

            yield return null;
        }
    }
}