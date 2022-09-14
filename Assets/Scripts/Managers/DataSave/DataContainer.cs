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
    public DataContainer(int metalPlates, int energyCores, int gears, int circuitBoards)
    {
        this.metalPlates = metalPlates;
        this.energyCores = energyCores;
        this.gears = gears;
        this.circuitBoards = circuitBoards;
    }
}
