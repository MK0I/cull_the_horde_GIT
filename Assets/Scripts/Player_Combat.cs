using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Combat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform aimMarker;
    [SerializeField] private GameObject magicBoltPrefab;
    [SerializeField] private Player_Animation playerAnimation;

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
        if (controls.Player.Fire.WasPressedThisFrame())
        {
            Fire();
        }
    }

    private void Fire()
    {
        Vector2 mouseScreen = controls.Player.Aim.ReadValue<Vector2>();

        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0f;

        Vector2 direction = (mouseWorld - aimMarker.position).normalized;

        GameObject bolt = Instantiate(
            magicBoltPrefab,
            aimMarker.position,
            Quaternion.identity);

        bolt.GetComponent<Magic_Bolt>().Initialize(direction);

        if (playerAnimation != null)
        {
            playerAnimation.PlayCast();
        }

    }

}