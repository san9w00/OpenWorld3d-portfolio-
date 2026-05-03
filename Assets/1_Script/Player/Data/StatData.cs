using UnityEngine;

public readonly struct StatData
{
    public readonly float Current;
    public readonly float Max;

    // 현재 비율 계산
    public float Normalized => Max <= 0 ? 0 : Current / Max;

    public StatData(float current, float max)
    {
        Current = current;
        Max = max;
    }


    // 레벨 UI 표시용 데이터
    public readonly struct LevelUIData
    {
        public readonly int Level;
        public readonly int CurrentExp;
        public readonly int RequiredExp;

        public LevelUIData(int level, int currentExp, int requiredExp)
        {
            Level = level;
            CurrentExp = currentExp;
            RequiredExp = requiredExp;
        }
    }
}
