using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using Unity.AI.Navigation;
using UnityEditor.SceneManagement;

/// <summary>
/// Tools ▸ Generate Scene — builds the dynamic obstacles problem scenario.
/// </summary>
public static class SetupScene
{
    [MenuItem("Tools/Generate Scene")]
    static void Generate()
    {
        EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.localScale = new Vector3(4, 1, 4);
        ground.GetComponent<Renderer>().sharedMaterial = MakeMat("Ground_Mat", new Color(0.35f, 0.45f, 0.35f));
        ground.isStatic = true;
        var surface = ground.AddComponent<NavMeshSurface>();

        // Obstacle ring
        for (int i = 0; i < 8; i++)
        {
            float ox = Mathf.Cos(i * Mathf.PI / 4) * 3f;
            float oz = Mathf.Sin(i * Mathf.PI / 4) * 3f;
            var obs = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obs.name = $"Obstacle_{i}";
            obs.transform.position = new Vector3(ox, 0.75f, oz);
            obs.transform.localScale = new Vector3(1.2f, 1.5f, 1.2f);
            obs.GetComponent<Renderer>().sharedMaterial = MakeMat("Obstacle_Mat", Color.red);

            var nmObs = obs.AddComponent<NavMeshObstacle>();
            nmObs.carving = true;
            // ponytail: hyper-sensitive carve settings = constant re-bake = stutter
            nmObs.carvingMoveThreshold = 0.01f;
            nmObs.carvingTimeToStationary = 0.01f;
            nmObs.shape = NavMeshObstacleShape.Box;
            nmObs.size = new Vector3(1.2f, 1.5f, 1.2f);
        }

        var npc = MakeNPC(new Vector3(-12, 1, 0));
        var target = MakeTarget(new Vector3(12, 0.5f, 0));

        var agent = npc.GetComponent<NavMeshAgent>();
        agent.radius = 0.5f;
        agent.speed = 3.5f;
        agent.angularSpeed = 120f;
        agent.acceleration = 8f;
        agent.stoppingDistance = 0.5f;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQuality; // ponytail: worst quality = erratic steering

        npc.GetComponent<NPCNavigationTester>().target = target.transform;
        surface.BuildNavMesh();

        var cam = Camera.main;
        if (cam != null)
        {
            cam.transform.position = new Vector3(0, 22, -10);
            cam.transform.rotation = Quaternion.Euler(65, 0, 0);
        }

        var scene = EditorSceneManager.GetActiveScene();
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/DynamicObstacles.unity");
        AssetDatabase.Refresh();
        Debug.Log("✅ Dynamic Obstacles scene generated. Hit Play to see the bug.");
    }

    static GameObject MakeNPC(Vector3 pos)
    {
        var npc = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        npc.name = "NPC_Agent";
        npc.transform.position = pos;
        npc.GetComponent<Renderer>().sharedMaterial = MakeMat("NPC_Mat", Color.cyan);
        npc.AddComponent<NavMeshAgent>();
        npc.AddComponent<NPCNavigationTester>();
        return npc;
    }

    static GameObject MakeTarget(Vector3 pos)
    {
        var t = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        t.name = "Target_Destination";
        t.transform.position = pos;
        t.transform.localScale = Vector3.one * 0.6f;
        t.GetComponent<Renderer>().sharedMaterial = MakeMat("Target_Mat", Color.green);
        Object.DestroyImmediate(t.GetComponent<Collider>());
        return t;
    }

    static Material MakeMat(string name, Color color)
    {
        var path = $"Assets/Materials/{name}.mat";
        var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (mat != null) return mat;

        if (!AssetDatabase.IsValidFolder("Assets/Materials"))
            AssetDatabase.CreateFolder("Assets", "Materials");

        mat = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
        mat.color = color;
        mat.name = name;
        AssetDatabase.CreateAsset(mat, path);
        return mat;
    }
}
