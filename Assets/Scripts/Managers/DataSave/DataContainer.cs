using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DataContainer
{
    public int metalPlates;
    public int energyCores;
    public int gears;
    public int circuitBoards;
    public bool wBOneUpgOneUnlocked;
    public bool wBOneUpgTwoUnlocked;
    public bool wBOneUpgThreeUnlocked;
    public int volume = 100;
    public int fXVolume = 100;
    public DataContainer(int metalPlates, int energyCores, int gears, int circuitBoards)
    {
        this.metalPlates = metalPlates;
        this.energyCores = energyCores;
        this.gears = gears;
        this.circuitBoards = circuitBoards;
    }
}
