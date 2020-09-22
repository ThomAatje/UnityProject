using UnityEngine;

namespace Assets.Scripts.Core
{
    public class InputAxisProvider : MonoBehaviour, IInputProvider
    {
        public bool JumpButton => Input.GetButton("Jump");
        //public bool RunButton => Input.GetKey(KeyCode.LeftShift);

        public Vector2 GetMovementAxis()
        {
            var x = Input.GetAxisRaw("Horizontal");
            var y = Input.GetAxisRaw("Vertical");

            return new Vector2(x, y);
        }
    }
}
