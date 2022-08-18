using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    [SerializeField] private int _index;
    public void InitializeNode(int index, GameObject mapVariation = null)
    {
        _index = index;

        if(mapVariation != null)
        {
            GameObject mapVar = Instantiate(mapVariation, transform.position, Quaternion.identity);
            mapVar.transform.parent = gameObject.transform;
        }
    }
}
