# 🌾 Combine Harvester Simulator

A 3D simulation project focusing on realistic combine harvester operation, agricultural mechanics, crop cutting, and harvesting workflows.

---

## 📌 Features

- **Realistic Vehicle Dynamics**: Authentic steering, throttle, braking, and terrain physics.
- **Harvesting Mechanics**: Functional cutter bar / header, reel animation, threshing, and crop collection.
- **Crop & Field System**: Interactive crop grid with cutting, yield accumulation, and chaff/straw dispersion.
- **Grain Storage & Unloading**: Internal grain tank capacity management and pipe unloading mechanics.
- **Multi-Camera Support**: Third-person follow camera, cockpit/interior driver view, and top-down working perspective.
- **Interactive UI & HUD**: Real-time speedometers, fuel levels, tank capacity gauges, and operational status indicators.

---

## 🛠️ Tech Stack & Architecture

- **Engine / Framework**: Unity (or Unreal / Custom C#)
- **Physics**: Rigidbody physics / Custom Wheel Colliders
- **Architecture**: Modular, decoupled systems adhering to SOLID principles and DRY patterns.

### System Overview
- `HarvesterController`: Handles vehicle movement, steering, acceleration, and physics.
- `HeaderController`: Controls cutter bar activation, ground alignment, and cutting animations.
- `CropManager`: Detects crop collisions, updates crop mesh/state, and computes harvested yield.
- `GrainStorageSystem`: Tracks storage capacity, fill levels, and unloading pipe triggers.
- `UIManager`: Updates dashboard telemetry, speedometer, and harvesting indicators.

---

## 🎮 Controls

| Action | Primary Input | Secondary / Controller |
| :--- | :--- | :--- |
| **Accelerate / Reverse** | `W` / `S` (or `Up` / `Down`) | Left Stick (Vertical) |
| **Steer** | `A` / `D` (or `Left` / `Right`) | Left Stick (Horizontal) |
| **Toggle Header (Cut)** | `Space` / `B` | `X` Button |
| **Raise / Lower Header** | `Q` / `E` | D-Pad Up / Down |
| **Toggle Unloading Pipe** | `O` | `Y` Button |
| **Switch Camera View** | `C` | Right Stick Click |

---

## 🚀 Getting Started

### Prerequisites
- [Unity Engine](https://unity.com/) (Version: `2022.3 LTS` or higher recommended)
- Git LFS (if large 3D models/textures are tracked)

### Installation
1. Clone the repository:
   ```bash
   git clone [https://github.com/bossSRS/combined-harvestor-simulator.git](https://github.com/bossSRS/combined-harvestor-simulator.git)
