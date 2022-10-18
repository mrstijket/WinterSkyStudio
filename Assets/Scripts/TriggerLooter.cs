using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLooter : MonoBehaviour
{
    [SerializeField] GameObject[] looter;
    BoxCollider2D boxCollider2D;

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boxCollider2D.enabled = false;
            for (int i = 0; i < looter.Length; i++)
            {
                looter[i].SetActive(true);
            }
        }
    }
}
