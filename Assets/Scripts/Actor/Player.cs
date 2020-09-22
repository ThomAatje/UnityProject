using Assets.Scripts.Core;
using Assets.Scripts.Movement;
using UnityEngine;

namespace Assets.Scripts.Actor
{
    public class Player : Character
    {
        [SerializeField] private PlayerMotor _motor;
        [SerializeField] private InputAxisProvider _input;
        [SerializeField] private PlayerLook _look;

        private void Awake()
        {
            _motor = GetComponent<PlayerMotor>();
            _input = GetComponent<InputAxisProvider>();
        }

        private void Update()
        {
            var axis = _input.GetMovementAxis();

            _look.RollToAxis(axis);
            _motor.CreateMoveDirection(axis);
            
            if (_input.JumpButton)
                _motor.Jump();

            _motor.Move();
        }
    }
}
