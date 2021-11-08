using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour
{

    private Rigidbody2D _rigidBody;
    [SerializeField] private float _speed;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private float _groundCheckerRadius;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private LayerMask _whatIsCell;
    [SerializeField] private Collider2D _headCollider;
    [SerializeField] private float _headChekerRadius;
    [SerializeField] private Transform _headChecker;

    [Header(("Animation"))]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _runAnimatorKey;
    [SerializeField] private string _jumpAnimationKey;
    [SerializeField] private string _crouchAnimatorKey;

    private float _horizontalDirection;
    private float _verticalDirection;
    private bool _jump;
    private bool _crawl;
    public bool CanClimb { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        _horizontalDirection = Input.GetAxisRaw("Horizontal");
        _verticalDirection = Input.GetAxisRaw("Vertical");

        _animator.SetFloat(_runAnimatorKey, Mathf.Abs(_horizontalDirection));


        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jump = true;
        }


        if (_horizontalDirection > 0 && _spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = false;
        }
        else if(_horizontalDirection < 0 && !_spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = true;
        }

        _crawl = Input.GetKey(KeyCode.C);

    }

    private void FixedUpdate()
    {
        _rigidBody.velocity = new Vector2(_horizontalDirection * _speed, _rigidBody.velocity.y);

        if (CanClimb)
        {
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _verticalDirection * _speed);
            _rigidBody.gravityScale = 0;
        }
        else
        {
            _rigidBody.gravityScale = 2;
        }


        bool canJump = (bool)Physics2D.OverlapCircle((Vector2)_groundChecker.position, _groundCheckerRadius, _whatIsGround);
        bool canStand = (bool)!Physics2D.OverlapCircle((Vector2)_headChecker.position, _headChekerRadius, _whatIsCell);

        _headCollider.enabled = !_crawl && canStand;

        if (_jump && canJump)
        {
            _rigidBody.AddForce(Vector2.up * _jumpForce);
            _jump = false;
        }

        _animator.SetBool(_jumpAnimationKey, !canJump);
        _animator.SetBool(_crouchAnimatorKey, !_headCollider.enabled);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundChecker.position, _groundCheckerRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_headChecker.position, _headChekerRadius);
    }

    public void AddHp(int hpPoints)
    {
        Debug.Log("Added " + hpPoints + " hp");
    }
}
