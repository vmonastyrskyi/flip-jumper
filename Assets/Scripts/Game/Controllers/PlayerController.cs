using System;
using System.Collections;
using Audio;
using DG.Tweening;
using Game.EventSystems;
using Game.Systems;
using UnityEngine;
using Util;

namespace Game.Controllers
{
    public enum PlayerState
    {
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
        private const float PlatformCenterRadius = 0.5f;
        private const float PlatformAvailableRadius = 1.5f;
        private const float PlatformOutRadius = 3;

        private AudioManager _audioManager;
        private PlatformSystem _platformSystem;
        private JumpDirection _jumpDirection = JumpDirection.Right;
        private PlayerState _playerState = PlayerState.Idle;
        private GameObject _systems;
        private Animator _animator;
        private Vector3 _p0;
        private Vector3 _p1;
        private Vector3 _p2;
        private float _jumpDistance = MinJumpDistance;
        private float _prepareJumpTime;
        private float _jumpTime;
        private bool _isStarted;
        private bool _isGrounded;
        private bool _isSteppedCenter;
        private bool _isGamePaused;
        private int _prepareJumpPower;

        private static readonly int Step = Animator.StringToHash("Step");
        private static readonly int PrepareToJump = Animator.StringToHash("Prepare_To_Jump");

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _audioManager = GetComponent<AudioManager>();
            _systems = GameObject.FindWithTag("Systems");
            _platformSystem = _systems.GetComponent<PlatformSystem>();
        }

        private IEnumerator Start()
        {
            yield return null;

            GameEventSystem.Instance.OnDirectionChanged += Turn;
            GameEventSystem.Instance.OnGamePaused += DisableController;
            GameEventSystem.Instance.OnGameResumed += EnableController;
        }

        private void DisableController()
        {
            _isGamePaused = true;
            _audioManager.PauseAll();

            if (_playerState == PlayerState.PrepareToJump)
                CalculateJumpPosition();
        }

        private void EnableController()
        {
            _isGamePaused = false;
            _audioManager.UnPauseAll();

            if (_playerState == PlayerState.PrepareToJump)
            {
                ChangeState(PlayerState.Jumping);

                _animator.Rebind();
                _animator.Play("Flip");

                if (!_isStarted)
                {
                    _isStarted = true;
                    GameEventSystem.Instance.StartGame();
                }
            }
            else if (_playerState == PlayerState.Dead)
            {
                if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Flip"))
                {
                    _animator.Rebind();
                    _animator.Play("Flip");
                }
            }
        }

