using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerEquipment : MonoBehaviour
{
    [System.Serializable]
    public class WeaponSlot
    {
        public WeaponItemSO weaponData;
        public GameObject weaponObject;
    }

    [Header("Default")]
    [SerializeField] private WeaponItemSO defaultWeapon; // 맨손

    [SerializeField] private List<WeaponSlot> weaponSlots;

    private GameObject currentWeapon;
    private PlayerCombat playerCombat;
    private PlayerController playerController;

    private static WeaponItemSO savedWeapon;

    public GameObject CurrentWeapon => currentWeapon;
    public WeaponItemSO CurrentWeaponData { get; private set; }
    public WeaponItemSO DefaultWeapon => defaultWeapon;

    private void Awake()
    {
        playerCombat = GetComponent<PlayerCombat>();
        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        // 저장된 무기가 있으면 해당 무기 장착.
        // 없을시 기본 무기(주먹) 장착.
        EquipWeapon(savedWeapon != null ? savedWeapon : defaultWeapon);
    }

    public void EquipWeapon(WeaponItemSO weaponData)
    {
        if (weaponData == null)
            weaponData = defaultWeapon;

        CurrentWeaponData = weaponData; // 현재 장착 무기 갱신
        savedWeapon = weaponData;       // 씬 전환 후에도 복원할수 있도록 저장

        // 전부 비활성화
        foreach (var slot in weaponSlots)
        {
            if (slot.weaponObject != null) // 맨손 무기 대응
            {
                slot.weaponObject.SetActive(false);
            }
        }

        foreach (var slot in weaponSlots)
        {
            if (slot.weaponData == weaponData)
            {
                if (slot.weaponObject != null) // 맨손 무기 대응
                {
                    slot.weaponObject.SetActive(true);
                }

                currentWeapon = slot.weaponObject;

                WeaponHitbox hitbox = currentWeapon.GetComponent<WeaponHitbox>();
                if (hitbox != null)
                {
                    hitbox.SetWeaponData(weaponData);
                    playerCombat.SetWeapon(hitbox);
                }

                playerController.SetWeaponType(weaponData.weaponType); //(애니메이션 전환 위해)

                Debug.Log("무기 장착" + weaponData.itemName);

                FindAnyObjectByType<WeaponUI>()?.Refresh();

                EventBus.Publish(new WeaponChangedEvent(CurrentWeaponData));

                return;
            }
        }

        Debug.LogWarning("해당 무기 오브젝트가 연결되지 않음");
    }

    public WeaponItemSO GetCurrentWeaponData()
    {
        foreach(var slot in weaponSlots)
        {
            if (slot.weaponObject.activeSelf)
                return slot.weaponData;
        }
        return null;
    }
}
