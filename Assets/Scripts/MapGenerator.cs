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

    [Header("Map Layout Generation")]
    [SerializeField] private int _loops;
    [Range(0f, 10f)][SerializeField] private float _newStructureChances;
    [SerializeField] private Vector2[] _surroundingNodesPositions = new Vector2[4];
    [SerializeField] private int _activeNodes;
    [SerializeField] private int _maxActiveNodes;
    [SerializeField] private int _minActiveNodes;

    void Start()
    {
        InitializeValues();
        Generation();
    }

    private void InitializeValues()
    {
        _mapNodes = new GameObject[_mapSize * _mapSize];
        _activeNodes = 0;

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
        NodeEnumeration();
        NodeGeneration();
        MapDebug();
    }

    private void MapLayout()
    {
        //center is 1
        _mapArray[(int)_roundedHalfMapSize - 1, (int)_roundedHalfMapSize - 1] = 1;
        _activeNodes = 1;
        //select a random node
        while(_activeNodes < _minActiveNodes)
        {
            for(int i = 0; i < _loops; i++)
            {
                if(_activeNodes >= _maxActiveNodes)
                {
                    break;
                }
                AddNodeToMap();
            }   
        }
    }

    private void AddNodeToMap()
    {
        int height = Random.Range(0, _mapSize);
        int widht = Random.Range(0, _mapSize);
        int node = _mapArray[height, widht];
        //if the random node is 1 check its surrounding nodes
        if(node != 0)
        {
            //Up
            if(height != 0)
            {
                _surroundingNodesPositions[0].x = widht;
                _surroundingNodesPositions[0].y = height - 1;
            }
            //Down
            if(height != _mapSize - 1)
            {
                _surroundingNodesPositions[1].x = widht;
                _surroundingNodesPositions[1].y = height + 1;
            }
            //Left
            if(widht != 0)
            {
                _surroundingNodesPositions[2].x = widht - 1;
                _surroundingNodesPositions[2].y = height;
            }
            //Right
            if(widht != _mapSize - 1)
            {
                _surroundingNodesPositions[3].x = widht + 1;
                _surroundingNodesPositions[3].y = height;
            }
            //for each surrounding node check if its empty
            foreach(Vector2 nodePos in _surroundingNodesPositions)
            {
                //if it is empty give the probability of assigning it a 1
                if(_activeNodes >= _maxActiveNodes)
                {
                    break;
                }

                if(_mapArray[(int)nodePos.y, (int)nodePos.x] == 0)
                {
                    int chance = Random.Range(0,100);
                    if(chance <= (_newStructureChances * 10))
                    {
                        _mapArray[(int)nodePos.y, (int)nodePos.x] = 1;
                        _activeNodes++;
                    }
                }
            }
        }
    }

    private void NodeEnumeration()
    {
        //for each node in _mapArray
        for(int i = 0; i < _mapSize; i++)
        {
            for(int e = 0; e < _mapSize; e++)
            {
                //if node value is not 0
                if(_mapArray[i, e] != 0)
                {
                    float code = 0f;
                    //Left
                    if(e != 0)
                    {
                        _surroundingNodesPositions[0].x = e - 1;
                        _surroundingNodesPositions[0].y = i;

                        if(_mapArray[(int)_surroundingNodesPositions[0].y, (int)_surroundingNodesPositions[0].x] > 0)
                        {
                            code += 1f;
                        }
                    }
                    //Up
                    if(i != 0)
                    {
                        _surroundingNodesPositions[1].x = e;
                        _surroundingNodesPositions[1].y = i - 1;
                        if(_mapArray[(int)_surroundingNodesPositions[1].y, (int)_surroundingNodesPositions[1].x] > 0)
                        {
                            code += 0.1f;
                        }
                    }
                    //Right
                    if(e != _mapSize - 1)
                    {
                        _surroundingNodesPositions[2].x = e + 1;
                        _surroundingNodesPositions[2].y = i;
                        if(_mapArray[(int)_surroundingNodesPositions[2].y, (int)_surroundingNodesPositions[2].x] > 0)
                        {
                            code += 0.01f;
                        }
                    }
                    //Down
                    if(i != _mapSize - 1)
                    {
                        _surroundingNodesPositions[3].x = e;
                        _surroundingNodesPositions[3].y = i + 1;
                        if(_mapArray[(int)_surroundingNodesPositions[3].y, (int)_surroundingNodesPositions[3].x] > 0)
                        {
                            code += 0.001f;
                        }
                    }

                    AssignValue(i, e, code);
                }
            }
        }
    }

    private void AssignValue(int i, int e, float code)
    {
        switch(code)
        {
            case 1.111f:
                _mapArray[i, e] = 1;
            break;

            case 1.110f:
                _mapArray[i, e] = 2;
            break;

            case 1.101f:
                _mapArray[i, e] = 3;
            break;

            case 1.100f:
                _mapArray[i, e] = 4;
            break;

            case 1.011f:
                _mapArray[i, e] = 5;
            break;

            case 1.010f:
                _mapArray[i, e] = 6;
            break;

            case 1.001f:
                _mapArray[i, e] = 7;
            break;

            case 1.000f:
                _mapArray[i, e] = 8;
            break;

            case 0.111f:
                _mapArray[i, e] = 9;
            break;

            case 0.110f:
                _mapArray[i, e] = 10;
            break;

            case 0.101f:
                _mapArray[i, e] = 11;
            break;

            case 0.100f:
                _mapArray[i, e] = 12;
            break;

            case 0.011f:
                _mapArray[i, e] = 13;
            break;

            case 0.010f:
                _mapArray[i, e] = 14;
            break;

            case 0.001f:
                _mapArray[i, e] = 15;
            break;

            case 0.000f:
                _mapArray[i, e] = 16;
            break;

            default:
                Debug.LogError("MapGenerator::NodeEnumeration()::AssignValue() no value can be assigned | code: " + code);
                Debug.Log("height: " + i + " width: " + e);
            break;
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
                // newNode.name = "Node: " + i + ", " + e;

                int index = 0;
                if(i != 0)
                {
                    index = (i * _mapSize) + e;
                    _mapNodes[index] = newNode;
                }
                else
                {
                    index = e;
                    _mapNodes[index] = newNode;
                }

                if(_mapArray[i, e] != 0)
                {
                    newNode.GetComponent<MapNode>().InitializeNode(index, _mapVariants[_mapArray[i, e] - 1]);
                }
                else
                {
                    newNode.GetComponent<MapNode>().InitializeNode(index);
                }
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
