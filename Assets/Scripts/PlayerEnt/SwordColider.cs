using UnityEngine;

public class WeaponColliderHandler : MonoBehaviour
{
    private EntityMeleeCombatManager combatManager;
    
    void Start()
    {
        combatManager = GetComponentInParent<EntityMeleeCombatManager>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (combatManager != null)
        {
            combatManager.StartAttack(other);
        }
    }
}