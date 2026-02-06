using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // Current Stats
    private float curHP;
    private float curStemina;
    private float curMoveSpeed;
    private float curAtkDamage;
    private float curJumpPower;

    private PlayerData data;

    private void Awake()
    {
        curHP = data.maxHP;
        curStemina = data.maxStemina;
        curMoveSpeed = data.moveSpeed;
        curAtkDamage = data.atkDamage;
        curJumpPower = data.jumpPower;
    }
}
