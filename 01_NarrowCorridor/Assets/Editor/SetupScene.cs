using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using Unity.AI.Navigation;
using UnityEditor.SceneManagement;

/// <summary>
/// Tools ▸ Generate Scene — builds the narrow corridor problem scenario.
/// </summary>
public static class SetupScene
{
    const float WallHeight = 3f;

    [MenuItem("Tools/Generate Scene")]
    static void Generate()
    {
        EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.localScale = new Vector3(4, 1, 2);
        ground.GetComponent<Renderer>().sharedMaterial = MakeMat("Ground_Mat", new Color(0.35f, 0.45f, 0.35f));
        ground.isStatic = true;
        var surface = ground.AddComponent<NavMeshSurface>();

        // ponytail: corridor half-width 0.55 vs agent radius 0.5 = guaranteed scrape
        float z = 0f;
        float halfW = 0.55f;
        float length = 30f;
        MakeWall("Wall_Left",  new Vector3(0, WallHeight / 2, z + halfW), new Vector3(length, WallHeight, 0.2f));
        MakeWall("Wall_Right", new Vector3(0, WallHeight / 2, z - halfW), new Vector3(length, WallHeight, 0.2f));

        var npc = MakeNPC(new Vector3(-12, 1, z));
        var target = MakeTarget(new Vector3(12, 0.5f, z));

        var agent = npc.GetComponent<NavMeshAgent>();
        agent.radius = 0.5f;           // too wide for 1.1m corridor
        agent.speed = 3.5f;
        agent.angularSpeed = 120f;
        agent.acceleration = 8f;
        agent.stoppingDistance = 0.5f;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;

        npc.GetComponent<NPCNavigationTester>().target = target.transform;
        surface.BuildNavMesh();

        var cam = Camera.main;
        if (cam != null)
        {
            cam.transform.position = new Vector3(0, 20, -8);
            cam.transform.rotation = Quaternion.Euler(70, 0, 0);
        }

        var scene = EditorSceneManager.GetActiveScene();
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/NarrowCorridor.unity");
        AssetDatabase.Refresh();
        Debug.Log("✅ Narrow Corridor scene generated. Hit Play to see the bug.");
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

    static GameObject MakeWall(string name, Vector3 pos, Vector3 scale)
    {
        var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = name;
        wall.transform.position = pos;
        wall.transform.localScale = scale;
        wall.isStatic = true;
        wall.GetComponent<Renderer>().sharedMaterial = MakeMat("Wall_Mat", new Color(0.55f, 0.55f, 0.6f));
        return wall;
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
