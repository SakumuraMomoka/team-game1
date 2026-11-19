
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Shoot")]
    [SerializeField] private Transform shootPoint;

    private float shootTimer = 0f;

    [Header("Weapon Data")]
    public WeaponDate weaponDate;

    private void Update()
    {
        // ”­ËŠÔŠu‚ğŒv‘ª
        if (shootTimer > 0f)
        {
            shootTimer -= Time.deltaTime;
        }
    }

    public void SetWeapon(WeaponDate newDate)
    {
        weaponDate = newDate;
        shootTimer = 0f;
    }

    // ”­Ëˆ—
    public void TryShoot(Vector2 direction)
    {
        if (weaponDate == null) return;
        if (weaponDate.tamaPrefab == null) return;
        if (shootPoint == null) return;

        // ”­ËŠÔŠu‚ğŠm”F
        if (shootTimer > 0f) return;

        direction = direction.normalized;

        if (direction.sqrMagnitude == 0f) return;

        // ’e‚ğ¶¬
        GameObject tama = Instantiate(
            weaponDate.tamaPrefab,
            shootPoint.position,
            Quaternion.identity
        );

        TamaController tamaController =
            tama.GetComponent<TamaController>();

        if (tamaController == null)
        {
            Debug.LogError("’e‚ÉTamaController‚ª‚ ‚è‚Ü‚¹‚ñ");
            Destroy(tama);
            return;
        }

        tamaController.SetDirection(direction);

        tamaController.Initialize(
            weaponDate.damage,
            weaponDate.speed,
            weaponDate.duration,
            weaponDate.attenuationValue
        );

        // Ÿ‚Ì”­Ë‚Ü‚Å‚ÌŠÔ
        shootTimer = weaponDate.interval;
    }
}
