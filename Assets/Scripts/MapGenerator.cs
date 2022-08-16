using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject []_mapNodes;
    [SerializeField] private int[,] _mapArray;
    [SerializeField] private int _heightLevel, _widthLevel;
    private int _lastNumberGenerated;

    void Start()
    {
        _mapArray = new int[_heightLevel, _widthLevel];
        Generation();
    }

    private void Generation()
    {
        Instantiate(_mapNodes[0], transform.position, Quaternion.identity);
        //generate a multidimensional array with random numbers that specify what node should be generated
        for(int i = 0; i > _heightLevel; i++)
        {
            for(int e = 0; e > _widthLevel; i++)
            {
                
            }
        }
    }
}
