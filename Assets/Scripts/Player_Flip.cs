using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Flip : MonoBehaviour
{
    private Camera mainCamera;
    private PlayerControls controls;

    private void Awake()
    {
        mainCamera = Camera.main;
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Update()
    {
        Vector2 mouseScreen = controls.Player.Aim.ReadValue<Vector2>();

        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0f;

        Vector3 scale = transform.localScale;

        scale.x = mouseWorld.x >= transform.parent.position.x
            ? Mathf.Abs(scale.x)
            : -Mathf.Abs(scale.x);

        transform.localScale = scale;
    }

}