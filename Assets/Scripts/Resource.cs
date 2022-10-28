using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private int _id = 0;
    [SerializeField] private AudioClip _resourcePickupSound;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            GameManager.Instance.AddResource(_id);
            MusicManager.Instance.fXPlayer.PlayOneShot(_resourcePickupSound, SaveManager.Instance.fXVolume);
            Destroy(this.gameObject);
        }
    }
}
