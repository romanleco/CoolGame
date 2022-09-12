using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    [SerializeField] private int _index;
    [SerializeField] public GameObject[] possibleEnemies;
    [SerializeField] private GameObject[] _mapVariations;
    public void InitializeNode(int index, int code, bool isCenterNode = false, bool emptyNode = false)
    {
        _index = index;

        if(!emptyNode)
        {
            GameObject mapVar = Instantiate(_mapVariations[0], transform.position, Quaternion.identity);
            MapVariant mapVarScr = mapVar.GetComponent<MapVariant>();
            mapVarScr.SetIsCenter(isCenterNode);
            mapVarScr.SetCode(code);
            mapVarScr.AssignValue();
            mapVar.transform.parent = gameObject.transform;
        }
    }
}
