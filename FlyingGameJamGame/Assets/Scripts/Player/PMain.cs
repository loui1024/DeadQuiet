﻿//  Copyright © Loui Eriksson
//  All Rights Reserved.

using UnityEngine;

public class PMain : BHealth {

    [HideInInspector] public PlayerParameters stats;

    [HideInInspector] public PInput m_PInput;
    [HideInInspector] public PMove m_PMove;
    [HideInInspector] public PCamera m_PCamera;
    [HideInInspector] public PShoot m_PShoot;
    [HideInInspector] public PUI m_PUI;

    protected override void Awake() {
        Init();

        base.Awake();
    }

    private void Init() {

        stats = PlayerParameters.Instance;

        m_PInput = gameObject.AddComponent<PInput>();
        m_PInput.Init(this);

        m_PMove = gameObject.AddComponent<PMove>();
        m_PMove.Init(this);

        m_PCamera = gameObject.AddComponent<PCamera>();
        m_PCamera.Init(this);

        m_PShoot = gameObject.AddComponent<PShoot>();
        m_PShoot.Init(this);

        m_PUI = gameObject.AddComponent<PUI>();
        m_PUI.Init(this);
    }

    public override void TakeDamage(float _amount) {
        base.TakeDamage(_amount);
    }

    public override void AddHealth(float _amount) {
        base.AddHealth(_amount);
    }

    public override void Die() {

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);

        base.Die();
    }
}