using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Weapon : MonoBehaviour //武器の攻撃のプログラミングをまとめる 
{
    [Header("Shoot")]
    private float shootTimer;
    [SerializeField] private Transform shootPoint;
    private Vector2 playerDirection;
    [Header("参照")]
    private PlayerInput input;
    private Camera mainCamera;
    public WeaponDate weaponDate;
    [SerializeField] private Transform playerTrans;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        mainCamera = Camera.main;
    }

    private Vector3 GetMouseWorldPosition(float targetZ)//カーソルの位置を求める
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();

        float distance = targetZ - mainCamera.transform.position.z;

        return mainCamera.ScreenToWorldPoint(
            new Vector3(mousePosition.x, mousePosition.y, distance)
        );
    }

    public void SetWeapon(WeaponDate newDate)
    {
        weaponDate = newDate;
    }

    public void NormalShoot()//武器ごとに攻撃方法のプログラミングを作る必要がある。いったんこれだけ
    {
        if (weaponDate == null) return;

        // プレイヤーからカーソルへの方向を取得
        Vector3 worldPosition = GetMouseWorldPosition(transform.position.z);
        playerDirection = worldPosition - playerTrans.position;

        if (shootTimer <= 0f)
        {
            GameObject tama = Instantiate(
                weaponDate.tamaPrefab,
                shootPoint.position,
                Quaternion.identity
            );

            TamaController tamaController = tama.GetComponent<TamaController>();
            tamaController.SetDirection(playerDirection);
            tamaController.Initialize(
                weaponDate.damage,
                weaponDate.speed,
                weaponDate.duration,
                weaponDate.attenuationValue);

            shootTimer = weaponDate.interval;
        }

        shootTimer -= Time.deltaTime;
    }

}
