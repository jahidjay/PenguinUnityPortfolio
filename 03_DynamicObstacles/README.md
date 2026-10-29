# Problem 03 — Dynamic Obstacles (Carving & Avoidance)

## The Bug
The NPC agent **stutters, freezes, or takes wildly erratic paths** when navigating through a cluster of dynamic obstacles.

## Why It Happens
- 8 cubes have `NavMeshObstacle` with `carving = true`.
- `carvingMoveThreshold` is **0.01** — even micro-float drift triggers a full NavMesh re-carve.
- `carvingTimeToStationary` is **0.01** — obstacles are treated as stationary almost instantly, causing constant carve/uncarve cycles.
- `obstacleAvoidanceType` is set to **LowQuality** — the cheapest (worst) avoidance prediction.

## Quick Start
1. Open this folder as a Unity project (Unity 6000.4.0f1)
2. Go to **Tools → Generate Scene**
3. Hit **Play** — watch the agent stutter

## What to Look At
- `NavMeshObstacle.carvingMoveThreshold` on each obstacle cube
- `NavMeshObstacle.carvingTimeToStationary` on each obstacle cube
- `NavMeshAgent.obstacleAvoidanceType` on the NPC

## Your Task
Fix the obstacles and agent so navigation through the cluster is **smooth and stable**.

Tuning targets:
- [ ] Agent doesn't stutter or freeze mid-path
- [ ] Agent finds a consistent path through the obstacles
- [ ] No visible path recalculation jitter

## Hints (try before reading)
<details>
<summary>Hint 1</summary>
Raise <code>carvingMoveThreshold</code> to 0.1 or higher — obstacles don't actually move, so they shouldn't re-carve.
</details>
<details>
<summary>Hint 2</summary>
Raise <code>carvingTimeToStationary</code> to 0.5+ so Unity doesn't flip between moving/stationary states every frame.
</details>
<details>
<summary>Hint 3</summary>
Switch <code>obstacleAvoidanceType</code> to <code>HighQualityObstacleAvoidance</code> for better local steering around carved holes.
</details>
