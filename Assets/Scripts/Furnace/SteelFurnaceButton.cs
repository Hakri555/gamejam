using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SteelFurnaceButton : MonoBehaviour
{

    [SerializeField] private SteelFurnaceBehaviour furnace;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    
    public void EnableFurnace()
    {
        furnace.gameObject.SetActive(true);
    }
    
}
