using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [System.Serializable]
    public class WeaponSlot
    {
        public WeaponItemSO weaponData;
        public GameObject weaponObject;
    }

    [SerializeField] private List<WeaponSlot> weaponSlots;

    private GameObject currentWeapon;

    public void EquipWeapon(WeaponItemSO weaponData)
    {
        // 전부 비활성화
        foreach(var slot in weaponSlots)
        {
            slot.weaponObject.SetActive(false);
        }

        foreach(var slot in weaponSlots)
        {
            if(slot.weaponData == weaponData)
            {
                slot.weaponObject.SetActive(true);
                currentWeapon = slot.weaponObject;
                Debug.Log("무기 장착" + weaponData.itemName);
                return;
            }
        }

        Debug.LogWarning("해당 무기 오브젝트가 연결되지 않음");
    }
}
