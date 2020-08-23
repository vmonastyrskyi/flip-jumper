using System;
using UnityEngine;

namespace Game
{
    public enum PlayerState
    {
        Start,
        Idle,
        Jumped,
        Stepped
    }

    public class PlayerController : MonoBehaviour
    {
        private const float MaxJumpDistance = 12f;
        private const float JumpSpeed = 1.125f;

        public ParticleSystem steppedEffectParticle;

        private Animator _animator;
        private Rigidbody _rigidbody;
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
        private bool _isInPit;

        private static readonly int IsStepOn = Animator.StringToHash("IsStepOn");
        private static readonly int PrepareLeftJump = Animator.StringToHash("PrepareLeftJump");
        private static readonly int PrepareRightJump = Animator.StringToHash("PrepareRightJump");
        private static readonly int TurnLeft = Animator.StringToHash("TurnLeft");
        private static readonly int TurnRight = Animator.StringToHash("TurnRight");

        // TODO Debug
        private const int PositionCount = 50;
        private readonly Vector3[] _positions = new Vector3[50];
        private LineRenderer _lineRenderer;

        private void Start()
        {
            GameEventSystem.current.OnSpawnDirectionChanged += Turn;

            PlayerEventSystem.current.OnStateChanged += state => { _playerState = state; };

            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();

            // TODO Debug
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = PositionCount;
            _lineRenderer.startWidth = 0;
            _lineRenderer.endWidth = .35f;
        }

