using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Movement;

public class ProjectileThrow : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float moveSpeed = 4f;
    public float maxCloseDistance = 4f;

    PlayerMovement player;

    [Header("Projectile")]
    [SerializeField] float projectileSpeed = 1f;
    float projectileStart = 0f;
    [SerializeField] float projectileColddown = 1f;
    [SerializeField] Transform projectile;
    public GameObject rockObject;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        Move();
    }
    private void Move()
    {
        float option = Random.Range(0, 2);
        print(option);
        if(option == 0)
        {
            rb.velocity = new Vector2(moveSpeed,0f);
        }
        else if (option == 1)
        {
            rb.velocity = new Vector2(-moveSpeed, 0f);
        }
        else
        {
            print("error");
        }
    }
    private void Update()
    {
        Throw();
        Flip();
    }
    private void Flip()
    {
        if(player.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else if(player.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
    }
    public void Throw()
    {
        //if (!isAlive) { return; }
        if (Time.time > projectileStart + projectileColddown) //Input.GetKeyDown(KeyCode.E) &&
        {
            projectileStart = Time.time;
            if (Vector3.Distance(player.transform.position, transform.position) < maxCloseDistance)
            {
                //close range type attack
            }
            else
            {
                StartCoroutine(FireContinuously());
            }
        }
    }
    IEnumerator FireContinuously()
    {
        GameObject star = Instantiate(rockObject, projectile.position, Quaternion.identity) as GameObject;
        //var direction = -transform.right + Vector3.up;
        //star.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed, ForceMode2D.Impulse);
        //if (transform.localScale.x > 0f)
        //        // = new Vector2(projectileSpeed, 0);
        //if (transform.localScale.x < 0f)
        //    star.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed, 0);
        yield return new WaitForSeconds(1.5f);
        Destroy(star);
    }
}
