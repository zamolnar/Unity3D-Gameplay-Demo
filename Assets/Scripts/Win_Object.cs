using System;
using System.Xml;
using UnityEngine;
using UnityEngine.InputSystem;


public class Win_Object : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has bumped into the box collider!");
            //Player.GetComponent<Player_Movemet>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
