using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private Image weaponImage;
    [SerializeField] private Image skillImage;
    [SerializeField] private Image cooldownOverlay;

    private PlayerEquipment equipment;
    private WeaponItemSO currentWeapon;
    private WeaponSkillSO currentSkill;
    private GameObject player;

    private void Awake()
    {
        equipment = FindAnyObjectByType<PlayerEquipment>();

        if (equipment != null)
        {
            player = equipment.gameObject;
        }
    }

    private void Update()
    {
        UpdateCooldown();
    }

    public void Refresh()
    {
        currentWeapon = equipment.GetCurrentWeaponData();

        if (currentWeapon == null)
        {
            weaponImage.enabled = false;
            skillImage.enabled = false;
            cooldownOverlay.fillAmount = 0;
            return;
        }

        weaponImage.enabled = true;
        weaponImage.sprite = currentWeapon.itemIcon;

        currentSkill = currentWeapon.skill;

        if(currentSkill == null)
        {
            skillImage.enabled = false;
            cooldownOverlay.fillAmount = 0;
            return;
        }

        skillImage.enabled = true;
        skillImage.sprite = currentSkill.skillIcon;
    }

    private void UpdateCooldown()
    {
        if(currentSkill == null)
        {
            cooldownOverlay.fillAmount = 0;
            return;
        }

        float remain = currentSkill.GetRemainingCooldown(player);
        float ratio = remain / currentSkill.cooldown;

        cooldownOverlay.fillAmount = ratio;
    }
}
