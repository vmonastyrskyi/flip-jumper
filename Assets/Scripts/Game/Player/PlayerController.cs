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
        Jumping,
        Stepped,
        Dead
    }

    public class PlayerController : MonoBehaviour
    {
        private const float MaxJumpDistance = 12;
        private const float MinJumpDistance = 1;
        private const float JumpTime = 1.125f;

        private Transform _centerPivot;
        private Transform _downPivot;
        private Animator _animator;
        private SpawnDirection _spawnDirection = SpawnDirection.Right;
        private SpawnDirection _lastSpawnDirection = SpawnDirection.Right;
        private PlayerState _playerState = PlayerState.Start;
        private Vector3 _p0;
        private Vector3 _p1;
        private Vector3 _p2;
        private float _jumpDistance = MinJumpDistance;
        private float _prepareJumpTime;
        private bool _isGrounded;

        private static readonly int IsStepped = Animator.StringToHash("IsStepped");
        private static readonly int PrepareJump = Animator.StringToHash("PrepareJump");

        // TODO Debug
        // private const int PositionCount = 50;
        // private readonly Vector3[] _positions = new Vector3[50];
        // private LineRenderer _lineRenderer;

        private void Awake()
        {
            _centerPivot = transform.GetChild(0);
            _downPivot = _centerPivot.GetChild(0);
            _animator = _centerPivot.GetComponent<Animator>();

            // TODO Debug
            // _lineRenderer = GetComponent<LineRenderer>();
            // _lineRenderer.positionCount = PositionCount;
            // _lineRenderer.startWidth = 0;
            // _lineRenderer.endWidth = .25f;
        }

        private void Start()
        {
            GameEventSystem.instance.OnDirectionChange += Turn;
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
                        transform.DORotate(new Vector3(0, -90, 0), .33f).SetEase(Ease.InOutSine);
                        break;
                    case SpawnDirection.Right:
                        transform.DORotate(new Vector3(0, 0, 0), .33f).SetEase(Ease.InOutSine);
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
            if (!_isGrounded) return;
            
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if ((_playerState == PlayerState.Start || _playerState == PlayerState.Idle) &&
                    touch.phase == TouchPhase.Began)
                {
                    ChangeState(PlayerState.PrepareToJump);
                    _animator.SetBool(PrepareJump, true);
                }
                else if (_playerState == PlayerState.PrepareToJump && 
                         (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved))
                {
                    CalculateJumpDistance();

                    _prepareJumpTime += Time.deltaTime;
                    _jumpDistance += MaxJumpDistance * Time.deltaTime;

                    if (_prepareJumpTime > 1 || _jumpDistance > MaxJumpDistance)
                    {
                        _jumpDistance = MaxJumpDistance;
                        _prepareJumpTime = 1;
                    }
                }
                else if (_playerState == PlayerState.PrepareToJump && 
                         touch.phase == TouchPhase.Ended)
                {
                    ChangeState(PlayerState.Jumping);

                    _prepareJumpTime = 0;
                    _p0 = transform.position;

                    _animator.Rebind();
                    _animator.Play("Flip");
                }
            }

            if ((_playerState == PlayerState.Start || _playerState == PlayerState.Idle) &&
                Input.GetKeyDown(KeyCode.Space))
            {
                ChangeState(PlayerState.PrepareToJump);
                _animator.SetBool(PrepareJump, true);
            }

            if (_playerState == PlayerState.PrepareToJump && Input.GetKey(KeyCode.Space))
            {
                CalculateJumpDistance();

                _prepareJumpTime += Time.deltaTime;
                _jumpDistance += MaxJumpDistance * Time.deltaTime;

                if (_prepareJumpTime > 1 || _jumpDistance > MaxJumpDistance)
                {
                    _jumpDistance = MaxJumpDistance;
                    _prepareJumpTime = 1;
                }
            }

            if (_playerState == PlayerState.PrepareToJump && Input.GetKeyUp(KeyCode.Space))
            {
                ChangeState(PlayerState.Jumping);

                _prepareJumpTime = 0;
                _p0 = transform.position;

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
            // var position = _downPivot.position;
            // Debug.DrawRay(position, _p1, Color.black);
            // Debug.DrawRay(position, _p2, Color.black);
        }

        private void Jump()
        {
            if (_playerState != PlayerState.Jumping) return;

            _prepareJumpTime += Time.deltaTime / JumpTime;

            if (_prepareJumpTime > 1) return;

           transform.position = QuadraticBezier(_p0, _p0 + _p1, _p0 + _p2, _prepareJumpTime);

            // TODO Debug
            // DrawQuadraticCurve();
        }

        private static Vector3 QuadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            return Mathf.Pow(1 - t, 2) * p0 +
                   2f * (1 - t) * t * p1 +
                   Mathf.Pow(t, 2) * p2;
        }

        // private void DrawQuadraticCurve()
        // {
        //     for (var i = 1; i < PositionCount + 1; i++)
        //     {
        //         var t = i / (float) PositionCount;
        //         var p0 = new Vector3(_p0.x, _p0.y - 1, _p0.z);
        //
        //         _positions[i - 1] = QuadraticBezier(p0, p0 + _p1, p0 + _p2, t);
        //     }
        //
        //     _lineRenderer.SetPositions(_positions);
        // }

        private void Idle()
        {
            if (_playerState == PlayerState.Stepped &&
                _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                ChangeState(PlayerState.Idle);
        }

        private void OnCollisionEnter(Collision other)
        {
            Debug.Log(other.transform.name);

            Debug.Log(ValidateStep(other.gameObject));
            if (ValidateStep(other.gameObject))
            {
                transform.parent = other.transform;

                _isGrounded = true;
                _animator.SetBool(IsStepped, true);

                if (_playerState != PlayerState.Start)
                {
                    ChangeState(PlayerState.Stepped);

                    _p0 = _p1 = _p2 = Vector3.zero;
                    _prepareJumpTime = 0;
                    _jumpDistance = MinJumpDistance;

                    var platformGuid = other.gameObject.GetComponent<PlatformManager>().Guid;
                    PlatformEventSystem.instance.PlayerStepped(platformGuid);
                }
            }
            else
            {
                ChangeState(PlayerState.Dead);
            }
        }

        private bool ValidateStep(GameObject platform)
        {
            var playerPosition = transform.TransformPoint(Vector3.zero);
            var platformCollider = platform.GetComponent<BoxCollider>();
            var platformPosition = platform.transform.TransformPoint(Vector3.zero);

            switch (_spawnDirection)
            {
                case SpawnDirection.Left:
                    if (playerPosition.z > (platformPosition.z + platformCollider.size.z / 2))
                        return false;
                    if (playerPosition.z < (platformPosition.z - platformCollider.size.z / 2))
                        return false;
                    break;
                case SpawnDirection.Right:
                    if (playerPosition.x > (platformPosition.x + platformCollider.size.x / 2))
                        return false;
                    if (playerPosition.x < (platformPosition.x - platformCollider.size.x / 2))
                        return false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Pit") && _playerState != PlayerState.Dead)
                ChangeState(PlayerState.Dead);
        }

        private void OnCollisionExit(Collision other)
        {
            transform.parent = null;
            _isGrounded = false;
        }

        private void ChangeState(PlayerState state)
        {
            PlayerEventSystem.instance.ChangeState(_playerState = state);
        }
    }
}