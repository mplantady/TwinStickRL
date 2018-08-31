# TwinStickRL

## Installation
You need to install TensorflowSharp to be able to use pre-trained models
https://github.com/llSourcell/Unity_ML_Agents/blob/master/docs/Using-TensorFlow-Sharp-in-Unity-(Experimental).md

## Training
You must build the game with one brain in external and the other in internal mode. (TwinMove or TwinAim)

The 2 brains are trained separately using this command:

### Move Brain
python learn.py ./build/TwinMove.exe --curriculum=./curricula/Twin_move.json --run-id=Twin_move --train

### Aim Brain
python learn.py ./build/TwinAim.exe --curriculum=./curricula/Twin_aim.json --run-id=Twin_aim --train

### Curriculum variables
To help the training some variables change over time depending on reward thresholds
- spawn_rate: number of step between 2 enemy spawns
- min_enemy_distance: minimum distance allowed to spawn
- max_enemy_count: maximum of simultaneous enemy
- reset_center_prob: probability that the agent spawn at the center of the arena
- skip_internal_brain: disable the other brain during early stage of the training

## Informations
This repository contains UnityML 0.4 (https://github.com/Unity-Technologies/ml-agents)
