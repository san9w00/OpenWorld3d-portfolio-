using UnityEngine;

[CreateAssetMenu(menuName = "Items/Skills/ShieldSkill")]
public class ShieldSkill : WeaponSkillSO
{
    [Header("Shield Settings")]
    public float duration = 1f;
    public float damageMultiplier = 0.33f;

    public override void UseSkill(GameObject user)
    {
        PlayerStatus status = user.GetComponent<PlayerStatus>();

        if (status == null) return;

        MonoBehaviour runner = user.GetComponent<InputHandler>();

        // 데미지 감소 적용
        status.ApplyDamageMultiplier(damageMultiplier, duration, runner);

        // VFX 호출
        EventBus.Publish(new VFXEvent(
            status.transform.position,
            VFXActionType.Skill,
            VFXSwordType.Origin
        ));

        Debug.Log("쉴드 스킬 사용!");
    }
}
