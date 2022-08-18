using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject []_mapNodes;
    [SerializeField] private GameObject _mapNodePrefab;
    [SerializeField] private GameObject []_mapVariants;
    [SerializeField] private int[,] _mapArray;
    [SerializeField] private int _mapSize; // must be odd
    private float _roundedHalfMapSize;
    [SerializeField] private float _nodeHeight, _nodeWidth;
    private int _lastNumberGenerated;
    private float _mapCenter;
    private Vector2 _topLeftNodeCoords;
    private Vector2 _nextNodeCoords;

    void Start()
    {
        InitializeValues();
        Generation();
    }

    private void InitializeValues()
    {
        _mapNodes = new GameObject[_mapSize * _mapSize];

        _mapArray = new int[_mapSize, _mapSize];
        _roundedHalfMapSize = Mathf.Ceil((float)_mapSize / 2);
        _mapCenter = _roundedHalfMapSize * _roundedHalfMapSize;

        _topLeftNodeCoords.y = _nodeHeight * (_roundedHalfMapSize - 1);
        _topLeftNodeCoords.x = -_nodeWidth * _roundedHalfMapSize;

        _nextNodeCoords = _topLeftNodeCoords;
    }

    private void Generation()
    {
        MapLayout();
        NodeGeneration();
        MapDebug();
    }

    private void MapLayout()
    {
        //generate a multidimensional array with random numbers that specify what node should be generated
        for(int i = 0; i < _mapSize; i++)
        {
            for(int e = 0; e < _mapSize; e++)
            {
                if(i != _roundedHalfMapSize - 1 && e != _roundedHalfMapSize - 1)
                {
                    _mapArray[i, e] = Random.Range(0, 2);
                }
                else
                {
                    _mapArray[i, e] = 1;
                }
            }
        }
    }

    private void NodeGeneration()
    {
        //generate nodes
        for(int i = 0; i < _mapSize; i++)
        {
            for(int e = 0; e < _mapSize; e++)
            {
                _nextNodeCoords.x += _nodeWidth;
                GameObject newNode = Instantiate(_mapNodePrefab, _nextNodeCoords, Quaternion.identity);
                // _mapNodes[(i * e) - 1] = newNode;
                // if(_mapArray[i, e] > 0)
                // {
                //     Instantiate(_mapVariants[_mapArray[i, e] - 1], newNode.transform.position, Quaternion.identity);
                // }
            }
            _nextNodeCoords.y -= _nodeHeight;
            _nextNodeCoords.x = _topLeftNodeCoords.x;
        }
    }

    private void MapDebug()
    {
        //checking map
        string map = "";
        for(int i = 0; i < _mapSize; i++)
        {
            for(int e = 0; e < _mapSize; e++)
            {
                map += "[" + _mapArray[i, e].ToString() + "] ";
            }
            map += "\n";
        }
        Debug.Log(map);
    }
}
