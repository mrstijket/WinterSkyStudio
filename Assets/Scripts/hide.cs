using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Movement;

public class hide : MonoBehaviour
{
    PlayerMovement playerMovement;
    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }
    private void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            print("touched " + other.gameObject.name);
            playerMovement._rigidbody2D.gravityScale = 0;
            playerMovement.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        print("untouched " + other.gameObject.name);
        playerMovement.gameObject.layer = LayerMask.NameToLayer("Player");
        playerMovement._rigidbody2D.gravityScale = 1;
    }
}
