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

    public void getInfiniteResourses()
    {
        coal = int.MaxValue; copper = int.MaxValue; iron = int.MaxValue; adamantium = int.MaxValue; steel = int.MaxValue; hellishSteel = int.MaxValue;
    }

    public int getResourseByType(int type)
    {
        switch (type)
        {
            case 0:
                return iron;
            case 1:
                return copper;
            case 2:
                return coal;
            case 3:
                return adamantium;
            case 4:
                return steel;
            case 5:
                return hellishSteel;
        }
        return 0;
    }

    public bool reduceResourseByType(int type, int ammount)
    {
        if (type < 0 || type > 5 || ammount < 0) {
            return false;
        }
        switch (type)
        {
            case 0:
                if (iron >= ammount)
                {
                    iron -= ammount;
                    return true;
                }
                else
                {
                    return false;
                }
            case 1:
                if (copper >= ammount)
                {
                    copper -= ammount;
                    return true;
                }
                else
                {
                    return false;
                }
            case 2:
                if (coal >= ammount)
                {
                    coal -= ammount;
                    return true;
                }
                else
                {
                    return false;
                }
            case 3:
                if (adamantium >= ammount)
                {
                    adamantium -= ammount;
                    return true;
                }
                else
                {
                    return false;
                }
            case 4:
                if (steel >= ammount)
                {
                    steel -= ammount;
                    return true;
                }
                else
                {
                    return false;
                }
            case 5:
                if (hellishSteel >= ammount)
                {
                    hellishSteel -= ammount;
                    return true;
                }
                else
                {
                    return false;
                }
            default:
                return false;
        }
    }
}
