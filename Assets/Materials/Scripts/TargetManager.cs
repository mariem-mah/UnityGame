using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TargetManager : MonoBehaviour
{
    public GameObject targetPerfab;
    public float spawnDelay = 2f;
    public float timeBetweenSpawnsMin = 1f;
    public float timeBetweenSpawnsMax = 5f;

    public float SpawnRadius = 10f;
    public float maxSpawnHeight = 40f;
    public int maxNumTargets = 20;

    [Range(0, 1), Tooltip("How much % of point value is removed when target is at stopping distance (0 = 0%, 1 = 100%)")]
    public float pointsValueLoss;

    public PointsDisplay pointsDisplay;

    private List<Target> spawendTargets = new List<Target> ();
    private Queue<Target> inactiveTargets = new Queue<Target> ();

    public Queue<Target> InactiveTargets
    {
        get { return inactiveTargets; }
    }
    void Awake()
    {
        // Disable on game start because this is controlled by game manager
        this.enabled = false;
    }
    void OnEnable ()
    {
       // InitTargets ();
        StartCoroutine (SpawnTarget ());
    }
    void OnDisable ()
    {
        StopCoroutine (SpawnTarget ());
        ResetAllTargets ();
    }
    public void InitTargets ()
    {

        //TEMP:store player
      //  GameObject player = GameObject.FindGameObjectWithTag("Player") as GameObject; 

        //create target parent game object (for a cleaner outline)
        GameObject targetParent = new GameObject();
        targetParent.name = "Targets";

        //Instantiate all targets
        for (int i = 0; i < maxNumTargets; i++)
        {
            Target targetInstance = (Instantiate (targetPerfab) as GameObject).GetComponent<Target> ();

            //Register target to manager 
            targetInstance.Targetmanager = this;

            //Set parent 
            targetInstance.transform.parent = targetParent.transform;

            //Set player target
          //  targetInstance.Player = player;
            targetInstance.Player = GameManager.instance.player;

            //Initialize target
            targetInstance.InitTarget ();

            //Add to target  lists 
            spawendTargets.Add (targetInstance);
        }
        ResetAllTargets();
    }
    private IEnumerator SpawnTarget ()
    { //wait before spawing 
        yield return new WaitForSeconds(spawnDelay);
        while (this.isActiveAndEnabled)
        {
            if (inactiveTargets.Count > 0)
            {
                //Get inactive target from queue
                Target target = inactiveTargets.Dequeue();

                //Move target to position and make sure it is visible for the player 
                Vector3 position;

                do
                {
                    position = transform.position + Random.onUnitSphere * SpawnRadius;   // **********************************************************************random
                } while (position.y < transform.position.y || position.y > maxSpawnHeight);
                target.transform.position = position;

                //Activate target
             //   target.Active();
                target.Activate();
            }

            // Get random wait time
            float waitTime = Random.Range(timeBetweenSpawnsMin, timeBetweenSpawnsMax);
            yield return new WaitForSeconds(waitTime);
        }

    }
    private void ResetAllTargets ()
    {

        // Clear targets queue 
        inactiveTargets.Clear ();

        //Reset each target

        foreach (Target target in spawendTargets)
        {
            target.Reset ();
        }
    }

}