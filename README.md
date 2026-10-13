# Light Weaver

**Light Weaver** is a 2D puzzle game developed in Unity where players solve optics-based challenges by manipulating mirrors to redirect light beams toward designated targets. By analyzing level layouts, aligning angles of incidence, and navigating around obstacles, players activate Light Cores across multiple levels of increasing complexity.

---

## Game Concept

In *Light Weaver*, levels are designed around light propagation and reflection. Players are tasked with guiding one or more colored light beams from their emitters to their respective targets. To succeed, beams must frequently pass through intermediate checkpoints and trigger interactive switches while avoiding path-blocking barriers, all within a limited move allowance.

---

## Gameplay Mechanics & Features

- **2D Laser Optics & Reflection**:
  - Real-time 2D raycasting engine (`LaserBeam2D`) calculating beam trajectories and reflections (`Vector2.Reflect`) upon striking mirrors.
  - Multi-beam and multi-color laser emitter support.
- **Mirror Manipulation**:
  - **Positioning**: Move mirrors across the puzzle grid using intuitive drag-and-drop handles.
  - **Rotation**: Rotate mirrors smoothly to dial in precise reflection angles.
- **Checkpoints, Switches & Obstacles**:
  - **Checkpoints**: Primary targets require beams to validate intermediate checkpoints before completing the circuit.
  - **Moveable Obstacles**: Laser-activated trigger buttons dynamically open or move barriers on the board.
  - **Static Obstacles**: Opaque blocks that occlude and terminate laser beams.
- **Move Allowance System**:
  - Each level grants a limited number of mirror moves (`rightToMove`), requiring players to plan routes efficiently before committing adjustments.
- **Progression & Level Design**:
  - 30 distinct puzzle levels progressively introducing new layouts, obstacles, and multi-laser challenges.
  - Persistent level completion tracking via `PlayerPrefs`.
- **Achievement System**:
  - Built-in achievement tracker (`AchievementManager`) recognizing key puzzle milestones:
    - *First Spark of Light*: Complete Level 1.
    - *Master of Reflection*: Use 3 or more mirrors in a single level.
    - *Mind Spark*: Solve a level on the first attempt.
    - *Flawless Victory*: Clear 5 consecutive levels on the first attempt.
    - *Color Commander*: Direct multiple light sources in a level.
    - *Quick Thinker*: Complete a puzzle in under 20 seconds.
- **Audio & Visual Presentation**:
  - Dynamic audio manager for background music and SFX (reflections, activations, victory, defeat).
  - Particle effects and animations for Light Core activation and level transitions.
  - Full UI suite including level select pagination, pause menu, and volume controls.

---

## Controls

The game is built with Unity's EventSystem, supporting both PC (mouse) and mobile (touch) input:

| Action | PC (Mouse) | Mobile (Touch) |
| :--- | :--- | :--- |
| **Move Mirror** | Click & drag mirror move handle | Tap & drag mirror move handle |
| **Rotate Mirror** | Click & drag rotation handle | Tap & drag rotation handle |
| **Menu / UI Navigation** | Left Click | Tap |
| **Pause / Settings** | Click Pause button | Tap Pause button |

---

## Technologies Used

- **Game Engine**: Unity `2021.3.30f1` (LTS)
- **Scripting Language**: C# (.NET / Unity Scripting API)
- **Physics**: Unity 2D Physics (`Physics2D.Raycast`, `BoxCollider2D`)
- **Rendering**: Unity `LineRenderer`, 2D Sprites
- **UI Framework**: Unity UI (uGUI), TextMesh Pro
- **Tweening / Animations**: LeanTween
- **Version Control**: Git & Git LFS

---

## Installation & Setup

### Prerequisites
- [Unity Hub](https://unity.com/download)
- Unity Editor version **`2021.3.30f1`**
- [Git](https://git-scm.com/) with [Git LFS](https://git-lfs.github.com/) installed

### Setup Instructions
1. Clone the repository:
   ```bash
   git clone https://github.com/NK28-V/Game-Design-Sem5.git
   ```
2. Navigate to the project directory and ensure all LFS assets are pulled:
   ```bash
   cd Game-Design-Sem5
   git lfs pull
   ```
3. Open **Unity Hub**.
4. Click **Add** -> **Add project from disk**.
5. Select the `Light-Weaver` project root directory.
6. Verify that Unity version **`2021.3.30f1`** is selected.
7. Open the project.
8. In the Unity Project window, navigate to `Assets/Scenes/MainMenu.unity` and open it.
9. Click the **Play** button in the Unity Editor to test and run the game.

---

## Project Context

This project was created as an academic collaborative project for **Semester 5 Game Design**. The goal of the project was to design and implement an engaging 2D puzzle game focusing on spatial reasoning, player feedback, progressive level difficulty, and clean game feel.

---

## Collaborators

* **Nidhish Kasat** — [NK28-V](https://github.com/NK28-V)
* **Advik Shrivastava** — [AdvikShr](https://github.com/AdvikShr)

---

## Acknowledgements

- **LeanTween**: Tweening engine by Dented Pixel used for UI transitions and element animations.
- **Third-Party Assets**: Sound effects, music tracks, and graphical assets used under their respective licenses (documented in `Assets/License/`).
