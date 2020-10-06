using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Assets.Scripts.Actor
{
    public class Zombie : Character
    {

        private const double UPDATE_TIME = 0.033;
        private double updateDeltaTime = 0;

        private NavMeshAgent navMeshAgent;

        /// <summary>
        /// This should be the player most of the time, currently public for testing
        /// </summary>
        public GameObject targetObject;
        public Vector3 goalPosition;

        // Start is called before the first frame update
        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            targetObject = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            updateDeltaTime += Time.deltaTime;
            if (updateDeltaTime >= UPDATE_TIME)
            {
                navMeshAgent.SetDestination(targetObject.transform.position);
                updateDeltaTime = 0;
            }
                
        }

        protected override void Die(GameObject sender)
        {
            base.Die(sender);
            Destroy(this);
        }
    }
}