        private void Turn(SpawnDirection direction)
        {
            _lastSpawnDirection = _spawnDirection;
            _spawnDirection = direction;

            if (_lastSpawnDirection != _spawnDirection)
            {
                switch (_spawnDirection)
                {
                    case SpawnDirection.Left:
                        _animator.SetBool(TurnLeft, true);
                        break;
                    case SpawnDirection.Right:
                        _animator.SetBool(TurnRight, true);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void Update()
        {
            CalculateJumpDistance();

            HandleJump();

            Jump();

            Idle();
        }

        private void CalculateJumpDistance()
        {
            var position = transform.position;

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
            Debug.DrawRay(position, _p2, Color.black);
            Debug.DrawRay(position, _p1, Color.black);
        }

        private void HandleJump()
        {
            var isIdle = _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
            var isPrepareLeftJump = _animator.GetCurrentAnimatorStateInfo(0).IsName("PrepareLeftJump");
            var isPrepareRightJump = _animator.GetCurrentAnimatorStateInfo(0).IsName("PrepareRightJump");

            // if (Input.touchCount > 0)
            // {
            //     var touch = Input.GetTouch(0);
            //     if (_isGrounded && isIdle && !_animator.GetBool(IsStepOn) &&
            //         touch.phase == TouchPhase.Began)
            //     {
            //         switch (_spawnDirection)
            //         {
            //             case SpawnDirection.Left:
            //                 _animator.SetBool(PrepareLeftJump, true);
            //                 break;
            //             case SpawnDirection.Right:
            //                 _animator.SetBool(PrepareRightJump, true);
            //                 break;
            //             default:
            //                 throw new ArgumentOutOfRangeException();
            //         }
            //     }
            //     else if (_isGrounded && (isPrepareLeftJump || isPrepareRightJump) &&
            //              (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved))
            //     {
            //         _jumpTime += Time.deltaTime;
            //         if (_jumpTime > 1f)
            //         {
            //             _jumpTime = 1f;
            //             return;
            //         }
            //
            //         _jumpDistance += Time.deltaTime * MaxJumpDistance;
            //         if (_jumpDistance > MaxJumpDistance)
            //             _jumpDistance = MaxJumpDistance;
            //     }
            //     else if (_isGrounded && (isPrepareLeftJump || isPrepareRightJump) &&
            //              touch.phase == TouchPhase.Ended)
            //     {
            //         PlayerEventSystem.current.ChangeState(PlayerState.Jumped);
            //
            //         _rigidbody.useGravity = false;
            //         _p0 = transform.position;
            //         _jumpTime = 0;
            //         _isGrounded = false;
            //         _isJumping = true;
            //
            //         _animator.Rebind();
            //         switch (_spawnDirection)
            //         {
            //             case SpawnDirection.Left:
            //                 _animator.Play("LeftFlip");
            //                 break;
            //             case SpawnDirection.Right:
            //                 _animator.Play("RightFlip");
            //                 break;
            //             default:
            //                 throw new ArgumentOutOfRangeException();
            //         }
            //     }
            // }

            if (_isGrounded && isIdle && !_animator.GetBool(IsStepOn) &&
                Input.GetKeyDown(KeyCode.Space))
            {
                switch (_spawnDirection)
                {
                    case SpawnDirection.Left:
                        _animator.SetBool(PrepareLeftJump, true);
                        break;
                    case SpawnDirection.Right:
                        _animator.SetBool(PrepareRightJump, true);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (_isGrounded && (isPrepareLeftJump || isPrepareRightJump) &&
                     Input.GetKey(KeyCode.Space))
            {
                _jumpTime += Time.deltaTime;
                if (_jumpTime > 1f)
                {
                    _jumpTime = 1f;
                    return;
                }

                _jumpDistance += Time.deltaTime * MaxJumpDistance;
                if (_jumpDistance > MaxJumpDistance)
                    _jumpDistance = MaxJumpDistance;
            }
            else if (_isGrounded && (isPrepareLeftJump || isPrepareRightJump) &&
                     Input.GetKeyUp(KeyCode.Space))
            {
                PlayerEventSystem.current.ChangeState(PlayerState.Jumped);

                _rigidbody.useGravity = false;
                _p0 = transform.position;
                _jumpTime = 0;
                _isGrounded = false;
                _isJumping = true;

                _animator.Rebind();
                switch (_spawnDirection)
                {
                    case SpawnDirection.Left:
                        _animator.Play("LeftFlip");
                        break;
                    case SpawnDirection.Right:
                        _animator.Play("RightFlip");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void Jump()
        {
            if (_isJumping && !_isInPit)
            {
                _jumpTime += Time.deltaTime / JumpSpeed;

                if (_jumpTime >= 1f)
                    _jumpTime = 1f;

                transform.position = QuadraticBezier(_p0, _p0 + _p1, _p0 + _p2, _jumpTime);

                // TODO Debug
                DrawQuadraticCurve();
            }
        }

        private static Vector3 QuadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            return Mathf.Pow(1f - t, 2) * p0 +
                   2f * (1f - t) * t * p1 +
                   Mathf.Pow(t, 2) * p2;
        }

        private void DrawQuadraticCurve()
        {
            for (var i = 1; i < PositionCount + 1; i++)
            {
                var t = i / (float) PositionCount;
                _positions[i - 1] = QuadraticBezier(_p0, _p0 + _p1, _p0 + _p2, t);
            }

            _lineRenderer.SetPositions(_positions);
        }

        private void Idle()
        {
            if (_playerState != PlayerState.Start &&
                _playerState != PlayerState.Idle &&
                _playerState != PlayerState.Jumped &&
                _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                PlayerEventSystem.current.ChangeState(PlayerState.Idle);
        }

        private void OnCollisionEnter(Collision other)
        {
            _rigidbody.useGravity = true;
            transform.parent = other.transform;
        }

        private void OnCollisionExit(Collision other)
        {
            transform.parent = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case "Floor":
                    _isGrounded = true;
                    _isJumping = false;
                    _animator.SetBool(IsStepOn, true);

                    if (_playerState != PlayerState.Start)
                    {
                        PlayerEventSystem.current.ChangeState(PlayerState.Stepped);

                        _jumpTime = 0;
                        _jumpDistance = 0;

                        var steppedEffect = Instantiate(steppedEffectParticle,
                            transform.position,
                            Quaternion.Euler(-90, 0, 0));
                        steppedEffect.Play();
                        Destroy(steppedEffect.gameObject, .55f);

                        _animator.SetBool(IsStepOn, true);

                        var platformGuid = other.GetComponentInParent<PlatformManager>().Guid;
                        PlatformEventSystem.current.SetIsPlayerStepped(platformGuid, true);
                    }

                    break;
            }
        }
    }
}