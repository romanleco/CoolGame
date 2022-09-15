using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject []_mapNodes;
    [SerializeField] private GameObject _mapNodePrefab;
    [SerializeField] private int[,] _mapArray;
    [SerializeField] private int _mapSize; // must be odd
    private float _roundedHalfMapSize;
    [SerializeField] private float _nodeHeight, _nodeWidth;
    private int _lastNumberGenerated;
    private float _mapCenter;
    private Vector2 _lastNodePos;
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
                        _lastNodePos.x = nodePos.x;
                        _lastNodePos.y = nodePos.y;
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
                    int code = 0;
                    
                    //Left
                    if(e != 0)
                    {
                        _surroundingNodesPositions[0].x = e - 1;
                        _surroundingNodesPositions[0].y = i;

                        if(_mapArray[(int)_surroundingNodesPositions[0].y, (int)_surroundingNodesPositions[0].x] > 0)
                        {
                            code += 1000;
                        }
                    }
                    //Up
                    if(i != 0)
                    {
                        _surroundingNodesPositions[1].x = e;
                        _surroundingNodesPositions[1].y = i - 1;
                        if(_mapArray[(int)_surroundingNodesPositions[1].y, (int)_surroundingNodesPositions[1].x] > 0)
                        {
                            code += 100;
                        }
                    }
                    //Right
                    if(e != _mapSize - 1)
                    {
                        _surroundingNodesPositions[2].x = e + 1;
                        _surroundingNodesPositions[2].y = i;
                        if(_mapArray[(int)_surroundingNodesPositions[2].y, (int)_surroundingNodesPositions[2].x] > 0)
                        {
                            code += 10;
                        }
                    }
                    //Down
                    if(i != _mapSize - 1)
                    {
                        _surroundingNodesPositions[3].x = e;
                        _surroundingNodesPositions[3].y = i + 1;
                        if(_mapArray[(int)_surroundingNodesPositions[3].y, (int)_surroundingNodesPositions[3].x] > 0)
                        {
                            code += 1;
                        }
                    }

                    _mapArray[i, e] = code;
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
                newNode.name = "Node: " + i + ", " + e;

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
                    if(i == _lastNodePos.y && e == _lastNodePos.x)
                    {
                        newNode.GetComponent<MapNode>().InitializeNode(index, (_mapArray[i, e]), false, false, true);
                    }
                    else
                    {
                        if(index == (_mapNodes.Length - 1) / 2)
                        {
                            newNode.GetComponent<MapNode>().InitializeNode(index, (_mapArray[i, e]), true);
                        }
                        else
                        {
                            newNode.GetComponent<MapNode>().InitializeNode(index, (_mapArray[i, e]));
                        }
                    }
                }
                else
                {
                    newNode.GetComponent<MapNode>().InitializeNode(index, (_mapArray[i, e]), false, true);
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
