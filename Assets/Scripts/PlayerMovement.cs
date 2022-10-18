using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

namespace Player.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        //[SerializeField] Camera _mainCamera;
        //[SerializeField] FieldOfView _fieldOfView;
        Animator animator;
        public float speed = 5f;
        public float jumpHeight = 5f;
        [HideInInspector] public Rigidbody2D _rigidbody2D;
        [HideInInspector] public CapsuleCollider2D _capsuleCollider2D;

        private bool canDash = true;
        private bool isDashing;
        private float dashPower = 10f;
        private float dashingTime = 0.2f;
        private float dashingCooldown = 1f;
        [SerializeField] private TrailRenderer trailRenderer;
        void Start()
        {
            this.gameObject.layer = LayerMask.NameToLayer("Player");
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (isDashing) return;
            Move();
            Dash();
            Jump();
            FlipSprite();
            //FollowPointer();
        }

        //void FollowPointer()
        //{
        //    Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //    mouseWorldPosition.z = 0f;
        //    //Debug.Log(mouseWorldPosition);
        //    _fieldOfView.SetAimDirection(mouseWorldPosition);
        //    _fieldOfView.SetOrigin(transform.position);
        //}

        private void Move()
        {
            float controlThrow = Input.GetAxis("Horizontal");
            Vector2 playerVelocity = new Vector2(controlThrow * speed, _rigidbody2D.velocity.y);
            if(playerVelocity != Vector2.zero)
            {
                _rigidbody2D.velocity = playerVelocity;
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }
                
        }
        private void Dash()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                StartCoroutine(DashProcess());
            }
        }
        private IEnumerator DashProcess()
        {
            canDash = false;
            isDashing = true;
            float originalGravity = _rigidbody2D.gravityScale;
            _rigidbody2D.gravityScale = 0;
            _rigidbody2D.velocity = new Vector2(transform.localScale.x * dashPower, 0f);
            trailRenderer.emitting = true;
            yield return new WaitForSeconds(dashingTime);
            _rigidbody2D.velocity = new Vector2(transform.localScale.x * dashPower / 5f, 0f);
            trailRenderer.emitting = false;
            _rigidbody2D.gravityScale = originalGravity;
            yield return new WaitForSeconds(0.5f);
            isDashing = false;
            yield return new WaitForSeconds(dashingCooldown);
            canDash = true;
        }
        private void Jump()
        {
            if (!_capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }
            if (Input.GetButtonDown("Jump"))
            {
                Vector2 jumpVelocityAdd = new Vector2(0f, jumpHeight);
                _rigidbody2D.velocity += jumpVelocityAdd;
            }
        }
        private void FlipSprite()
        {
            bool playerHasHorizontalSpeed = Mathf.Abs(_rigidbody2D.velocity.x) > Mathf.Epsilon;
            if (playerHasHorizontalSpeed)
            {
                transform.localScale = new Vector2(Mathf.Sign(_rigidbody2D.velocity.x), 1f);
            }
        }
    }

}