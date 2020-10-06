using Assets.Scripts.Actor;
using UnityEngine;

public class Gunbase : MonoBehaviour
{
    public float damage;
    public float range;
    public float fireDelta;

    public float nextFire;
    private float myTime = 0.0f;
    public Camera fpsCam;
    
    void Update()
    {
        myTime = myTime + Time.deltaTime;
        if (Input.GetButton("Fire1") && myTime > nextFire)
        {
            nextFire = myTime + fireDelta;
            Debug.Log(Time.time);
            Shoot();
            nextFire = nextFire - myTime;
            myTime = 0.0f;
        }
    }

    // Update is called once per frame
    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {

            Debug.Log(hit.transform.name);
            Character character = hit.transform.GetComponent(typeof(Character)) as Character;
            if (character != null)
            {
                character.TakeDamage(damage, true);
            }
        }
    }
}

