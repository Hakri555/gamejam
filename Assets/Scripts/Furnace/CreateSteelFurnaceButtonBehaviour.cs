using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSteelFurnaceButtonBehaviour : MonoBehaviour
{
    [SerializeField] GameInfoDummy resourses;
    [SerializeField] SteelFurnaceBehaviour sf;
    [SerializeField] GameObject sfButton;
    [SerializeField] GameObject progreassBarBackground;
    [SerializeField] GameObject progressBarForeground; 
    public void ConstructFurnace()
    {
        if (resourses.copper >= 15 && resourses.iron >= 20)
        {
            gameObject.SetActive(false);
            resourses.copper -= 15;
            resourses.iron -= 20;
            sf.gameObject.SetActive(true);
            sfButton.gameObject.SetActive(true);
            progreassBarBackground.SetActive(true);
            progressBarForeground.SetActive(true);
        }
    }
}
