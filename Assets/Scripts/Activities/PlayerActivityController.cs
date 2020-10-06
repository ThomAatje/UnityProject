using UnityEngine;

namespace Assets.Scripts.Activities
{
    public class PlayerActivityController : MonoBehaviour, IJumpActivityProvider, IDashActivityProvider
    {
        public Activity Jump = new Activity("Jump");
        public Activity Dash = new Activity("Dash");
        public Activity GetDashActivity()
        {
            return Dash;
        }

        public Activity GetJumpActivity()
        {
            return Jump;
        }

        private void Update()
        {
            UpdateActivity(Jump);
            UpdateActivity(Dash);
        }

        private static void UpdateActivity(Activity activity)
        {
            if (Input.GetButtonDown(activity.Name))
            {
                activity.Active = true;
            }
            else if (Input.GetButtonUp(activity.Name))
            {
                activity.Active = false;
                activity.ActiveTime = 0;
            }

            activity.AccumulateActiveTime();
        }
    }
}
