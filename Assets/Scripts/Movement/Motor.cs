using Assets.Scripts.Actions;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Movement
{
    /// <summary>
    /// Handles gravity for the character controller and fires events at ground or ceiling collisions.
    /// </summary>
    public abstract class Motor : MonoBehaviour
    {
        [Header("Ceiling Contact")]
        [SerializeField]
        private bool _ceilingContact;
        public float HeadContactAtDistance = 0.3f;
        public Transform CeilingHitTransform;
        public Transform PreviousCeilingHitTransform;
        public UnityEvent OnCeilingContact;
        public UnityEvent OnCeilingContactSeparation;

        [Header("Ground Contact")]
        [SerializeField]
        private bool _grounded;
        public float CurrentHitDistance;
        public float GroundedAtDistance = 1f;
        public Transform GroundHitTransform;
        public Transform PreviousGroundHitTransform;
        public UnityEvent OnGroundContact;
        public UnityEvent OnGroundContactSeparation;

        [Header("Physics")]
        public CharacterController Controller;
        public float GravityModifier = 0.25f;
        public float FallSpeed;


        public bool HeadContact
        {
            get => _ceilingContact;
            set
            {
                if (value && _ceilingContact == false)
                {
                    OnCeilingContact?.Invoke();
                    _ceilingContact = true;
                }
                else if (value == false && _grounded)
                {
                    OnCeilingContactSeparation?.Invoke();
                    _ceilingContact = false;
                }
            }
        }

        public bool Grounded
        {
            get => _grounded;
            set
            {
                if (value && _grounded == false)
                {
                    OnGroundContact?.Invoke();
                    _grounded = true;
                }
                else if (value == false && _grounded)
                {
                    OnGroundContactSeparation?.Invoke();
                    _grounded = false;
                }

            }
        }

        protected virtual void Awake()
        {
            if (Controller == null)
            {
                Controller = GetComponent<CharacterController>();
            }
        }

        protected virtual void FixedMove()
        {
            StoreGroundInfo();
            StoreCeilingInfo();
        }

        protected virtual void UpdateForces()
        {
            var gravity = Physics.gravity.y * (1f + GravityModifier) * Time.timeScale * Time.fixedDeltaTime;

            if (Grounded && FallSpeed <= 0.0f)
            {
                FallSpeed = gravity;
                return;
            }

            FallSpeed += gravity;
        }

        protected virtual void StoreGroundInfo()
        {
            if (GroundHitTransform != null)
            {
                PreviousGroundHitTransform = GroundHitTransform;
            }

            if (Physics.SphereCast(transform.position, Controller.radius, Vector3.down, out var hit, 5))
            {
                CurrentHitDistance = hit.distance;
                GroundHitTransform = hit.transform;

                if (Controller.isGrounded)
                {
                    var collisionAction = hit.transform.GetComponent<IGroundCollisionAction>();
                    collisionAction?.OnCollision(this);
                    Grounded = true;
                }
                else
                {
                    var hoverAction = hit.transform.GetComponent<IGroundHoverAction>();
                    hoverAction?.OnHover(this);

                    if (hit.distance >= GroundedAtDistance)
                    {
                        Grounded = false;
                    }
                }
            }
            else
            {
                Grounded = false;
                GroundHitTransform = null;
            }
        }

        protected virtual void StoreCeilingInfo()
        {
            if (CeilingHitTransform != null)
            {
                PreviousCeilingHitTransform = CeilingHitTransform;
            }

            if (Physics.SphereCast(transform.position, Controller.radius, Vector3.up, out var hit, 5))
            {
                HeadContact = hit.distance < HeadContactAtDistance;
                CeilingHitTransform = hit.transform;

                if (HeadContact)
                {
                    var collisionAction = hit.transform.GetComponent<ICeilingCollisionAction>();
                    collisionAction?.OnCollision(this);
                }
                else
                {
                    var hoverAction = hit.transform.GetComponent<ICeilingHoverAction>();
                    hoverAction?.OnHover(this);
                }
            }
            else
            {
                HeadContact = false;
                CeilingHitTransform = null;
            }
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Debug.DrawLine(transform.position, transform.position + Vector3.down * CurrentHitDistance);
            Gizmos.DrawWireSphere(transform.position + Vector3.down * CurrentHitDistance, Controller.radius);
        }
    }
}
