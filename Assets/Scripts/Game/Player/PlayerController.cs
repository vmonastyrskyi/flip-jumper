using System;
using DG.Tweening;
using Game.Platform;
using UnityEngine;

namespace Game.Player
{
    public enum PlayerState
    {
        Start,
        Idle,
        PrepareToJump,
        Jumped,
        Stepped,
        Dead
    }

    public class PlayerController : MonoBehaviour
    {
        private const float MaxJumpDistance = 12f;
        private const float JumpSpeed = 1.125f;
        private const float InitialFallVelocity = 9.81f / 1.75f;

        [SerializeField] private ParticleSystem steppedEffectParticle;

        private Rigidbody _rigidbody;
        private Transform _centerPivot;
        private Transform _downPivot;
        private Animator _animator;
        private SpawnDirection _spawnDirection = SpawnDirection.Right;
        private SpawnDirection _lastSpawnDirection = SpawnDirection.Right;
        private PlayerState _playerState = PlayerState.Start;
        private Vector3 _p0;
        private Vector3 _p1;
        private Vector3 _p2;
        private float _jumpDistance;
        private float _jumpTime;
        private bool _isJumping;
        private bool _isGrounded;

        private static readonly int IsStepped = Animator.StringToHash("IsStepped");
        private static readonly int PrepareJump = Animator.StringToHash("PrepareJump");

        // TODO Debug
        private const int PositionCount = 50;
        private readonly Vector3[] _positions = new Vector3[50];
        private LineRenderer _lineRenderer;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _centerPivot = transform.GetChild(0);
            _downPivot = _centerPivot.GetChild(0);
            _animator = _centerPivot.GetComponent<Animator>();

            // TODO Debug
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = PositionCount;
            _lineRenderer.startWidth = 0;
            _lineRenderer.endWidth = .25f;

            GameEventSystem.instance.OnSpawnDirectionChanged += Turn;
        }

