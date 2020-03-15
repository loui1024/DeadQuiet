//  Copyright © Loui Eriksson
//  All Rights Reserved.

using System.Collections;
using UnityEngine;

public class Mothership : MonoBehaviour {

    [HideInInspector] private BHealth m_Player;

    public BHealth[] m_Weakpoints;
    public float m_TimeRemaining = 180.0f;

    [SerializeField] private GameObject m_Debris;

    private bool m_CoroutineRunning = false;

    private void LateUpdate() {

        bool destroyed = true;

        for (int i = 0; i < m_Weakpoints.Length; i++) {
            if (m_Weakpoints[i] != null) {
                destroyed = false;
            }
        }

        if (destroyed) {
            if (!m_CoroutineRunning) {
                StartCoroutine(Die());
            }
        }
        else {

            m_TimeRemaining -= Time.deltaTime;

            if (m_TimeRemaining <= 0) {
                m_Player.Die();
            }
        }
    }

    public IEnumerator Die() {

        m_CoroutineRunning = true;

        PCamera camera = FindObjectOfType<PCamera>();

        Time.timeScale = 0.3f;

        yield return new WaitForSecondsRealtime(1.0f);

        Instantiate(m_Debris, transform.position, transform.rotation);

        yield return new WaitForSecondsRealtime(1.0f);

        StartCoroutine(camera.Shake(60.0f, 4.0f, 3f));

        yield return new WaitForSecondsRealtime(1.0f);

        Time.timeScale = 1f;

        while (transform.position.y > -2000) {
            transform.position += new Vector3(0.0f, -1.0f, -0.77f) * 300.0f * Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSecondsRealtime(1.0f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);

        m_CoroutineRunning = false;
    }
}