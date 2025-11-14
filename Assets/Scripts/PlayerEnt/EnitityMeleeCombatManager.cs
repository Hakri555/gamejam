
using System.Collections.Generic;
using UnityEngine;

public class EntityMeleeCombatManager : MonoBehaviour
{
    [SerializeField] private GameObject weaponObject;
    private List<GameObject> alreadyHit = new List<GameObject>();

    void Start()
    {
        if (weaponObject != null)
        {
            weaponObject.SetActive(false);
        }
    }

    public void StartAttack(Collider2D other)
    {
        alreadyHit.Clear();
        if (weaponObject != null)
        {
            weaponObject.SetActive(true);
            if (other.CompareTag("enemy"))
            {
                IDamageable damageable = other.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(1f);
                    Debug.Log("Логика melee onhit отработала по: " + other.name);
                }
            }
        }
        EndAttack();
    }

    public void EndAttack()
    {
        if (weaponObject != null)
        {
            weaponObject.SetActive(false);
        }
    }


    public void DestroyHandler()
    {
        StopAllCoroutines();
        // Будьте осторожны с Destroy - это уничтожит весь объект персонажа
        // Destroy(gameObject);
    }
}


