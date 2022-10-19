using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapVariant : MonoBehaviour
{
    [SerializeField] private GameObject[] _entranceBlockers = new GameObject[2];
    private GameObject[] _dynamicBlockers = new GameObject[2];
    [SerializeField] private GameObject[] _enemySpawners = new GameObject[6];
    [SerializeField] private bool _isCenter;
    private bool _isEndNode;
    public int _code;
    [SerializeField] private List<EnemyWalker> _enemiesAlive = new List<EnemyWalker>();
    [SerializeField] private bool _right, _up, _left, _down; //which directions have an active node
    private bool _cleared;
    [SerializeField] private GameObject[] _stairs;
    [SerializeField] private GameObject _returnPortal;
    [SerializeField] private GameObject[] _backgrounds = new GameObject[6];
    [SerializeField] private SpriteRenderer[] _stateLights;
    void Start()
    {
        PopulateEnemySpawns();
        SpawnEnemies();

        _entranceBlockers[0] = this.transform.Find("EntranceBlockLeft").gameObject;
        _entranceBlockers[1] = this.transform.Find("EntranceBlockRight").gameObject;

        if(_left)
        {
            _entranceBlockers[0].SetActive(false);
        }

        if(_right)
        {
            _entranceBlockers[1].SetActive(false);
        }

        if(!_up)
        {
            _stairs[0].SetActive(false);
        }

        if(!_down)
        {
            _stairs[1].SetActive(false);
        }

        for(int i = 0; i < _entranceBlockers.Length; i++)
        {
            if(_entranceBlockers[i].activeSelf == false)
            {
                _dynamicBlockers[i] = _entranceBlockers[i];
            }
        }

        if(_isCenter)
        {
            foreach(GameObject s in _stairs)
            {
                if(s.activeSelf)
                s.GetComponent<Stairs>().SetFunctional(true);
            }

            foreach(SpriteRenderer spr in _stateLights)
            {
                spr.color = Color.green;
            }
        }

        if(_isEndNode)
        {
            _returnPortal.SetActive(true);
        }

        _backgrounds[Random.Range(0, _backgrounds.Length)].SetActive(true);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!_isCenter)
        {    
            if(other.tag == "Player" && !_cleared)
            {
                ManageEntrances(true);
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
        _enemySpawners[5] = this.transform.Find("EnemySpawn (5)").gameObject;
    }

    private void SpawnEnemies()
    {
        MapNode parentNode = transform.parent.GetComponent<MapNode>();
        if(!_isCenter)
        {
            foreach(GameObject o in _enemySpawners)
            {
                int entNum = Random.Range(0, 4);
                for(int i = 0; i < entNum; i ++)
                {
                    GameObject newEnemy = Instantiate(parentNode.possibleEnemies[Random.Range(0, parentNode.possibleEnemies.Length)], o.transform.position, Quaternion.identity);
                    newEnemy.transform.parent = o.transform;
                    _enemiesAlive.Add(newEnemy.GetComponent<EnemyWalker>());
                }
            }
        }
    }

    public void CheckRoomClear(EnemyWalker enemy)
    {
        _enemiesAlive.Remove(enemy);
        if(_enemiesAlive.Count <= 0)
        {   
            _cleared = true;
            foreach(GameObject s in _stairs)
            {
                if(s.activeSelf)
                s.GetComponent<Stairs>().SetFunctional(true);
            }
            _returnPortal.GetComponent<Teleporter>().SetFunctional();

            foreach(SpriteRenderer spr in _stateLights)
            {
                spr.color = Color.green;
            }

            StartCoroutine("Open");
        }
    }

    private void ManageEntrances(bool close)
    {
        foreach(GameObject o in _dynamicBlockers)
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

        if(close)
        {
            foreach(EnemyWalker enemy in _enemiesAlive)
            {
                enemy.ActivateEnemy();
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

    public void SetCode(int code)
    {
        _code = code;
    }

    public void SetEndNode(bool TorF)
    {
        _isEndNode = TorF;
    }

    public void AssignValue()
    {
        switch(_code)
        {
            case 1111:
                _left = true;
                _up = true;
                _right = true;
                _down = true;
            break;

            case 1110:
                _left = true;
                _up = true;
                _right = true;
            break;

            case 1101:
                _left = true;
                _up = true;
                _down = true;
            break;

            case 1100:
                _left = true;
                _up = true;
            break;

            case 1011:
                _left = true;
                _right = true;
                _down = true;
            break;

            case 1010:
                _left = true;
                _right = true;
            break;

            case 1001:
                _left = true;
                _down = true;
            break;

            case 1000:
                _left = true;
            break;

            case 111:
                _up = true;
                _right = true;
                _down = true;
            break;

            case 110:
                _up = true;
                _right = true;
            break;

            case 101:
                _up = true;
                _down = true;
            break;

            case 100:
                _up = true;
            break;

            case 11:
                _right = true;
                _down = true;
            break;

            case 10:
                _right = true;
            break;

            case 1:
                _down = true;
            break;

            case 0:
                
            break;

            default:
                Debug.LogError("MapVariant::AssignValue() no value can be assigned | code: " + _code);
            break;
        }
    }
}
