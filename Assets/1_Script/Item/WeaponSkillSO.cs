using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponSkillSO : ScriptableObject
{
    [Header("Skill Icon")]
    public Sprite skillIcon;

    [Header("Cooldown")]
    public float cooldown = 10f;

    private Dictionary<GameObject, float> _lastUsedTime = new Dictionary<GameObject, float>();

    public bool CanUse(GameObject user)
    {
        if (!_lastUsedTime.ContainsKey(user))
            return true;

        return Time.time >= _lastUsedTime[user] + cooldown;
    }

    public void TryUse(GameObject user)
    {
        if (!CanUse(user))
        {
            float coolTimeDebug = (_lastUsedTime[user] + cooldown) - Time.time;
            Debug.Log($"{name} 스킬 쿨타임 남음: {coolTimeDebug:F1}초");

            return;
        }

        _lastUsedTime[user] = Time.time;

        Debug.Log($"{name} 스킬 사용!");

        UseSkill(user);
    }

    public float GetRemainingCooldown(GameObject user)
    {
        if (!_lastUsedTime.ContainsKey(user))
            return 0f;

        float remain = (_lastUsedTime[user] + cooldown) - Time.time;
        return Mathf.Max(0, remain);
    }

    public abstract void UseSkill(GameObject user);

#if UNITY_EDITOR
    public virtual void DrawGizmo(Vector3 position)
    {
        // 기본은 아무것도 안 그림
    }
#endif
}
