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
    [SerializeField] private Collider2D _headCollider;
    [SerializeField] private float _headChekerRadius;
    [SerializeField] private Transform _headChecker;

    private float _direction;
    private bool _jump;
    private bool _crawl;


    // Start is called before the first frame update
    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        _direction = Input.GetAxisRaw("Horizontal");


        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jump = true;
        }


        if (_direction > 0 && _spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = false;
        }
        else if(_direction < 0 && !_spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = true;
        }

        _crawl = Input.GetKey(KeyCode.C);
        
        

    }

    private void FixedUpdate()
    {
        _rigidBody.velocity = new Vector2(x:_direction * _speed, _rigidBody.velocity.y);


        bool canJump = (bool)Physics2D.OverlapCircle((Vector2)_groundChecker.position, _groundCheckerRadius, _whatIsGround);
        bool canStand = (bool)!Physics2D.OverlapCircle((Vector2)_headChecker.position, _headChekerRadius, _whatIsGround);

        _headCollider.enabled = !_crawl && canStand;

        if (_jump && canJump)
        {
            _rigidBody.AddForce(Vector2.up * _jumpForce);
            _jump = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundChecker.position, _groundCheckerRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_headChecker.position, _headChekerRadius);
    }
}
