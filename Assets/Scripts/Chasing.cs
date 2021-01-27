using Game.Controllers;
using Game.EventSystems;
using Game.Player;
using UnityEngine;

public class Chasing : MonoBehaviour
{
    private const float MaxDistance = 6;
    private const float MinSpeed = 2;
    private const float MaxSpeed = 4;
    private const float MinTurnSpeed = 0.00625f;
    private const float MaxTurnSpeed = 0.0125f;
    private const float MinAnimatorSpeed = 1;
    private const float MaxAnimatorSpeed = 2;

    private Transform _transform;
    private Animator _animator;
    private GameObject _target;
    private GameObject _player;
    private GameObject _platform;

    private float _speed = MinSpeed;
    private float _turnSpeed = MinTurnSpeed;

    private void Start()
    {
        _transform = transform;
        _animator = GetComponent<Animator>();
        _target = _player = GameObject.FindWithTag("Player");

        PlayerEventSystem.instance.OnStateChanged += ChangeTarget;
    }

    private void ChangeTarget(PlayerState state)
    {
        if (state == PlayerState.Jumping)
            _target = _player;
        else if (state == PlayerState.Stepped)
            _target = _player.transform.parent.gameObject;
    }

    private void Update()
    {
        var dirToTarget = _target.transform.position - _transform.position;
        
        UpdateSpeed();
        LootAt(dirToTarget);
        Follow();
    }

    private void UpdateSpeed()
    {
        var deltaTime = Time.deltaTime;
        var animatorSpeed = _animator.speed;

        if (Vector3.Distance(_target.transform.position, _transform.position) > MaxDistance)
        {
            _speed = _speed < MaxSpeed ? _speed + deltaTime : MaxSpeed;
            _turnSpeed = _turnSpeed < MaxTurnSpeed ? _turnSpeed + deltaTime / 500 : MaxTurnSpeed;
            _animator.speed = animatorSpeed < MaxAnimatorSpeed ? animatorSpeed + deltaTime : MaxAnimatorSpeed;
        }
        else
        {
            _speed = _speed > MinSpeed ? _speed - deltaTime : MinSpeed;
            _turnSpeed = _turnSpeed > MinTurnSpeed ? _turnSpeed - deltaTime / 500 : MinTurnSpeed;
            _animator.speed = animatorSpeed > MinAnimatorSpeed ? animatorSpeed - deltaTime : MinAnimatorSpeed;
        }
    }

    private void LootAt(Vector3 dirToTarget)
    {
        var direction = new Vector3(dirToTarget.x, 0, dirToTarget.z);
        var targetRotation = Quaternion.LookRotation(direction);
        _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, _turnSpeed);
    }

    private void Follow()
    {
        _transform.position += _transform.rotation * Vector3.forward * (_speed * Time.deltaTime);
    }
}