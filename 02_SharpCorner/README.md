# Problem 02 — Sharp 90° Corner (Angular Speed & Acceleration)

## The Bug
The NPC agent **overshoots the corner, oscillates back and forth, and jerks** when trying to navigate a 90-degree turn.

## Why It Happens
- `angularSpeed` is set to **30°/s** — far too slow to turn 90° at speed 6.
- `acceleration` is **20** — the agent speeds up hard but can't steer, so it rams past the turn.
- `stoppingDistance` is **0** — no early deceleration, it slides into walls.

## Quick Start
1. Open this folder as a Unity project (Unity 6000.4.0f1)
2. Go to **Tools → Generate Scene**
3. Hit **Play** — watch the agent overshoot

## What to Look At
- `NavMeshAgent.angularSpeed` on the NPC
- `NavMeshAgent.acceleration` on the NPC
- `NavMeshAgent.stoppingDistance` on the NPC
- `NavMeshAgent.speed` on the NPC

## Your Task
Fix the agent so it navigates the 90° corner **cleanly without overshooting or oscillating**.

Tuning targets:
- [ ] Agent rounds the corner in one smooth arc
- [ ] No wall collision or back-and-forth jitter
- [ ] Agent reaches the target marker

## Hints (try before reading)
<details>
<summary>Hint 1</summary>
Raise <code>angularSpeed</code> to 120–360. The agent needs to be able to turn faster than it travels.
</details>
<details>
<summary>Hint 2</summary>
Lower <code>acceleration</code> to ~8 so the agent doesn't launch itself into walls before it can steer.
</details>
<details>
<summary>Hint 3</summary>
Set <code>stoppingDistance</code> to 0.5–1.0 so the agent starts braking before reaching tight geometry.
</details>
