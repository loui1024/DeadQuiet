//  Copyright © Loui Eriksson
//  All Rights Reserved.

using UnityEngine;

[RequireComponent(typeof(Light))]
public class Flicker : MonoBehaviour {
    [SerializeField] private Light m_Light;

    public float m_FlickerRate;
    public float m_FlickerIntensity;

    private float m_DefaultIntensity;
    private float m_X = 0;

    // Start is called before the first frame update
    private void Start() {

        if (!m_Light) {
            m_Light = GetComponent<Light>();
        }

        m_DefaultIntensity = m_Light.intensity;
    }

    // Update is called once per frame
    private void Update() {
        m_X += Time.deltaTime * m_FlickerRate;

        m_Light.intensity = m_DefaultIntensity + (Mathf.PerlinNoise(m_X, 0) * m_FlickerIntensity);
    }
}