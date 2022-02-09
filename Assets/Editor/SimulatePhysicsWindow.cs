using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CMGTools
{
    [System.Serializable]
    public class SimulatePhysicsWindow : EditorWindow
    {
        #region Serializables
        [SerializeField]
        List<SimulatedBody> p_previousSimulated1;

        [SerializeField]
        List<SimulatedBody> p_previousSimulated2;

        [SerializeField]
        List<SimulatedBody> p_previousSimulated3;

        [SerializeField]
        int maxIterations = 1000;

        [SerializeField]
        bool realTime = true;

        [SerializeField]
        GameObject pointObject;

        [SerializeField]
        Vector2 forceMinMax;

        [SerializeField]
        Vector3 forceAngleInDegrees = new Vector3(0, 0, 0);

        [SerializeField]
        bool randomizeForceAngle;

        [SerializeField]
        float noFallOffArea = 1;

        [SerializeField]
        float explosionArea = 10;

        [SerializeField]
        List<VisualObjects> visualisationLines;

        [SerializeField]
        bool visualiseDirections = true;

        [SerializeField]
        bool resetCannon;

        [SerializeField]
        Vector3 resultingForce;

        [SerializeField]
        state currentState;
        #endregion

        #region Privates
        List<SimulatedBody>[] simulated;
        List<SimulatedBody> simulatedBodies;
        List<SimulatedBody> resetBodies;
        List<Rigidbody> generatedRigidbodies;
        List<Collider> generatedColliders;
        IEnumerator sim;
        bool repaint;
        #endregion

        #region Statics
        static List<SimulatedBody>[] previousSimulated;
        static int previousIndex;
        static int stackedSimulations = 0;
        static GUIStyle buttonDisabled;
        static float cooldown = 0;
        #endregion

        enum state
        {
            force,
            explosion,
            cannon
        }

        [System.Serializable]
        struct SimulatedBody
        {
            [SerializeField]
            public Rigidbody rigidbody;

            [SerializeField]
            Vector3 originalPosition;

            [SerializeField]
            Quaternion originalRotation;

            [SerializeField]
            Transform transform;

            [SerializeField]
            public bool isKinematic;

            public SimulatedBody(Rigidbody rigidbody)
            {
                this.rigidbody = rigidbody;
                transform = rigidbody.transform;
                originalPosition = rigidbody.position;
                originalRotation = rigidbody.rotation;
                isKinematic = rigidbody.isKinematic;
            }

            public void Reset()
            {
                transform.position = originalPosition;
                transform.rotation = originalRotation;
                if (rigidbody != null)
                {
                    rigidbody.velocity = Vector3.zero;
                    rigidbody.angularVelocity = Vector3.zero;
                    rigidbody.isKinematic = isKinematic;
                }
            }
        }

        [MenuItem("Tools/SimulatePhysics")]
        static void OpenWindow()
        {
            GetWindow<SimulatePhysicsWindow>("SimulatePhysics").Init();
        }

        private void Init()
        {
            simulated = previousSimulated;
        }

        private void OnGUI()
        {
            if (simulated == null)
            {
                simulated = FetchSimulated();

                previousIndex = 0;
            }

            Vector2 forceMinMaxOld = forceMinMax;

            Vector3 forceAngleInDegreesOld = forceAngleInDegrees;

            float falloffOld = noFallOffArea;

            float explosionAreaOld = explosionArea;

            bool visualiseDirectionsOld = visualiseDirections;

            int maxIterationsOld = maxIterations;

            state oldState = currentState;

            float width = EditorGUIUtility.currentViewWidth;

            GUILayout.Label("Physics Simulation", EditorStyles.boldLabel);

            GUILayout.Label("Select objects for simulation and click \"Simulate\".");

            GUILayout.Space(5);

            maxIterations = EditorGUILayout.IntField("Max iterations: ", maxIterations, GUILayout.Width(Mathf.Min(250, width)));

            realTime = EditorGUILayout.Toggle("Simulate in Realtime: ", realTime);

            visualiseDirections = EditorGUILayout.Toggle("Visualise Directions: ", visualiseDirections);

            if (maxIterations <= 0)
            {
                maxIterations = 1000;
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.BeginHorizontal();

            GUIStyle button = new GUIStyle(GUI.skin.button);

            buttonDisabled = new GUIStyle(GUI.skin.button);

            buttonDisabled.normal.textColor = Color.gray;

            if (GUILayout.Button("Simple Force", currentState == state.force ? buttonDisabled : button, GUILayout.Width(width / 3)))
            {
                if (currentState != state.force)
                {
                    currentState = state.force;
                }
            }

            if (GUILayout.Button("Explosion", currentState == state.explosion ? buttonDisabled : button, GUILayout.Width(width / 3)))
            {
                if (currentState != state.explosion)
                {
                    currentState = state.explosion;
                }
            }

            if (GUILayout.Button("Cannon", currentState == state.cannon ? buttonDisabled : button, GUILayout.Width(width / 3)))
            {
                if (currentState != state.cannon)
                {
                    currentState = state.cannon;
                }
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            switch (currentState)
            {
                case state.force:

                    forceMinMax.x = EditorGUILayout.FloatField("Force Min:", forceMinMax.x, GUILayout.Width(200));

                    forceMinMax.y = EditorGUILayout.FloatField("Force Max:", forceMinMax.y, GUILayout.Width(200));

                    if (forceMinMax.x > forceMinMax.y)
                    {
                        forceMinMax.y = forceMinMax.x;
                    }

                    if (!randomizeForceAngle)
                    {
                        ForceAngle(width);
                    }

                    randomizeForceAngle = EditorGUILayout.Toggle("Randomize Force Angle ", randomizeForceAngle);
                    break;
                case state.explosion:
                    pointObject = (GameObject) EditorGUILayout.ObjectField("Explosion center", pointObject, typeof(GameObject), true, null);

                    forceMinMax.x = EditorGUILayout.FloatField("Force:", forceMinMax.x, GUILayout.Width(200));

                    noFallOffArea = EditorGUILayout.FloatField("No Falloff Area:", noFallOffArea, GUILayout.Width(200));

                    explosionArea = EditorGUILayout.FloatField("Explosion Area:", explosionArea, GUILayout.Width(200));

                    if (explosionArea <= 0)
                    {
                        explosionArea = noFallOffArea;
                    }

                    if (noFallOffArea > explosionArea)
                    {
                        noFallOffArea = explosionArea;
                    }

                    if (pointObject != null && PrefabUtility.GetPrefabType(pointObject) == PrefabType.Prefab)
                    {
                        pointObject = null;
                    }
                    break;
                case state.cannon:
                    pointObject = (GameObject) EditorGUILayout.ObjectField("Cannon object", pointObject, typeof(GameObject), true, null);

                    forceMinMax.x = EditorGUILayout.FloatField("Force:", forceMinMax.x, GUILayout.Width(200));

                    ForceAngle(width);

                    if (pointObject != null && PrefabUtility.GetPrefabType(pointObject) == PrefabType.Prefab)
                    {
                        pointObject = null;
                    }

                    resetCannon = EditorGUILayout.Toggle("Reset Cannon ", resetCannon);

                    break;
                default:
                    break;
            }

            GUILayout.Space(20);

            if (sim == null)
            {
                if (cooldown > Time.realtimeSinceStartup)
                {
                    GUILayout.Button("Simulate", buttonDisabled);
                }
                else if (GUILayout.Button("Simulate"))
                {
                    PrepareSim();
                }

                if (stackedSimulations > 0)
                {
                    GUILayout.Space(10);

                    GUILayout.Label("Stored simulations (max 3): " + stackedSimulations);

                    GUILayout.Space(10);

                    if (GUILayout.Button("Reset"))
                    {
                        ResetAllBodies();
                    }
                }
            }
            else
            {
                if (GUILayout.Button("Stop Simulation"))
                {
                    EndSim();
                }
            }

            if (visualiseDirections && !visualiseDirectionsOld)
            {
                OnSelectionChange();
            }

            if (forceAngleInDegreesOld != forceAngleInDegrees || forceMinMaxOld != forceMinMax || oldState != currentState || falloffOld != noFallOffArea || explosionArea != explosionAreaOld || maxIterations != maxIterationsOld)
            {
                RecalculatePoints();
            }
        }

        void ForceAngle(float width)
        {
            forceAngleInDegrees.x = EditorGUILayout.Slider("Force Angle X: ", forceAngleInDegrees.x, -360, 360, GUILayout.Width(width - 20));

            forceAngleInDegrees.y = EditorGUILayout.Slider("Force Angle Y: ", forceAngleInDegrees.y, -360, 360, GUILayout.Width(width - 20));
        }

        public void ResetAllBodies()
        {
            EndSim();

            if (simulated[previousIndex] != null)
            {
                foreach (SimulatedBody body in simulated[previousIndex])
                {
                    body.Reset();
                }

                simulated[previousIndex] = null;

                if (--previousIndex < 0)
                {
                    previousIndex += 3;
                }

                --stackedSimulations;
            }
        }

        Rigidbody AutoGenerateComponents(Transform transform)
        {
            Rigidbody rb = transform.GetComponent<Rigidbody>();

            if (!rb)
            {
                rb = transform.GetComponentInChildren<Rigidbody>();

                if (!rb)
                {
                    rb = transform.gameObject.AddComponent<Rigidbody>();
                    generatedRigidbodies.Add(rb);
                }
            }

            if (!transform.GetComponent<Collider>() && !transform.GetComponentInChildren<Collider>())
            {
                generatedColliders.Add(transform.gameObject.AddComponent<BoxCollider>());
            }

            return rb;
        }

        public void PrepareSim()
        {
            if (currentState > state.force && pointObject == null)
            {
                return;
            }

            generatedRigidbodies = new List<Rigidbody>();

            generatedColliders = new List<Collider>();

            simulatedBodies = new List<SimulatedBody>();

            resetBodies = new List<SimulatedBody>();

            List<Transform> transforms = new List<Transform>(Selection.GetTransforms(SelectionMode.OnlyUserModifiable));

            if (currentState == state.cannon)
            {
                AutoGenerateComponents(pointObject.transform);

                transforms.Add(pointObject.transform);
            }

            foreach (Transform transform in transforms)
            {
                simulatedBodies.Add(new SimulatedBody(AutoGenerateComponents(transform)));
            }

            foreach (Rigidbody rb in FindObjectsOfType<Rigidbody>())
            {
                if (!transforms.Contains(rb.transform))
                {
                    resetBodies.Add(new SimulatedBody(rb));

                    rb.isKinematic = true;
                }
                else
                {
                    rb.isKinematic = false;
                }
            }

            Vector3 point = (currentState == state.explosion) ? pointObject.transform.position : Vector3.zero;

            float force;

            Vector3 forceDir;

            forceDir = Quaternion.Euler(forceAngleInDegrees) * Vector3.forward;

            force = Random.Range(forceMinMax.x, forceMinMax.y);

            if (currentState == state.cannon)
            {
                simulatedBodies[simulatedBodies.Count - 1].rigidbody.AddForce(forceDir * forceMinMax.x, ForceMode.Impulse);
            }
            else
            {
                foreach (SimulatedBody body in simulatedBodies)
                {
                    if (currentState == state.explosion)
                    {
                        forceDir = (body.rigidbody.transform.position - point).normalized;

                        force = forceMinMax.x * Mathf.Max(0, 1 - Mathf.Max(0, Vector3.Distance(body.rigidbody.transform.position, point) - noFallOffArea) / Mathf.Max(1, explosionArea - noFallOffArea));
                    }

                    body.rigidbody.AddForce(forceDir * force, ForceMode.Impulse);
                }
            }

            sim = Simulate();

            if (!realTime)
            {
                while (sim != null)
                {
                    sim.MoveNext();
                }
            }
        }

        private void Update()
        {
            if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
            {
                PlayModeChange();
            }
            if (repaint && cooldown < Time.realtimeSinceStartup)
            {
                Repaint();

                repaint = false;
            }

            if (sim != null)
            {
                sim.MoveNext();
            }
        }

        IEnumerator Simulate()
        {
            Physics.autoSimulation = false;

            for (int i = 0; i < maxIterations; i++)
            {
                Physics.Simulate(Time.fixedDeltaTime);

                if (simulatedBodies.TrueForAll(body => body.rigidbody.IsSleeping()))
                {
                    break;
                }

                yield return null;
            }

            EndSim();
        }

        void EndSim()
        {
            Physics.autoSimulation = true;

            if (sim != null)
            {
                foreach (SimulatedBody body in resetBodies)
                {
                    body.Reset();
                }

                foreach(SimulatedBody body in simulatedBodies)
                {
                    body.rigidbody.isKinematic = body.isKinematic;
                }

                RemoveAutoGeneratedComponents();

                sim = null;

                if (currentState == state.cannon && resetCannon)
                {
                    simulatedBodies[simulatedBodies.Count - 1].Reset();

                    simulatedBodies.RemoveAt(simulatedBodies.Count - 1);
                }

                if (simulatedBodies != null)
                {
                    if (++previousIndex > 2)
                    {
                        previousIndex -= 3;
                    }

                    if (stackedSimulations < 3)
                    {
                        ++stackedSimulations;
                    }

                    simulated[previousIndex] = simulatedBodies;

                    simulatedBodies = null;
                }

                cooldown = Time.realtimeSinceStartup + 1f;

                Repaint();

                repaint = true;
            }
        }

        void RemoveAutoGeneratedComponents()
        {
            foreach (Rigidbody rb in generatedRigidbodies)
            {
                DestroyImmediate(rb);
            }
            foreach (Collider c in generatedColliders)
            {
                DestroyImmediate(c);
            }
        }

        private void OnSelectionChange()
        {
            if (!visualiseDirections)
                return;

            visualisationLines = new List<VisualObjects>();

            resultingForce = Quaternion.Euler(forceAngleInDegrees) * Vector3.forward * forceMinMax.x;

            if (currentState != state.cannon)
            {
                foreach (Transform transform in Selection.GetTransforms(SelectionMode.OnlyUserModifiable))
                {
                    visualisationLines.Add(new VisualObjects(transform, CalculatePoints(transform)));
                }
            }
            else
            {
                if (pointObject == null)
                {
                    return;
                }

                visualisationLines.Add(new VisualObjects(pointObject.transform, CalculatePoints(pointObject.transform)));
            }
        }

        void RecalculatePoints()
        {
            if (!visualiseDirections)
                return;

            resultingForce = Quaternion.Euler(forceAngleInDegrees) * Vector3.forward * forceMinMax.x;

            visualisationLines = new List<VisualObjects>();

            if (currentState != state.cannon)
            {
                foreach (Transform transform in Selection.GetTransforms(SelectionMode.OnlyUserModifiable))
                {
                    visualisationLines.Add(new VisualObjects(transform, CalculatePoints(transform)));
                }
            }
            else
            {
                if (pointObject == null)
                {
                    return;
                }

                visualisationLines.Add(new VisualObjects(pointObject.transform, CalculatePoints(pointObject.transform)));
            }

            SceneView.RepaintAll();
        }

        private Vector3[] CalculatePoints(Transform transform)
        {
            Vector3 velocity = Vector3.zero;

            Rigidbody rigid = transform.GetComponent<Rigidbody>();

            float mass = 1;

            if (rigid != null)
            {
                mass = rigid.mass;
            }

            switch (currentState)
            {
                case state.force:
                case state.cannon:
                    velocity = resultingForce / mass;

                    break;
                case state.explosion:
                    if (pointObject != null)
                    {
                        velocity = (transform.position - pointObject.transform.position).normalized * Mathf.Max(0, forceMinMax.x * (1 - Mathf.Max(0, Vector3.Distance(transform.position, pointObject.transform.position) - noFallOffArea) / Mathf.Max(1, explosionArea - noFallOffArea))) / mass;
                    }
                    else
                    {
                        velocity = resultingForce / mass;
                    }
                    break;
            }

            Vector3[] lines = new Vector3[maxIterations + 1];

            lines[0] = transform.position;

            for (int points = 0; points < maxIterations; points++)
            {
                velocity += Physics.gravity * Time.fixedDeltaTime;

                lines[points + 1] = lines[points] + velocity * Time.fixedDeltaTime;
            }

            return lines;
        }

        void OnSceneGUI(SceneView sceneView)
        {
            if (visualiseDirections && visualisationLines != null && sim == null)
            {
                Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;

                if (currentState == state.explosion && pointObject != null)
                {
                    Handles.color = Color.magenta;

                    DrawWireSphere(pointObject.transform.position, noFallOffArea);

                    Handles.color = Color.blue;

                    DrawWireSphere(pointObject.transform.position, explosionArea);
                }

                bool recalculate = false;

                foreach (VisualObjects line in visualisationLines)
                {
                    if (line.transform.position != line.points[0])
                    {
                        recalculate = true;
                    }
                }

                if (recalculate)
                {
                    GetWindow<SimulatePhysicsWindow>("SimulatePhysics").RecalculatePoints();
                }

                Handles.color = Color.red;

                foreach (VisualObjects line in visualisationLines)
                {
                    Handles.DrawAAPolyLine(3f, line.points);
                }
            }
        }

        void DrawWireSphere(Vector3 center, float radius)
        {
            Handles.DrawWireDisc(center, Vector3.up, radius);

            Handles.DrawWireDisc(center, Vector3.forward, radius);

            Handles.DrawWireDisc(center, Vector3.right, radius);
        }

        private void OnEnable()
        {
            SceneView.onSceneGUIDelegate -= OnSceneGUI;

            SceneView.onSceneGUIDelegate += OnSceneGUI;
        }

        private void OnDestroy()
        {
            SceneView.onSceneGUIDelegate -= OnSceneGUI;

            visualisationLines = null;

            if (sim != null)
            {
                ResetAllBodies();
            }

            previousSimulated = simulated;
        }

        struct VisualObjects
        {
            public readonly Transform transform;

            public Vector3[] points;

            public VisualObjects(Transform transform, Vector3[] points)
            {
                this.transform = transform;

                this.points = points;
            }
        }

        void PlayModeChange()
        {
            if (sim != null)
            {
                ResetAllBodies();
            }

            for (int i = 0; i < 3; i++)
            {
                int index = (i + previousIndex) % 3;

                if (simulated[index] != null)
                {
                    switch (index)
                    {
                        case 0:
                            p_previousSimulated1 = simulated[index];
                            break;
                        case 1:
                            p_previousSimulated2 = simulated[index];
                            break;
                        case 2:
                            p_previousSimulated3 = simulated[index];
                            break;
                    }
                }
            }
        }

        List<SimulatedBody>[] FetchSimulated()
        {
            List<SimulatedBody>[] list = new List<SimulatedBody>[3];

            int index = 0;

            if (p_previousSimulated1 != null && p_previousSimulated1.Count > 0)
            {
                list[index++] = p_previousSimulated1;
            }

            if (p_previousSimulated2 != null && p_previousSimulated2.Count > 0)
            {
                list[index++] = p_previousSimulated2;
            }

            if (p_previousSimulated1 != null && p_previousSimulated3.Count > 0)
            {
                list[index++] = p_previousSimulated3;
            }

            stackedSimulations = index;

            p_previousSimulated1 = null;
            p_previousSimulated2 = null;
            p_previousSimulated3 = null;

            return list;
        }
    }
}