using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private int currentWeaponIndex = 0;

    public WeaponDate[] weaponDates;
    Weapon weapon;
    private Input input;

    private void Awake()
    {
        input = GetComponent<Input>();
        weapon = GetComponent<Weapon>();
    }

    private void Start()
    {
        ChangeWeapon(0);
    }

    private void Update()
    {
        if (input.WeaponChange1)
        {
            ChangeWeapon(0);
        }

        if (input.WeaponChange2)
        {
            ChangeWeapon(1);
        }

        if (input.ShootPressed)//•Ší‚Ìí—Ş‚É‚æ‚Á‚ÄUŒ‚•û–@‚ª•Ï‚í‚é‚æ‚¤‚É’²®‚·‚é
        {
            weapon.NormalShoot();
        }
    }

    public void ChangeWeapon(int index)
    {
        if (index < 0 || index >= weaponDates.Length)
            return;

        currentWeaponIndex = index;

        weapon.SetWeapon(weaponDates[currentWeaponIndex]);

        Debug.Log("‘•”õ‚µ‚½•ŠíF" +
            weaponDates[currentWeaponIndex].weaponName);
    }
}