        private void Turn(JumpDirection newJumpDirection)
        {
            if (_jumpDirection != newJumpDirection)
            {
                switch (newJumpDirection)
                {
                    case JumpDirection.Left:
                        transform.DORotate(new Vector3(0, -90, 0), .33f).SetEase(Ease.Linear);
                        break;
                    case JumpDirection.Right:
                        transform.DORotate(new Vector3(0, 0, 0), .33f).SetEase(Ease.Linear);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            _jumpDirection = newJumpDirection;
        }

        private void Update()
        {
            if (!_isGamePaused)
            {
                CalculateJump();

                Jump();

                Idle();
            }
        }

        private void CalculateJump()
        {
            if (!_isGrounded) return;

            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);

                if (_playerState == PlayerState.Idle &&
                    touch.phase == TouchPhase.Began)
                {
                    if (!PointerEventSystem.IsPointerOverGameObject(touch.position, new[] {"Button"}))
                    {
                        ChangeState(PlayerState.PrepareToJump);

                        _audioManager.Play("Prepare_To_Jump", 0.85f);
                        _prepareJumpPower = 1;

                        _animator.SetTrigger(PrepareToJump);
                    }
                }

                if (_playerState == PlayerState.PrepareToJump &&
                    (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved))
                {
                    _prepareJumpTime += Time.deltaTime;
                    _jumpDistance += MaxJumpDistance * Time.deltaTime;

                    if (_prepareJumpPower == 1 && _prepareJumpTime > 0.33f)
                    {
                        _audioManager.Play("Prepare_To_Jump", 1f);
                        _prepareJumpPower = 2;
                    }
                    else if (_prepareJumpPower == 2 && _prepareJumpTime > 0.66f)
                    {
                        _audioManager.Play("Prepare_To_Jump", 1.15f);
                        _prepareJumpPower = 3;
                    }

                    if (_prepareJumpTime > 1 || _jumpDistance > MaxJumpDistance)
                    {
                        _jumpDistance = MaxJumpDistance;
                        _prepareJumpTime = 1;
                    }
                }

                if (_playerState == PlayerState.PrepareToJump &&
                    touch.phase == TouchPhase.Ended)
                {
                    ChangeState(PlayerState.Jumping);

                    CalculateJumpPosition();

                    _animator.Rebind();
                    _animator.Play("Flip");

                    if (!_isStarted)
                    {
                        GameEventSystem.Instance.StartGame();
                        _isStarted = true;
                    }
                }
            }

            if (_playerState == PlayerState.Idle && Input.GetKeyDown(KeyCode.Space))
            {
                ChangeState(PlayerState.PrepareToJump);

                _audioManager.Play("Prepare_To_Jump", 0.85f);
                _prepareJumpPower = 1;

                _animator.SetTrigger(PrepareToJump);
            }

            if (_playerState == PlayerState.PrepareToJump && Input.GetKey(KeyCode.Space))
            {
                _prepareJumpTime += Time.deltaTime;
                _jumpDistance += MaxJumpDistance * Time.deltaTime;

                if (_prepareJumpPower == 1 && _prepareJumpTime > 0.33f)
                {
                    _audioManager.Play("Prepare_To_Jump", 1f);
                    _prepareJumpPower = 2;
                }
                else if (_prepareJumpPower == 2 && _prepareJumpTime > 0.66f)
                {
                    _audioManager.Play("Prepare_To_Jump", 1.15f);
                    _prepareJumpPower = 3;
                }

                if (_prepareJumpTime > 1 || _jumpDistance > MaxJumpDistance)
                {
                    _jumpDistance = MaxJumpDistance;
                    _prepareJumpTime = 1;
                }
            }

            if (_playerState == PlayerState.PrepareToJump && Input.GetKeyUp(KeyCode.Space))
            {
                ChangeState(PlayerState.Jumping);

                CalculateJumpPosition();

                _animator.Rebind();
                _animator.Play("Flip");

                if (!_isStarted)
                {
                    GameEventSystem.Instance.StartGame();
                    _isStarted = true;
                }
            }
        }

