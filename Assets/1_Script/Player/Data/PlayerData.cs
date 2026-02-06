using UnityEngine;


[CreateAssetMenu(menuName = "Data/PlayerStats")]
public class PlayerData : ScriptableObject
{
    public float maxHP;
    public float maxStemina;
    public float moveSpeed;
    public float atkDamage;
    public float jumpPower;
}
