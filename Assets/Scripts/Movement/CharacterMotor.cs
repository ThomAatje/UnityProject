using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Movement
{
    public class CharacterMotor : Motor, IMotorControllable
    {
        public bool ForceAirSpeedDamping;
        public float AirDamping = 0.9f;
        public float AirControl = 0.5f;
        public float BackwardsSpeed = 1.0f;
        public float Acceleration = 0.3f;
        public float SlopeFactor = 1.0f;
        public float Damping = 0.17f;
        protected Vector2 Input;
        protected Vector3 Throttle;
        private Vector3 _moveDirection;

        public Vector2 GetInput()
        {
            return Input;
        }

        public void SetInput(Vector2 value)
        {
            Input = value.normalized;
        }

        protected override void Awake()
        {
            base.Awake();

            OnGroundContact.AddListener(() => ForceAirSpeedDamping = false);
        }

        protected virtual void FixedUpdate()
        {
            UpdateThrottle();
            UpdateForces();
            FixedMove();
        }

        protected virtual void UpdateThrottle()
        {
            UpdateThrottleGround();
        }

        protected virtual void UpdateThrottleGround()
        {
            var airSpeedModifier = 1.0f;

            if (!Grounded)
            {
                if (ForceAirSpeedDamping)
                {
                    airSpeedModifier = AirDamping;
                }
                else
                {
                    airSpeedModifier = Input != Vector2.zero ? AirControl : 0f;
                }
            }
            
            // convert horizontal input to forces in the motor
            Throttle += Input.x *
                        (transform.TransformDirection(Vector3.right * (Acceleration * Time.fixedDeltaTime) * airSpeedModifier) *
                         SlopeFactor);
            Throttle += (Input.y > 0 ? Input.y : Input.y * BackwardsSpeed) *
                        (transform.TransformDirection(Vector3.forward * (Acceleration * Time.fixedDeltaTime) * airSpeedModifier) *
                         SlopeFactor);

            // dampen motor force
            Throttle.x /= 1.0f + Damping * airSpeedModifier;
            Throttle.z /= 1.0f + Damping * airSpeedModifier;
        }

        protected override void FixedMove()
        {
            _moveDirection = Vector3.zero;
            _moveDirection += Throttle;
            _moveDirection.y += FallSpeed * Time.fixedDeltaTime;

            _moveDirection = new Vector3(
                Mathf.Abs(_moveDirection.x) < 0.0001f ? 0.0f : _moveDirection.x,
                Mathf.Abs(_moveDirection.y) < 0.0001f ? 0.0f : _moveDirection.y,
                Mathf.Abs(_moveDirection.z) < 0.0001f ? 0.0f : _moveDirection.z);

            Controller.Move(_moveDirection * Time.timeScale);

            base.FixedMove();
        }
    }
}