        private void Turn(SpawnDirection newSpawnDirection)
        {
            _lastSpawnDirection = _spawnDirection;
            _spawnDirection = newSpawnDirection;

            if (_lastSpawnDirection != _spawnDirection)
            {
                switch (_spawnDirection)
                {
                    case SpawnDirection.Left:
                        transform.DORotate(new Vector3(0, -90, 0), .33f);
                        break;
                    case SpawnDirection.Right:
                        transform.DORotate(new Vector3(0, 0, 0), .33f);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void Update()
        {
            HandleJump();

            Jump();

            Idle();
        }

        private void HandleJump()
        {
            var isIdle = _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
            var isPrepareJump = _animator.GetCurrentAnimatorStateInfo(0).IsName("PrepareJump");
            var isStepped = _animator.GetBool(IsStepped);

            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (_isGrounded && isIdle && !isStepped &&
                    touch.phase == TouchPhase.Began)
                {
                    ChangeState(PlayerState.PrepareToJump);
                    
                    _animator.SetBool(PrepareJump, true);
                }
                else if (_isGrounded && isPrepareJump &&
                         (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved))
                {
                    CalculateJumpDistance();

                    _jumpTime += Time.deltaTime;
                    _jumpDistance += MaxJumpDistance * Time.deltaTime;

                    if (_jumpTime > 1 || _jumpDistance > MaxJumpDistance)
                    {
                        _jumpDistance = MaxJumpDistance;
                        _jumpTime = 1;
                    }
                }
                else if (_isGrounded && isPrepareJump &&
                         touch.phase == TouchPhase.Ended)
                {
                    ChangeState(PlayerState.Jumped);
                
                    _rigidbody.useGravity = false;
                    _p0 = transform.position;
                    _isGrounded = false;
                    _isJumping = true;
                    _jumpTime = 0;

                    _animator.Rebind();
                    _animator.Play("Flip");
                }
            }

            if (_isGrounded && isIdle && !isStepped &&
                Input.GetKeyDown(KeyCode.Space))
            {
                ChangeState(PlayerState.PrepareToJump);
                
                _animator.SetBool(PrepareJump, true);
            }
            else if (_isGrounded && isPrepareJump &&
                     Input.GetKey(KeyCode.Space))
            {
                CalculateJumpDistance();

                _jumpTime += Time.deltaTime;
                _jumpDistance += MaxJumpDistance * Time.deltaTime;

                if (_jumpTime > 1 || _jumpDistance > MaxJumpDistance)
                {
                    _jumpDistance = MaxJumpDistance;
                    _jumpTime = 1;
                }
            }
            else if (_isGrounded && isPrepareJump &&
                     Input.GetKeyUp(KeyCode.Space))
            {
                ChangeState(PlayerState.Jumped);
                
                _rigidbody.useGravity = false;
                _p0 = transform.position;
                _isGrounded = false;
                _isJumping = true;
                _jumpTime = 0;

                _animator.Rebind();
                _animator.Play("Flip");
            }
        }

        private void CalculateJumpDistance()
        {
            switch (_spawnDirection)
            {
                case SpawnDirection.Left:
                    _p1 = (Vector3.forward + Vector3.up) * _jumpDistance;
                    _p2 = Vector3.forward * _jumpDistance;
                    break;
                case SpawnDirection.Right:
                    _p1 = (Vector3.right + Vector3.up) * _jumpDistance;
                    _p2 = Vector3.right * _jumpDistance;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // TODO Debug
            var position = _downPivot.position;
            Debug.DrawRay(position, _p1, Color.black);
            Debug.DrawRay(position, _p2, Color.black);
        }

        private void Jump()
        {
            if (!_isJumping) return;

            _jumpTime += Time.deltaTime / JumpSpeed;

            if (_jumpTime > 1 && !_isGrounded)
            {
                _rigidbody.velocity = Vector3.down * InitialFallVelocity;
                _rigidbody.useGravity = true;
                _isJumping = false;
                _jumpTime = 1;
            }

            transform.position = QuadraticBezier(_p0, _p0 + _p1, _p0 + _p2, _jumpTime);

            // TODO Debug
            DrawQuadraticCurve();
        }

        private static Vector3 QuadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            return Mathf.Pow(1 - t, 2) * p0 +
                   2f * (1 - t) * t * p1 +
                   Mathf.Pow(t, 2) * p2;
        }

        private void DrawQuadraticCurve()
        {
            for (var i = 1; i < PositionCount + 1; i++)
            {
                var t = i / (float) PositionCount;
                var p0 = new Vector3(_p0.x, _p0.y - 1, _p0.z);

                _positions[i - 1] = QuadraticBezier(p0, p0 + _p1, p0 + _p2, t);
            }

            _lineRenderer.SetPositions(_positions);
        }

        private void Idle()
        {
            if (_playerState != PlayerState.Start &&
                _playerState != PlayerState.Idle &&
                _playerState != PlayerState.Jumped &&
                _playerState != PlayerState.Dead &&
                _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                ChangeState(PlayerState.Idle);
        }

        private void OnCollisionEnter(Collision other)
        {
            Debug.Log(other.transform.name);

            if (CheckForStep(other.gameObject))
            {
                transform.parent = other.transform;

                _isGrounded = true;
                _isJumping = false;
                _animator.SetBool(IsStepped, true);

                if (_playerState != PlayerState.Start)
                {
                    ChangeState(PlayerState.Stepped);

                    _jumpTime = 0;
                    _jumpDistance = 0;

                    var steppedEffect = Instantiate(steppedEffectParticle,
                        _downPivot.position,
                        Quaternion.identity);
                    steppedEffect.Play();

                    _animator.SetBool(IsStepped, true);

                    var platformGuid = other.gameObject.GetComponent<PlatformManager>().Guid;
                    PlatformEventSystem.instance.SetIsPlayerStepped(platformGuid, true);
                }
            }
            else
            {
                ChangeState(PlayerState.Dead);
            }
        }

        private bool CheckForStep(GameObject platform)
        {
            var playerPosition = _downPivot.TransformPoint(Vector3.zero);
            var platformPosition = platform.transform.position;
            var platformMesh = platform.GetComponentInChildren<MeshRenderer>();

            switch (_spawnDirection)
            {
                case SpawnDirection.Left:
                    if (playerPosition.z < platformPosition.z)
                    {
                        var delta = platformPosition.z - (platformMesh.bounds.size / 2).z;
                        if (playerPosition.z >= delta)
                            return true;
                    }
                    else if (playerPosition.z > platformPosition.z)
                    {
                        var delta = platformPosition.z + (platformMesh.bounds.size / 2).z;
                        if (playerPosition.z <= delta)
                            return true;
                    }
                    else
                        return false;

                    break;
                case SpawnDirection.Right:
                    if (playerPosition.x < platformPosition.x)
                    {
                        var delta = platformPosition.x - (platformMesh.bounds.size / 2).x;
                        if (playerPosition.x >= delta)
                            return true;
                    }
                    else if (playerPosition.x > platformPosition.x)
                    {
                        var delta = platformPosition.x + (platformMesh.bounds.size / 2).x;
                        if (playerPosition.x <= delta)
                            return true;
                    }
                    else
                        return false;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return false;
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case "Pit":
                    ChangeState(PlayerState.Dead);
                    break;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            transform.parent = null;
        }

        private void ChangeState(PlayerState state)
        {
            PlayerEventSystem.instance.ChangeState(_playerState = state);
        }
    }
}