using System;
using System.Diagnostics;
using UnityEngine;

public class BetaAIController : MonoBehaviour
{
    [SerializeField]
    private LayerMask _player;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    [Range(1, 15)]
    private float _canAttackRadius;
    [SerializeField]
    [Range(5, 30)]
    private float _canSeeRadius;
    [SerializeField]
    [Range(1, 5)]
    private float _speed;
    [SerializeField]
    private LayerMask _obstacle;
    
    
    private Transform _target;
    private Rigidbody _rb;
    private Stopwatch _sw;
    private TimeSpan _ts = new TimeSpan(0, 0, 2);
    private Vector3 _moveDirection;
    private AudioSource _attackSound;

    // Start is called before the first frame update
    void Start()
    {
        _attackSound = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody>();
        _sw = new Stopwatch();
        _sw.Start();
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] _attackArea = Physics.OverlapSphere(transform.position, _canAttackRadius, _player);
        if (_attackArea.Length > 0)
        {
            if (_sw.Elapsed > _ts)
            {
                _animator.SetBool("PlayerOnAttackRange", true);
                print("attack!");
                _target = _attackArea[0].transform;
                Attack(_target.GetComponent<Health>());
            }
        }
        else
        { 
            _animator.SetBool("PlayerOnAttackRange", false);
            Collider[] _viewArea = Physics.OverlapSphere(transform.position, _canSeeRadius, _player);
            if (_viewArea.Length > 0)
            {
                _target = _viewArea[0].transform;
                Pursuit();
                print("Pursuit");
            }
            else
            {
                _animator.SetBool("Walking", false);
            }
        }
    }

    private void Attack(Health player)
    {
        _animator.SetBool("Walking", false);
        _animator.SetTrigger("CanAttack");
        _attackSound.Play();
        transform.LookAt(_target);
        player.TakeDamage(1);
        _sw.Restart();
    }

    private void Pursuit()
    {
        if (CanPursuit())
        {
            _moveDirection = (_target.position - transform.position).normalized * _speed; 
            _moveDirection.y = _rb.velocity.y;
            transform.LookAt(_target);
            _animator.SetBool("Walking", true);
            _rb.velocity = _moveDirection;
        }
        else
        {
            _animator.SetBool("Walking", false);
            _rb.velocity = Vector3.zero;
        }
    }

    private bool CanPursuit()
    {
        if (Physics.Raycast(transform.position, _moveDirection, Vector3.Distance(transform.position, _moveDirection), _obstacle))
        {
            print("Cannot pursuit");
            _animator.SetBool("Walking", false);
            return false;
        }
        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _canAttackRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _canSeeRadius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + _moveDirection * 3f);
    }
}
