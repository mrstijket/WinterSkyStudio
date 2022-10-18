using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWatchAI : MonoBehaviour 
{
    public bool gotCaught = false;
    EnemyFollowAI _enemyFollowAI;
    Vector3 changeView = Vector3.back;
    Animator _heathensRotation;
    [SerializeField] float rightSideAngle;
    [SerializeField] float leftSideAngle;
    [SerializeField] FieldOfView _fieldOfView;
    [SerializeField] BoxCollider2D leftBlock;
    [SerializeField] BoxCollider2D rightBlock;
    
    void Awake() {
        _heathensRotation = GetComponent<Animator>();
        _enemyFollowAI = GetComponent<EnemyFollowAI>();
        _enemyFollowAI.enabled = false;
    }

    void Update() {
        _fieldOfView.SetOrigin(transform.position + new Vector3(0f, 0.5f, 0f));
        if(transform.localScale.x == 1) {
            _fieldOfView.gameObject.SetActive(true);
            _fieldOfView.SetAimDirection(rightSideAngle);
        }
        else if(transform.localScale.x == -1) {
            _fieldOfView.gameObject.SetActive(true);
            _fieldOfView.SetAimDirection(leftSideAngle);
        }
        else {
            _fieldOfView.gameObject.SetActive(false);
        }
        //_fieldOfView.SetAimDirection(transform.localRotation.eulerAngles.y -35f);//transform.localScale.x);
        if (Input.GetKeyDown(KeyCode.E)) {
            if (changeView == Vector3.back)
                changeView = Vector3.down;
            else
                changeView = Vector3.back;
        }

        if (gotCaught) {
            if (leftBlock) {
                leftBlock.isTrigger = false;
            }
            if (rightBlock) {
                rightBlock.isTrigger = false;
            }
            
            AstarPath.active.Scan();
            _heathensRotation.enabled = false;
            _fieldOfView.gameObject.SetActive(false);
            StartCoroutine(StartFollowing());
        }

    }
    IEnumerator StartFollowing() {
        yield return new WaitForSeconds(0.2f);
        _enemyFollowAI.enabled = true;
    }
}
