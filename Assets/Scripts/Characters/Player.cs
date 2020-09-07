using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    public class Player : Character
    {
        //TODO: Create Input Provider and Motor 
        // Added in a temp camera, not actual player

        private Vector3 velocity = new Vector3(0, 0, 0);


        private void Update()
        {
            HandleInput();

            gameObject.transform.Translate(velocity/10);
        }

        private void HandleInput()
        {
            velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        }
    }
}
