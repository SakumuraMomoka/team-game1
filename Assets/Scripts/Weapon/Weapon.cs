using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Weapon : MonoBehaviour //武器の攻撃のプログラミングをまとめる 
{
    [Header("Shoot")]
    private float shootTimer = 0f;
    [SerializeField] private Transform shootPoint;
    [Header("参照")]
    private Input input;
    private Camera mainCamera;
    private PlayerController playerController;
    public WeaponDate weaponDate;
    [SerializeField] private Transform playerTrans;

    private void Awake()
    {
        input = GetComponent<Input>();
        mainCamera = Camera.main;
        GameObject player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    public void SetWeapon(WeaponDate newDate)
    {
        weaponDate = newDate;
    }

    public void OnetimeShoot()//単発発射
    {
        if (weaponDate == null) return;
    }

    public void ContinuousShoot()//連続発射
    {
        if (weaponDate == null) return;

        if (shootTimer > 0f)
        {
            shootTimer -= Time.deltaTime;
        }
        else
        {
            shootTimer = 0f;
        }

        if (input.ShootPressed)
        {
            if (shootTimer <= 0f)
            {
                GameObject tama = Instantiate(
                    weaponDate.tamaPrefab,
                    shootPoint.position,
                    Quaternion.identity
                );

                TamaController tamaController = tama.GetComponent<TamaController>();
                tamaController.SetDirection(playerController.playerDirection);
                tamaController.Initialize(
                    weaponDate.damage,
                    weaponDate.speed,
                    weaponDate.duration,
                    weaponDate.attenuationValue);

                shootTimer = weaponDate.interval;
            }
        }
    }

}
