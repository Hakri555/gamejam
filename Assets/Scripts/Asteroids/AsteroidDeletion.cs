using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AsteroidDeletion : MonoBehaviour
{
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Asteroid asteroid))
        {
            Destroy(asteroid.gameObject);
        }
    }
}
