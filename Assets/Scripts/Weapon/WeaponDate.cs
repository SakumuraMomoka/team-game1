using UnityEngine;

[CreateAssetMenu(menuName = "Date/WeaponDate")]
public class WeaponDate:ScriptableObject
{
    public string weaponName;

    public float damage;
    public float speed;
    public int amount;//一度に発射される玉の数
    public float duration;//持続時間
    public float interval;//クールタイム（発射間隔）
    public int tamaType;//弾の種類
    public float knockbackValue;//敵がノックバックする値
    public float attenuationValue;//距離減衰

    public GameObject tamaPrefab;
}
