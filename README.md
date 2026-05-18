# Stealth AI Game

A Unity project focused on NPC behavior systems, including perception, decision-making, and movement.

## Demo

![Demo](Documentação/demo.gif)

## Overview

This project implements a stealth scenario where NPC agents detect and react to the player using a layered perception system combined with a finite state machine.

## How It Works

### Perception System

The NPC detection is implemented using a two-step approach:

* Broad phase:

  * Distance and field-of-view check (low computational cost)
* Narrow phase:

  * Raycast to validate line-of-sight (high accuracy)

This approach reduces unnecessary physics calculations while maintaining reliable detection.

### State Machine

NPC behavior is controlled by a finite state machine:

* Patrol → default movement between waypoints
* Investigate → triggered when suspicious activity is detected
* Alert → triggered when the player is confirmed

### Movement

* Unity NavMesh is used for pathfinding
* NPC dynamically updates destination based on player position

## Technical Details

* Modular architecture:

  * EnemyVision
  * EnemyDetection
  * EnemyPatrol
* Separation of concerns:

  * Perception, decision, and movement handled independently
* Event-driven transitions reduce constant state polling

## Key Files

* Assets/Scripts/AI/EnemyVision.cs
* Assets/Scripts/AI/EnemyDetection.cs
* Assets/Scripts/AI/EnemyPatrol.cs

## What I Learned

* Designing AI behavior systems
* Balancing accuracy vs performance in real-time systems
* Structuring modular and maintainable game logic

## Requirements

* Unity 2022.3.6f1
