using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Aim : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform aimMarker;

    [Header("Settings")]
    [SerializeField] private float orbitRadius = 1f;

    [SerializeField] private Vector2 orbitCenterOffset = new Vector2(0f, 1f);

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

        Vector3 orbitCenter = transform.position + (Vector3)orbitCenterOffset;

        Vector2 direction = (mouseWorld - orbitCenter).normalized;

        aimMarker.position = orbitCenter + (Vector3)(direction * orbitRadius);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        aimMarker.rotation = Quaternion.Euler(0f, 0f, angle);

    }

}