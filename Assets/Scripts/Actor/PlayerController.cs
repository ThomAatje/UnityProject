using Assets.Scripts.Movement;
using UnityEngine;

namespace Assets.Scripts.Actor
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private IMotorControllable _motorControllable;
        private float _timeScale = 1f;
        private float _fixedDeltaTime;

        private void Awake()
        {
            _motorControllable = GetComponent<IMotorControllable>();
            _fixedDeltaTime = Time.fixedDeltaTime;
        }

        // Update is called once per frame
        private void Update()
        {
            _motorControllable.SetInput(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));

            if (Input.GetButtonDown("Fire2"))
            {
                _timeScale = 0.5f;
            }
            else if (Input.GetButtonUp("Fire2"))
            {
                _timeScale = 1.0f;
            }

            Time.timeScale = Mathf.Lerp(Time.timeScale, _timeScale, 0.08f);
        }

        private void FixedUpdate()
        {
            Time.fixedDeltaTime = _fixedDeltaTime * Time.timeScale;
        }
    }
}
