using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameInfoDummy : MonoBehaviour
{
    public int coal;
    public int copper;
    public int iron;
    public int adamantium;
    public int steel;
    public int hellishSteel;

    private void Awake()
    {
        coal = 0;
        copper = 0;
        iron = 0;
        adamantium = 0;
        steel = 0;
        hellishSteel = 0;
    }
}
