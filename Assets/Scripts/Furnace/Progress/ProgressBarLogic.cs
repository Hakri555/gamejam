using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarLogic : MonoBehaviour
{
    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    // Start is called before the first frame update
    void Start()
    {
        image.fillAmount = 0;
    }

    public void ShowProgress(float seconds)
    {
        StartCoroutine(fill(seconds));
    }

    IEnumerator fill(float time)
    {
        float startingTime = Time.time;
        while (Time.time-startingTime < time) {
            image.fillAmount = (Time.time - startingTime)/time;
            yield return null;
        }
    }
}
