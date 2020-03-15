//  Copyright © Loui Eriksson
//  All Rights Reserved.

using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour {

    public static MusicManager Instance;

    [SerializeField] private AudioSource m_AudioSource;

    [SerializeField] private AudioClip m_Intro;
    [SerializeField] private AudioClip m_Loop;

    [SerializeField] private AudioLowPassFilter m_LowPassFilter;

    public float m_Intensity;

    private void Awake() {
        Instance = this;

        m_AudioSource.clip = m_Intro;
        m_AudioSource.Play();
    }

    private void Update() {
        if (m_AudioSource.isPlaying == false) {
            m_AudioSource.clip = m_Loop;
            m_AudioSource.loop = true;

            m_AudioSource.Play();
        }

        m_LowPassFilter.cutoffFrequency = Mathf.Lerp(
            m_LowPassFilter.cutoffFrequency,
            Mathf.Clamp(1.0f - (m_Intensity), 0.1f, 1.0f) * 22000.0f,
            Time.deltaTime * 3.0f
            );

        m_Intensity = Mathf.Lerp(m_Intensity, 0, Time.deltaTime * 0.25f);
    }
}