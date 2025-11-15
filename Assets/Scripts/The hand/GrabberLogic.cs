using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrabberLogic : MonoBehaviour
{
    public bool isShooting;
    private Vector2 moveVector;
    private Vector2 startingPos;
    private Asteroid catchedAsteroid;
    [SerializeField] UpgradeState upgradeState;
    [SerializeField] GameInfoDummy gameInfoDummy;
    private void Awake()
    {
        isShooting = false;
        catchedAsteroid = null;
    }
    private void Update()
    {
        if (isShooting) {
            transform.position = Vector2.MoveTowards(transform.position, moveVector, upgradeState.clawSpeed * Time.deltaTime);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(shootOut());
        }
    }

    IEnumerator shootOut()
    {
        startingPos = transform.position;
        isShooting = true;
        moveVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        moveVector = startingPos + (moveVector - startingPos).normalized * upgradeState.clawReach;
        while ((Vector2)transform.position != moveVector && catchedAsteroid == null && transform.position.x > 2 && transform.position.x < 8.8f && transform.position.y < 5 && transform.position.y > -3) yield return null;
        moveVector = startingPos;
        while ((Vector2)transform.position != moveVector) yield return null;
        transform.position = moveVector;
        if (catchedAsteroid != null)
        {
            switch (catchedAsteroid.asteroidInfo.type)
            {
                case Asteroid.IRON:
                    gameInfoDummy.iron += catchedAsteroid.asteroidInfo.oreAmount;
                    break;
                case Asteroid.COPPER:
                    gameInfoDummy.copper += catchedAsteroid.asteroidInfo.oreAmount;
                    break;
                case Asteroid.ADAMANTIUM:
                    gameInfoDummy.adamantium += catchedAsteroid.asteroidInfo.oreAmount;
                    break;
                case Asteroid.COAL:
                    gameInfoDummy.coal += catchedAsteroid.asteroidInfo.oreAmount;
                    break;
                default:
                    Debug.LogWarning("Unexpected meteorite type!");
                    break;
            }
            Destroy(catchedAsteroid.gameObject);
            catchedAsteroid = null;
        }
        isShooting = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Asteroid asteroid) && catchedAsteroid == null) {
            asteroid.objectToFollow = this.gameObject;
            catchedAsteroid = asteroid;
        }
    }
}
