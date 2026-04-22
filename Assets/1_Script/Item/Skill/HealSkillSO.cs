using UnityEngine;

[CreateAssetMenu(menuName = "Items/Skills/HealSkill")]
public class HealSkillSO : WeaponSkillSO
{
    public GameObject healZonePrefab;

    public override void UseSkill(GameObject user)
    {
        if (healZonePrefab == null)
        {
            Debug.LogWarning("HealZone 프리팹이 없음!");
            return;
        }

        GameObject zone = Instantiate(
            healZonePrefab,
            user.transform.position,
            Quaternion.identity
        );

        EventBus.Publish(new VFXEvent(user.transform.position, VFXActionType.Skill, VFXSwordType.PeaceHeal));

        HealZone hz = zone.GetComponent<HealZone>();

        if (hz != null)
        {
            hz.Init(user);
        }
    }
}
