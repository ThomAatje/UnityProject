﻿using Assets.Scripts.Movement;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerLook : MonoBehaviour
    {
        private Vector2 _mouseAbsolute;
        private Vector2 _smoothMouse;
        public Vector2 ClampInDegrees = new Vector2(360, 180);
        public bool LockCursor;
        public Vector2 Sensitivity = new Vector2(2, 2);
        public Vector2 Smoothing = new Vector2(3, 3);
        public Vector2 TargetDirection;
        public Vector2 TargetCharacterDirection;

        public float HeightOffset = 1f;

        public float SmoothTime = 1.0f;
        // Assign this if there's a parent object controlling motion, such as a Character Controller.
        // Yaw rotation will affect this object instead of the camera if set.
        public GameObject CharacterBody;

        private void Start()
        {
            // Set target direction to the camera's initial orientation.
            TargetDirection = transform.localRotation.eulerAngles;

            if (!CharacterBody)
                CharacterBody = GameObject.FindGameObjectWithTag("Player");

            // Set target direction for the character body to its inital state.
            if (CharacterBody)
                TargetCharacterDirection = CharacterBody.transform.localRotation.eulerAngles;
        }

        private void LateUpdate()
        {
            // Ensure the cursor is always locked when set
            if (LockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            // Allow the script to clamp based on a desired target value.
            var targetOrientation = Quaternion.Euler(TargetDirection);
            var targetCharacterOrientation = Quaternion.Euler(TargetCharacterDirection);

            // Get raw mouse input for a cleaner reading on more sensitive mice.
            var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

            // Scale input against the sensitivity setting and multiply that against the smoothing value.
            mouseDelta = Vector2.Scale(mouseDelta, new Vector2(Sensitivity.x * Smoothing.x, Sensitivity.y * Smoothing.y));

            // Interpolate mouse movement over time to apply smoothing delta.
            _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / Smoothing.x);
            _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / Smoothing.y);

            // Find the absolute mouse movement value from point zero.
            _mouseAbsolute += _smoothMouse;

            // Clamp and apply the local x value first, so as not to be affected by world transforms.
            if (ClampInDegrees.x < 360)
                _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -ClampInDegrees.x * 0.5f, ClampInDegrees.x * 0.5f);

            // Then clamp and apply the global y value.
            if (ClampInDegrees.y < 360)
                _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -ClampInDegrees.y * 0.5f, ClampInDegrees.y * 0.5f);

            transform.localRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right) * targetOrientation;

            CharacterBody.transform.localRotation = Quaternion.AngleAxis(_mouseAbsolute.x, Vector3.up) * targetCharacterOrientation;

            transform.localRotation *= Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));

            transform.position =
                Vector3.Lerp(transform.position, CharacterBody.transform.position + Vector3.up * HeightOffset, Time.deltaTime * SmoothTime);
        }
    }
}
