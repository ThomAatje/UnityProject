using Assets.Scripts.Activities;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Movement
{
    public class PlayerMotor : CharacterMotor
    {
        [Header("Jumping")]
        [SerializeField] private UnityEvent _onJump;
        [SerializeField] private bool _jumpAutomatic = false;
        [SerializeField] private float _jumpForce = 0.18f;
        [SerializeField] private float _jumpForceDamping = 0.08f;
        [SerializeField] private float _jumpForceHoldDamping = 0.2f;
        private bool _jumpReady = true;

        [Header("Double Jumping")]
        [SerializeField] private UnityEvent _onDoubleJump;
        [SerializeField] private float _doubleJumpForce = 0.18f;
        [SerializeField] private float _doubleJumpMinimumAirTime = 0.2f;
        [SerializeField] private int _doubleJumpAmount = 1;
        private int _doubleJumpCurrentAmount;
        private float _doubleJumpNextTimestamp;

        [Header("Dashing")]
        [SerializeField] private UnityEvent _onDashChargedFull;
        [SerializeField] private UnityEvent _onDashChargedPartial;
        [SerializeField] private UnityEvent _onDash;
        [SerializeField] private int _dashDefaultAmount = 2;
        [SerializeField] private float _dashDefaultChargeTime = 2.0f;
        [SerializeField] private float _dashChargeTimeModifier = 0f;
        [SerializeField] private float _dashDelay = 0.2f;
        [SerializeField] private float _dashDistance = 5f;
        [SerializeField] private float _dashDrag = 0.2f;
        private int _dashCurrentAmount = 2;
        private bool _dashCharged;
        private float _dashNextTimestamp;
        private float _dashCurrentChargeTime = 3.0f;

        //Activities
        private IDashActivityProvider _dashActivityProvider;
        private IJumpActivityProvider _jumpActivityProvider;
        private Activity _jumpActivity;
        private Activity _dashActivity;

        protected override void Awake()
        {
            base.Awake();

            _dashActivityProvider = GetComponent<IDashActivityProvider>();
            _jumpActivityProvider = GetComponent<IJumpActivityProvider>();
            _dashActivity = _dashActivityProvider.GetDashActivity();
            _jumpActivity = _jumpActivityProvider.GetJumpActivity();
        }

        protected override void FixedUpdate()
        {
            UpdateThrottle();
            UpdateForces();
            UpdateJumpActivity();
            UpdateDashActivity();
            FixedMove();
        }

        private void UpdateJumpActivity()
        {
            UpdateJumpOnGround();
            Throttle.y /= 1f + _jumpForceDamping;
        }

        private void UpdateJumpOnGround()
        {
            if (_jumpActivity.Active == false)
            {
                _jumpReady = true;
                return;
            }

            if (Grounded == false)
            {
                if (Time.time > _doubleJumpNextTimestamp && _doubleJumpCurrentAmount > 0 && _jumpReady)
                {
                    FallSpeed = 0f;
                    Throttle.y = _doubleJumpForce * 2 / Time.timeScale;

                    _onDoubleJump?.Invoke();
                    _jumpReady = false;
                    _doubleJumpCurrentAmount--;
                    _doubleJumpNextTimestamp = Time.time + _doubleJumpMinimumAirTime;
                }

                return;
            }

            _doubleJumpCurrentAmount = _doubleJumpAmount;

            if (_jumpAutomatic || _jumpReady)
            {
                _onJump?.Invoke();
                _doubleJumpNextTimestamp = Time.time + _doubleJumpMinimumAirTime;
                _jumpReady = false;

                Throttle.y = _jumpForce / Time.timeScale;
            }
        }

        private void UpdateDashActivity()
        {
            if (_dashCurrentChargeTime < _dashDefaultChargeTime)
            {
                _dashCurrentChargeTime += Time.fixedDeltaTime * (1f + _dashChargeTimeModifier) * Time.timeScale;
                if (_dashCurrentAmount > 1)
                {
                    _dashCurrentAmount = Mathf.RoundToInt(Mathf.Clamp(_dashCurrentChargeTime / (_dashDefaultChargeTime / _dashDefaultAmount), 0, _dashDefaultAmount));
                }
            }

            if (_dashCurrentChargeTime >= _dashDefaultChargeTime && _dashCharged == false && Grounded)
            {
                if (_dashCurrentAmount == 0)
                {
                    _onDashChargedFull?.Invoke();
                }
                else
                {
                    _onDashChargedPartial?.Invoke();
                }

                _dashCurrentAmount = _dashDefaultAmount;
                _dashCharged = true;
            }

            if (!_dashActivity.Active)
            {
                return;
            }

            if (_dashCurrentAmount == 0 || Time.time < _dashNextTimestamp)
            {
                return;
            }

            ForceAirSpeedDamping = true;
            FallSpeed = 0f;
            _dashCharged = false;
            _dashCurrentAmount--;
            _dashNextTimestamp = Time.time + _dashDelay;
            _dashCurrentChargeTime = Mathf.Max(_dashCurrentChargeTime - _dashDefaultChargeTime / _dashDefaultAmount, 0);
            _onDash?.Invoke();

            var direction = Input == Vector2.zero ? transform.forward : Throttle.normalized;
            Throttle += Vector3.Scale(
                    direction,
                    _dashDistance * new Vector3(Mathf.Log(1f / (Time.fixedDeltaTime * _dashDrag + 1)) / -Time.fixedDeltaTime,
                    0,
                    Mathf.Log(1f / (Time.fixedDeltaTime * _dashDrag + 1)) / -Time.fixedDeltaTime)) / Time.timeScale;
        }
    }
}
