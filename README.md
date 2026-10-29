# NPC Navigation Tuning — Unity Portfolio Project

Three self-contained Unity 6 projects demonstrating **NavMesh agent debugging and parameter tuning**. Each project presents a deliberately broken navigation scenario for you to diagnose and fix.

Built as a practice exercise and portfolio recording piece for The Odin Project.

## Projects

| Folder | Problem | Root Cause |
|--------|---------|------------|
| `01_NarrowCorridor/` | Agent stuck / clips walls | `agent.radius` too large for corridor width |
| `02_SharpCorner/` | Overshoots, oscillates at 90° turn | Low `angularSpeed`, high `acceleration`, zero `stoppingDistance` |
| `03_DynamicObstacles/` | Stutters / freezes through obstacles | Hyper-sensitive carve thresholds + low-quality avoidance |

## How to Use

1. Open any folder as a **separate Unity project** (Unity 6000.4.0f1)
2. **Tools → Generate Scene** — one-click setup
3. **Play** — observe the bug
4. Read the project's `README.md` for diagnosis + hints
5. Fix it, re-play, verify

Each project is ~5 minutes of recording = ~15–20 min total.

## Skills Demonstrated

- Unity NavMesh baking and agent configuration
- Diagnosing navigation bugs from visual symptoms
- Runtime parameter tuning with inspector feedback
- Editor tooling (automated scene generation)
- Debug visualization (gizmos for path waypoints and velocity vectors)

## Tools & Versions

- Unity 6000.4.0f1
- com.unity.ai.navigation 2.0.11
