using UnityEngine;

public class Input : MonoBehaviour
{
    private InputSystem_Actions input;

    //player
    public Vector2 MoveInput {  get; private set; }
    public bool ShootPressed { get; private set; }
    public bool AvoidPressed { get; private set; }
    public bool DashPressed { get; private set; }

    //UI
    public bool WeaponChange1 { get; private set; }
    public bool WeaponChange2 { get; private set; }


    private void Awake()
    {
        input = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Update()
    {
        MoveInput = input.Player.Move.ReadValue<Vector2>();
        ShootPressed = input.Player.Shoot.IsPressed();
        AvoidPressed = input.Player.Avoid.WasPressedThisFrame();
        DashPressed = input.Player.Dash.IsPressed();

        WeaponChange1 = input.UI.WeaponChange1.WasPressedThisFrame();
        WeaponChange2 = input.UI.WeaponChange2.WasPressedThisFrame();
    }
}
