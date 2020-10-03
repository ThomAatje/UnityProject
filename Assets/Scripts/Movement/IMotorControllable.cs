using UnityEngine;

namespace Assets.Scripts.Movement
{
    public interface IMotorControllable
    {
        Vector2 GetInput();
        void SetInput(Vector2 value);
    }
}