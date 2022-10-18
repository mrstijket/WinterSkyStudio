using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Movement;

public class Projectile : MonoBehaviour
{
    PlayerMovement player;
    
    public AnimationCurve curve;
    [SerializeField] private float duration = 1f;
    [SerializeField] private float heightY = 3f;
    //[SerializeField] GameObject particles;
    
    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        Curve(transform.position, player.transform.position);
        Destroy(gameObject, 5); //Destroy after 5 seconds automatically
    }
    public void Curve(Vector3 start, Vector2 target)
    {
        StartCoroutine(CurveProcess(start,target));
    }
    public IEnumerator CurveProcess(Vector3 start, Vector2 target)
    {
        float timePassed = 0;
        Vector2 end = target;
        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration; // 0 to 1 time
            float heightT = curve.Evaluate(linearT); // value from the curve
            
            float height = Mathf.Lerp(0f, heightY, heightT);
            transform.position = Vector2.Lerp(start, end, linearT) + new Vector2(0, height);// adding values on y axis
            yield return null;
        }
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Instantiate(particles, transform.position, Quaternion.identity);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            print("player hit");
        }
    }
}

