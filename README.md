# TwinStickRL

## Training
You must build the game with one brain in external and the other in internal mode. (TwinMove or TwinAim)

The 2 brains are trained separately using this command:

### Move Brain
python learn.py ./build/TwinMove.exe --curriculum=./curricula/Twin_move.json --run-id=Twin_move --train

### Aim Brain
python learn.py ./build/TwinAim.exe --curriculum=./curricula/Twin_aim.json --run-id=Twin_aim --train

## Informations
This repository contains UnityML 0.4 (https://github.com/Unity-Technologies/ml-agents)
