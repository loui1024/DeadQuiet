//  Copyright © Loui Eriksson
//  All Rights Reserved.

using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MenuButton : MonoBehaviour {

    [SerializeField] private int m_ButtonID;

    private Material m_Material;

    private void Awake() {
        m_Material = this.GetComponent<MeshRenderer>().materials[0];

        m_Material.color = Color.grey;
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            Menu.Instance.MenuButtonClicked(m_ButtonID);
        }
    }

    private void OnMouseEnter() {
        Menu.Instance.SetCameraTarget(transform.position - SceneCamera.Instance.transform.position);

        m_Material.color = Color.green;
    }

    private void OnMouseExit() {
        Menu.Instance.SetCameraTarget(Vector3.forward);

        m_Material.color = Color.grey;
    }
}