# Grid-Based Container & Shelf Placement System

This project is a Unity prototype focused on building a grid-based item placement system in a 3D environment.

Instead of a traditional UI inventory, items are managed directly in the world using containers and shelves. The core of the system is a grid-based logic that controls how items are placed, validated, and organized in space.

---

## 🎯 Main focus

The goal of this project is to simulate an inventory-like system using:

- grid-based placement logic
- spatial (world-space) item positioning
- container and shelf management

Interaction exists only as a way to test and use the system.

---

## 🧠 Core systems

### 📦 Grid-Based Container Logic
Each container uses a 2D grid (`GatherableInventoryGrid`) that handles:

- placement validation (bounds + collision)
- multi-cell items (width × height)
- automatic placement
- removal and relocation

---

### 🗄️ Shelf Placement System
Shelves act as structured containers with multiple placement surfaces (`ContainerFloor`).

- items are placed using grid coordinates
- positions are converted to world-space
- supports multiple floors per shelf

---

### 🌍 World-Space Inventory Representation
Items are not stored in UI, but directly in the scene:

- each item is instantiated physically
- placement is based on grid logic
- container state is mirrored visually

---

### 🎯 Interaction Layer (minimal)
A simple raycast system is used only to trigger actions:

- detect interactable objects
- place items into containers
- take items back from shelves

---

## 🎮 Controls

- **WASD** – movement  
- **Mouse** – look  
- **E** – place item  
- **F** – take item  

---

## 🏗️ Architecture Overview

- `GatherableInventoryGrid` → core grid logic  
- `GatheringContainerController` → manages container state  
- `ContainerFloor` → converts grid coordinates to world positions  
- `Shelf` / `ShelfController` → shelf-specific logic  
- `PlayerInteractor` → simple input + raycast (not core system)  

---

## ⚙️ Requirements

- Unity (any recent version)
- Standard 3D project setup

---

## 🚀 Notes

This project focuses on system design and logic rather than UI or visuals.

The interaction layer is intentionally minimal — the main purpose is to demonstrate how items can be managed and placed in a structured way using grid-based logic in world space.

---

## 📌 Future improvements

- UI-based inventory (drag & drop)
- stacking system
- item rotation in runtime
- smarter placement (packing optimization)
- visual placement preview
