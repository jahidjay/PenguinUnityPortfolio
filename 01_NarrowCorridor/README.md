# Problem 01 — Narrow Corridor (Agent Radius)

## The Bug
The NPC agent gets **stuck, clips through walls, or jitters** when navigating a tight corridor.

## Why It Happens
- The corridor is only **1.1m wide** (0.55m per side from center).
- The NavMeshAgent `radius` is **0.5m** — leaving only **0.05m** of clearance on each side.
- The baked NavMesh nearly closes the walkable area in the corridor, causing the agent to scrape walls, get wedged, or fail to find a path entirely.

## Quick Start
1. Open this folder as a Unity project (Unity 6000.4.0f1)
2. Go to **Tools → Generate Scene**
3. Hit **Play** — watch the agent struggle

## What to Look At
- `NavMeshAgent.radius` on the NPC
- The NavMeshSurface bake settings (agent radius used during bake)
- The corridor wall spacing

## Your Task
Fix the agent so it navigates the corridor **smoothly without wall contact**.

Tuning targets:
- [ ] Agent moves through corridor without stopping
- [ ] No visible wall clipping or jitter
- [ ] Agent reaches the target marker

## Hints (try before reading)
<details>
<summary>Hint 1</summary>
Reduce <code>agent.radius</code> to something like 0.25 so there's real clearance.
</details>
<details>
<summary>Hint 2</summary>
After changing the radius, you may need to re-bake the NavMesh with a matching agent radius in the NavMeshSurface settings.
</details>
