using UnityEngine;

namespace Assets.Scripts.Core
{
    public interface IInputProvider
    {
        Vector2 GetMovementAxis();
    }
}
