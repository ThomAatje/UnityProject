using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts.Movement
{
    public class PlayerMotor : MonoBehaviour
    {
        public Vector3 Drag;
        public float DashDistance = 5f;
        public AudioClip JumpAudioClip;
        public AudioClip DoubleJumpAudioClip;
        public float DoubleJumpTimeout = 0.2f;
        public float DoubleJumpMark = 0;
        public float DashMark;
        public float DashTimeout = 0.3f;
        public AudioClip DashAudioClip;
        public AudioSource Source;

        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Vector3 _moveDirection = Vector3.zero;

        [Header("Motor Properties")]
        [SerializeField] private float _walkingSpeed = 4f;
        [SerializeField] private float _runningSpeed = 8f;
        [SerializeField] private float _jumpSpeed = 10f;
        [SerializeField] private float _gravity = 20;
        [SerializeField] private bool _alwaysRun = false;
        [SerializeField] private float _smoothTime = 0.15f;

        private int _jumpCount = 1;
        private int _dashCount = 1;
        private bool _isRunning = false;
        private Vector3 _velocity = Vector3.zero;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        public void CreateMoveDirection(Vector2 axis)
        {
            var movementSpeed = _alwaysRun || _isRunning ? _runningSpeed : _walkingSpeed;

            var forward = transform.TransformDirection(Vector3.forward);
            var right = transform.TransformDirection(Vector3.right);

            var direction = (right * axis.x + forward * axis.y).normalized * movementSpeed;
            var moveDirectionY = _moveDirection.y;

            _moveDirection = Vector3.SmoothDamp(_moveDirection, direction, ref _velocity, _smoothTime);
            _moveDirection.y = moveDirectionY;
        }

        public void Update()
        {
         
        }

        public void Jump()
        {
            if (_characterController.isGrounded)
            {
                //_jumpCount = 1;
                _moveDirection.y = _jumpSpeed;
                Source.PlayOneShot(JumpAudioClip);
                //DoubleJumpMark = Time.time + DoubleJumpTimeout;
            }
            /*
            else
            {
                if (_jumpCount > 0 && Time.time > DoubleJumpMark)
                {
                    _moveDirection.y = _jumpSpeed;
                    Source.PlayOneShot(DoubleJumpAudioClip);
                    _jumpCount--;
                }
            }
            */
        }

        public void Run()
        {
            if (_characterController.isGrounded)
                _isRunning = true;
        }

        public void Dash()
        {

            if (Time.time > DashMark && _dashCount > 0)
            {
                DashMark = Time.time + DashTimeout;
                Source.PlayOneShot(DashAudioClip);

                _moveDirection += Vector3.Scale(_moveDirection.normalized,
                    DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * Drag.x + 1)) / -Time.deltaTime),
                        0,
                        (Mathf.Log(1f / (Time.deltaTime * Drag.z + 1)) / -Time.deltaTime)));

                if (!_characterController.isGrounded)
                    _moveDirection.y += _jumpSpeed / 2;
            }
        }

        public void Walk()
        {
            if (_characterController.isGrounded)
                _isRunning = false;
        }

        public void Move()
        {
            if (!_characterController.isGrounded)
                _moveDirection.y -= _gravity * Time.deltaTime;
            else if (_moveDirection.y < 1)
                _moveDirection.y = 0;

            _characterController.Move(_moveDirection * Time.deltaTime);
        }
    }
}
