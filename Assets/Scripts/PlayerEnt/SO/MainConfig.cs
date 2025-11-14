
using UnityEngine;

public class bossMainConfig : ScriptableObject
{
    [CreateAssetMenu(fileName = "BossMainConfig", menuName = "Configs/Boss/Main")]
    public class BossMainConfig : ScriptableObject
    {
        [SerializeField] private entBasicStatsConfig _statsConfig;
        [SerializeField] private bossMovementConfig _movementConfig;
        [SerializeField] private bossAttackConfig _attackConfig;

        public entBasicStatsConfig Stats => _statsConfig;
        public bossMovementConfig Movement => _movementConfig;
        public bossAttackConfig Attack => _attackConfig;

        private void OnValidate()
        {
            if (_statsConfig == null || _movementConfig == null || _attackConfig == null)
                Debug.LogError("Один из конфигов Ent не был создан");
        }
    }
}
