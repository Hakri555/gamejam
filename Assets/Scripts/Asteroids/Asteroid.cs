using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Asteroid : MonoBehaviour
{
    [SerializeField] Sprite ironSprite;
    [SerializeField] Sprite copperSprite;
    [SerializeField] Sprite coalSprite;
    [SerializeField] Sprite adamantiumSprite;
    public GameObject objectToFollow;

    SpriteRenderer spriteRenderer;

    public const int IRON = 0;
    public const int COPPER = 1;
    public const int COAL = 2;
    public const int ADAMANTIUM = 3;
    public const int SMALL = 0;
    public const int MEDIUM = 1;
    public const int LARGE = 2;
    public struct asteroid
    {
        public int type;
        public int oreAmount;
        public float speed;
        public int size;
    }
    public asteroid asteroidInfo;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        objectToFollow = null;
    }
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.enabled = true;
        switch (asteroidInfo.type)
        {
            case IRON:spriteRenderer.sprite = ironSprite; break;
            case COPPER: spriteRenderer.sprite = copperSprite; break;
            case COAL: spriteRenderer.sprite = coalSprite; break;
            case ADAMANTIUM: spriteRenderer.sprite = adamantiumSprite; break;
            default: spriteRenderer.sprite = coalSprite; break;
        }
        
        switch (asteroidInfo.size)
        {
            case SMALL: transform.localScale = new Vector3(.8f, .8f, .8f); break;
            case MEDIUM: transform.localScale = new Vector3(1, 1, 1); break;
            case LARGE: transform.localScale = new Vector3(1.2f, 1.2f, 1.2f); break;
            default : transform.localScale = Vector3.one; break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (objectToFollow == null)
            transform.Translate(Vector2.right * asteroidInfo.speed * Time.deltaTime);
        else
            transform.position = objectToFollow.transform.position;
    }


}
