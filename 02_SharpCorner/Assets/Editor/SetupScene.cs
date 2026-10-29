using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using Unity.AI.Navigation;
using UnityEditor.SceneManagement;

/// <summary>
/// Tools ▸ Generate Scene — builds the sharp 90° corner problem scenario.
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
        ground.transform.localScale = new Vector3(4, 1, 4);
        ground.GetComponent<Renderer>().sharedMaterial = MakeMat("Ground_Mat", new Color(0.35f, 0.45f, 0.35f));
        ground.isStatic = true;
        var surface = ground.AddComponent<NavMeshSurface>();

        float cx = 0f, cz = 0f;
        float arm = 12f;
        float hw = 1.5f;

        // Horizontal arm (comes from left)
        MakeWall("Arm_H_Outer", new Vector3(cx - arm / 2, WallHeight / 2, cz + hw), new Vector3(arm, WallHeight, 0.2f));
        MakeWall("Arm_H_Inner", new Vector3(cx - arm / 2, WallHeight / 2, cz - hw), new Vector3(arm, WallHeight, 0.2f));

        // Vertical arm (goes up)
        MakeWall("Arm_V_Outer", new Vector3(cx + hw, WallHeight / 2, cz + arm / 2), new Vector3(0.2f, WallHeight, arm));
        MakeWall("Arm_V_Inner", new Vector3(cx - hw, WallHeight / 2, cz + arm / 2), new Vector3(0.2f, WallHeight, arm));

        // Corner plug
        MakeWall("Corner_Plug", new Vector3(cx + hw, WallHeight / 2, cz + hw), new Vector3(0.2f, WallHeight, 0.2f));

        var npc = MakeNPC(new Vector3(cx - 10, 1, cz));
        var target = MakeTarget(new Vector3(cx, 0.5f, cz + 10));

        var agent = npc.GetComponent<NavMeshAgent>();
        agent.radius = 0.3f;
        agent.speed = 6f;              // fast
        agent.angularSpeed = 30f;      // ponytail: way too low — causes overshoot/oscillation
        agent.acceleration = 20f;      // ponytail: high accel + low angular = jerk
        agent.stoppingDistance = 0f;    // no braking
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;

        npc.GetComponent<NPCNavigationTester>().target = target.transform;
        surface.BuildNavMesh();

        var cam = Camera.main;
        if (cam != null)
        {
            cam.transform.position = new Vector3(0, 25, -5);
            cam.transform.rotation = Quaternion.Euler(75, 0, 0);
        }

        var scene = EditorSceneManager.GetActiveScene();
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/SharpCorner.unity");
        AssetDatabase.Refresh();
        Debug.Log("✅ Sharp Corner scene generated. Hit Play to see the bug.");
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
