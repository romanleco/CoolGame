using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private int _id = 0;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            GameManager.Instance.AddResource(_id);
            Destroy(this.gameObject);
        }
    }
}
