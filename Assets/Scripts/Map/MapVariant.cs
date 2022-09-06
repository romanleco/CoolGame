using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapVariant : MonoBehaviour
{
    [SerializeField] private GameObject[] _entranceBlockers = new GameObject[4];
    private GameObject[] _inactiveBlockers = new GameObject[4];
    [SerializeField] private GameObject[] _enemySpawners = new GameObject[5];
    [SerializeField] private bool _isCenter;
    void Start()
    {
        _entranceBlockers[0] = this.transform.Find("EntranceBlockLeft").gameObject;
        _entranceBlockers[1] = this.transform.Find("EntranceBlockUp").gameObject;
        _entranceBlockers[2] = this.transform.Find("EntranceBlockRight").gameObject;
        _entranceBlockers[3] = this.transform.Find("EntranceBlockDown").gameObject;

        for(int i = 0; i < _entranceBlockers.Length; i++)
        {
            if(_entranceBlockers[i].activeSelf == false)
            {
                _inactiveBlockers[i] = _entranceBlockers[i];
            }
        }

        PopulateEnemySpawns();
        SpawnEnemies();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!_isCenter)
        {    
            if(other.tag == "Player")
            {
                ManageEntrances(true);
                StartCoroutine("Open");
            }
        }
        else
        {
            Destroy(gameObject.GetComponent<BoxCollider2D>());
        }
    }

    private void PopulateEnemySpawns()
    {
        _enemySpawners[0] = this.transform.Find("EnemySpawn").gameObject;
        _enemySpawners[1] = this.transform.Find("EnemySpawn (1)").gameObject;
        _enemySpawners[2] = this.transform.Find("EnemySpawn (2)").gameObject;
        _enemySpawners[3] = this.transform.Find("EnemySpawn (3)").gameObject;
        _enemySpawners[4] = this.transform.Find("EnemySpawn (4)").gameObject;
    }

    private void SpawnEnemies()
    {
        MapNode parentNode = transform.parent.GetComponent<MapNode>();
        if(!_isCenter)
        {
            foreach(GameObject o in _enemySpawners)
            {
                int entNum = Random.Range(-1, 4);
                for(int i = 0; i < entNum; i ++)
                {
                    // Instantiate(parentNode.possibleEnemies[Random.Range(0, parentNode.possibleEnemies.Length + 1)], o.transform.position, Quaternion.identity);
                    Instantiate(parentNode.possibleEnemies[0], o.transform.position, Quaternion.identity);
                }
            }
        }
    }

    private void ManageEntrances(bool close)
    {
        foreach(GameObject o in _inactiveBlockers)
        {
            if(o != null)
            {
                if(o.activeSelf == false && close)
                {
                    o.SetActive(true);
                }
                else if(!close)
                {
                    o.SetActive(false);
                }
            }
        }
    }

    IEnumerator Open()
    {
        yield return new WaitForSeconds(2);
        ManageEntrances(false);
    }

    public void SetIsCenter(bool TorF)
    {
        _isCenter = TorF;
    }
}
