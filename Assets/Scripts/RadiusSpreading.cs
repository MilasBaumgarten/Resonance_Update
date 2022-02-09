//Author: Maximilian Rietzsch
//
//add this skript to source gameobject (p.e fire source)
//usable with "FlammableFire" tagged objects
//spawning prefabs successive circular within defined area
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireSpread
{
    public class RadiusSpreading : MonoBehaviour
    {
        #region Globals
        [Header("Spread Setup")]
        [Space(8)]

        [Tooltip("How many objects can spawn in first circle?")]
        [SerializeField]
        private int objectQuantity = 12;

        [Tooltip("Number of objects added per iteration.")]
        [SerializeField]
        private int objectsIncrease = 10;

        [Tooltip("Defines the distance from gameobject to first instantiated prefab.")]
        [SerializeField]
        private float startRadius = 4.0f;

        [Tooltip("Defines the distance added per iteration.")]
        [SerializeField]
        private float radiusIncrease = 2f;

        [Tooltip("Do you want a random spawn distance offset within each circle?")]
        [SerializeField]
        private bool randomOffset = false;

        [Space(10)]
        [Tooltip("How many objects can spawn maximally? ")]
        [SerializeField]
        private int maxObjects = 100;

        [Space(10)]
        [Tooltip("Time between each iteration.")]
        [SerializeField]
        [Range(0.1f, 60)]
        private float spawnTimeInterval;

        [Space(10)]
        [Tooltip("Area in which the prefabs can spawn.")]
        [SerializeField]
        private GameObject effectArea;

        [Space(10)]
        [Tooltip("Time between extinguishing and start respawn.")]
        [SerializeField]
        private float timeCounter;

        [Tooltip("The object to be spawned.")]
        [SerializeField]
        private GameObject spawnPrefab;

        private MeshRenderer[] areaRenderer;
        private List<Bounds> areaBounds;

        private Transform[] effectAreas;
        private List<Transform> circles = new List<Transform>();

        private float timeCounterStart;
        private int circleIndex = 0;
        private bool inAction = false;              // true if extinguishing process is active

        private int objectCount;
        #endregion

        private void Start()
        {
            timeCounterStart = timeCounter;        // holds extiguishing process duration

            // defines the spreading area                                    
            #region Area Definition
            areaBounds = new List<Bounds>();
            effectAreas = effectArea.GetComponentsInChildren<Transform>();
            areaRenderer = effectArea.GetComponentsInChildren<MeshRenderer>();

            // deactivates mesh renderer for each area
            foreach (MeshRenderer mesh in areaRenderer)
            {
                mesh.enabled = false;
            }

            SetEffectArea();
            #endregion

            StartCoroutine(SuccessiveSpawn());
        }

        private void Update()
        {
            InAction();
        }

        /// <summary>
        /// add bounds of specified meshs to list
        /// </summary>
        private void SetEffectArea()
        {
            foreach (Transform child in effectAreas)
            {
                if (child != effectAreas[0])        //ignores transform of parent object (because GetComponentsInChildren<Transform>() contains parent transform)
                {
                    areaBounds.Add(child.GetComponent<MeshRenderer>().bounds);
                }
            }
        }

        /// <summary>
        /// calculates time of extinguishing after last activity (p.e "extinguished" flame object)
        /// </summary>
        private void InAction()
        {
            if (inAction)
            {
                timeCounter -= Time.deltaTime;

                if (timeCounter < 0)
                {
                    inAction = false;
                    StartCoroutine(Respawn());
                }
                else
                    StopCoroutine(Respawn());
            }
        }

        /// <summary>
        /// reset the current time
        /// </summary>
        private void CounterReset()
        {
            timeCounter = timeCounterStart;
        }

        /// <summary>
        /// defines a new virtual spawn circle
        /// </summary>
        /// <param name="center">position of the center of circle </param>
        /// <param name="radius">radius between center of circle and circle outline</param>
        /// <param name="a">angle multiplier of radian converter | angle between spawning objectes at given radius</param>
        /// <returns></returns>
        private Vector3 SpawnPoint(Vector3 center, float radius, int a)
        {
            Vector3 pos;
            pos.x = center.x + radius * Mathf.Sin(a * Mathf.Deg2Rad);
            pos.y = effectArea.transform.position.y;
            pos.z = center.z + radius * Mathf.Cos(a * Mathf.Deg2Rad);

            return pos;
        }

        /// <summary>
        /// method is called by extinguished object
        /// starts extinguishing process | stops object respawn | reset process duration
        /// </summary>
        /// <param name="ex">is extinguishing or not</param>
        public void Extinguishing(bool ex)
        {
            if (ex)
            {
                inAction = true;
                StopAllCoroutines();
                CounterReset();
            }
            else
                inAction = false;
        }

        /// <summary>
        /// spawns fire objects in a specific interval
        /// </summary>
        private IEnumerator SuccessiveSpawn()
        {
            int lastObjectCount = -1;   // saves active fire object duration of last transition

            // active while currently spawned object number is less then given maximum oject quantity
            #region Spawn Process
            while (objectCount < maxObjects && objectCount > lastObjectCount)
            {
                yield return new WaitForSeconds(spawnTimeInterval);    // waits for {spawnTimeScale} seconds
                lastObjectCount = objectCount;                         // set object count of last transition to current object count

                GameObject circleParent = new GameObject(("EffectCircle" + circleIndex));  // compiles a new circle as gameobject
                circleParent.transform.SetParent(this.transform);                          // adds circle as a child of this object

                // calculate position for each gameobject to get spawned in circle
                for (int i = 0; i < objectQuantity; i++)
                {
                    int a = i * 360 / objectQuantity + (randomOffset ? Random.Range(10, 50) : 0);   // if {randomOffset}, spawn gameobjects in random distance to eachother | if !{randomOffset} spawn gameobjects in periodic distance 
                    Vector3 pos = SpawnPoint(transform.position, startRadius, a);                   // calculates the object position

                    // if {pos} inside declared bounds and tagged as "FlammableFire"
                    foreach (Bounds bound in areaBounds)
                    {
                        if (bound.Contains(pos))
                        {
                            if (spawnPrefab.tag.Equals("FlammableFire"))
                            {
                                Instantiate(spawnPrefab, pos, Quaternion.identity, circleParent.transform);
                                objectCount++;
                            }
                        }
                    }
                }
                circles.Add(circleParent.transform);    // adds current circle to circle list 

                if (circleParent.transform.childCount == 0)
                {
                    Destroy(circleParent);
                }

                circleIndex++;
                startRadius += radiusIncrease;
                objectQuantity += objectsIncrease;
            }
            #endregion
        }

        /// <summary>
        /// respawn extinguished fire objects
        /// </summary>
        /// <returns></returns>
        private IEnumerator Respawn()
        {
            foreach (Transform circle in circles)
            {
                Transform[] list = circle.GetComponentsInChildren<Transform>(true);

                foreach (Transform fireBatch in list)
                {
                    if (fireBatch.tag.Equals("FlammableFire"))
                    {
                        fireBatch.gameObject.SetActive(true);
                    }
                }
                yield return new WaitForSeconds(spawnTimeInterval);
            }
        }
    }
}