        private void CalculateJumpPosition()
        {
            var playerPosition = _p0 = transform.position;

            var preLastPlatform = _platformSystem.PreLastPlatform();
            var preLastPlatformPosition = preLastPlatform.transform.position;

            Vector3 localPosition;
            switch (_jumpDirection)
            {
                case JumpDirection.Left:
                    localPosition = transform.localPosition;
                    if (localPosition.z + _jumpDistance <= PlatformAvailableRadius)
                    {
                        _isSteppedCenter = false;
                        _p1 = new Vector3(
                            playerPosition.x,
                            playerPosition.y + preLastPlatformPosition.y + _jumpDistance,
                            playerPosition.z + _jumpDistance
                        );
                        _p2 = new Vector3(
                            playerPosition.x,
                            playerPosition.y + preLastPlatformPosition.y,
                            playerPosition.z + _jumpDistance
                        );
                        return;
                    }
                    else if (localPosition.x + _jumpDistance <= PlatformOutRadius)
                    {
                        _p1 = new Vector3(
                            playerPosition.x,
                            playerPosition.y - localPosition.z + 3,
                            preLastPlatformPosition.z + 3
                        );
                        _p2 = new Vector3(
                            playerPosition.x,
                            playerPosition.y + preLastPlatformPosition.y,
                            preLastPlatformPosition.z + 3
                        );
                        StartCoroutine(Die(1));

                        return;
                    }

                    break;
                case JumpDirection.Right:
                    localPosition = transform.localPosition;
                    if (localPosition.x + _jumpDistance <= PlatformAvailableRadius)
                    {
                        _isSteppedCenter = false;
                        _p1 = new Vector3(
                            playerPosition.x + _jumpDistance,
                            playerPosition.y + preLastPlatformPosition.y + _jumpDistance,
                            playerPosition.z
                        );
                        _p2 = new Vector3(
                            playerPosition.x + _jumpDistance,
                            playerPosition.y + preLastPlatformPosition.y,
                            playerPosition.z
                        );
                        return;
                    }
                    else if (localPosition.x + _jumpDistance <= PlatformOutRadius)
                    {
                        _p1 = new Vector3(
                            preLastPlatformPosition.x + 3,
                            playerPosition.y - localPosition.x + 3,
                            playerPosition.z
                        );
                        _p2 = new Vector3(
                            preLastPlatformPosition.x + 3,
                            playerPosition.y + preLastPlatformPosition.y,
                            playerPosition.z
                        );
                        StartCoroutine(Die(1));

                        return;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var lastPlatform = _platformSystem.LastPlatform();
            var lastPlatformPosition = lastPlatform.transform.position;

            float distance;
            switch (_jumpDirection)
            {
                case JumpDirection.Left:
                    distance = lastPlatformPosition.z - playerPosition.z;
                    break;
                case JumpDirection.Right:
                    distance = lastPlatformPosition.x - playerPosition.x;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var deltaDistance = Mathf.Abs(distance - _jumpDistance);
            if (deltaDistance <= PlatformCenterRadius)
            {
                _isSteppedCenter = true;
                switch (_jumpDirection)
                {
                    case JumpDirection.Left:
                        _p1 = new Vector3(
                            lastPlatformPosition.x,
                            playerPosition.y + distance,
                            lastPlatformPosition.z
                        );
                        _p2 = new Vector3(
                            lastPlatformPosition.x,
                            playerPosition.y + lastPlatformPosition.y,
                            lastPlatformPosition.z
                        );
                        break;
                    case JumpDirection.Right:
                        _p1 = new Vector3(
                            lastPlatformPosition.x,
                            playerPosition.y + distance,
                            lastPlatformPosition.z
                        );
                        _p2 = new Vector3(
                            lastPlatformPosition.x,
                            playerPosition.y + lastPlatformPosition.y,
                            lastPlatformPosition.z
                        );
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (deltaDistance <= PlatformAvailableRadius)
            {
                _isSteppedCenter = false;
                switch (_jumpDirection)
                {
                    case JumpDirection.Left:
                        _p1 = new Vector3(
                            playerPosition.x,
                            playerPosition.y + _jumpDistance,
                            playerPosition.z + _jumpDistance
                        );
                        _p2 = new Vector3(
                            playerPosition.x,
                            playerPosition.y,
                            playerPosition.z + _jumpDistance
                        );
                        break;
                    case JumpDirection.Right:
                        _p1 = new Vector3(
                            playerPosition.x + _jumpDistance,
                            playerPosition.y + _jumpDistance,
                            playerPosition.z
                        );
                        _p2 = new Vector3(
                            playerPosition.x + _jumpDistance,
                            playerPosition.y,
                            playerPosition.z
                        );
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (deltaDistance <= PlatformOutRadius)
            {
                switch (_jumpDirection)
                {
                    case JumpDirection.Left:
                        if (playerPosition.z + _jumpDistance < lastPlatformPosition.z)
                        {
                            _p1 = new Vector3(
                                playerPosition.x,
                                playerPosition.y + distance - 2,
                                lastPlatformPosition.z - 2
                            );
                            _p2 = new Vector3(
                                playerPosition.x,
                                playerPosition.y + lastPlatformPosition.y,
                                lastPlatformPosition.z - 2
                            );
                        }
                        else
                        {
                            _p1 = new Vector3(
                                playerPosition.x,
                                playerPosition.y + distance + 2,
                                lastPlatformPosition.z + 2
                            );
                            _p2 = new Vector3(
                                playerPosition.x,
                                playerPosition.y + lastPlatformPosition.y,
                                lastPlatformPosition.z + 2
                            );
                        }

                        break;
                    case JumpDirection.Right:
                        if (playerPosition.x + _jumpDistance < lastPlatformPosition.x)
                        {
                            _p1 = new Vector3(
                                lastPlatformPosition.x - 2,
                                playerPosition.y + distance - 2,
                                playerPosition.z
                            );
                            _p2 = new Vector3(
                                lastPlatformPosition.x - 2,
                                playerPosition.y + lastPlatformPosition.y,
                                playerPosition.z
                            );
                        }
                        else
                        {
                            _p1 = new Vector3(
                                lastPlatformPosition.x + 2,
                                playerPosition.y + distance + 2,
                                playerPosition.z
                            );
                            _p2 = new Vector3(
                                lastPlatformPosition.x + 2,
                                playerPosition.y + lastPlatformPosition.y,
                                playerPosition.z
                            );
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                StartCoroutine(Die(2.25f));
            }
            else
            {
                switch (_jumpDirection)
                {
                    case JumpDirection.Left:
                        _p1 = _p0 + (Vector3.forward + Vector3.up) * _jumpDistance;
                        _p2 = _p0 + Vector3.forward * _jumpDistance;
                        break;
                    case JumpDirection.Right:
                        _p1 = _p0 + (Vector3.right + Vector3.up) * _jumpDistance;
                        _p2 = _p0 + Vector3.right * _jumpDistance;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                StartCoroutine(Die(1));
            }
        }

        private IEnumerator Die(float timeToFallDown)
        {
            if (_playerState != PlayerState.Dead)
            {
                GameEventSystem.Instance.GameOver();
                ChangeState(PlayerState.Dead);
            }

            yield return new WaitForSeconds(timeToFallDown);

            _audioManager.Play("Fall_Down");
        }

        private void Jump()
        {
            if (_playerState == PlayerState.Jumping || _playerState == PlayerState.Dead)
            {
                if (_jumpTime < 1)
                {
                    _jumpTime += Time.deltaTime;

                    if (_jumpTime > 1)
                        _jumpTime = 1;

                    transform.position = QuadraticBezier(_p0, _p1, _p2, _jumpTime);
                }
            }
        }

        private static Vector3 QuadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            return Mathf.Pow(1 - t, 2) * p0 +
                   2f * (1 - t) * t * p1 +
                   Mathf.Pow(t, 2) * p2;
        }

        private void Idle()
        {
            if (_playerState == PlayerState.Stepped &&
                _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                ChangeState(PlayerState.Idle);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Platform"))
            {
                if (_playerState != PlayerState.Dead)
                {
                    transform.parent = other.transform;

                    _isGrounded = true;
                    _animator.SetTrigger(Step);

                    _audioManager.Play(_isSteppedCenter ? "Centered_Step" : "Step");

                    if (_isStarted)
                    {
                        ChangeState(PlayerState.Stepped);

                        _p0 = _p1 = _p2 = Vector3.zero;
                        _jumpDistance = MinJumpDistance;
                        _prepareJumpTime = 0;
                        _jumpTime = 0;

                        var platformGuid = other.gameObject.GetComponent<PlatformController>().Guid;
                        PlatformEventSystem.Instance.PlayerStepped(platformGuid, _isSteppedCenter);
                    }
                    else
                    {
                        ChangeState(PlayerState.Stepped);

                        _animator.Play(Step);
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Coin"))
            {
                _audioManager.Play("Coin_Pickup");
                GameEventSystem.Instance.PickupCoin();
                Destroy(other.gameObject);
            }
        }

        private void OnCollisionExit(Collision other)
        {
            transform.parent = null;
            _isGrounded = false;
        }

        private void ChangeState(PlayerState state)
        {
            PlayerEventSystem.Instance.ChangeState(_playerState = state);
        }
    }
}