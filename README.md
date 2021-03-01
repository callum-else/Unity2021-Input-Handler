# Unity2021-Input-Handler
A custom input handler created in C# for the Unity Game Engine.

## Description
For this project, I wanted to create an easily reusable and useful tool that I could implement into future projects. With these prerequisites I decided to build an easily deployable Input Layer, avoiding messy class structures and hard-to-follow cross-module dependencies.

The core of this project comes from 3 main scripts: InputHandler, InputHandlerEditor and InputComponents.

The InputHandler script, and its Editor counterpart, act as an interface between Unity and the core InputComponents, housing all Unity inspector and MonoBehaviour functionality.

The InputComponents script stores the main bulk of the input processing, housing the InputAxis and KeyFormatter classes.

The KeyFormatter class handles string to KeyCode casting, utilising Regex formatting to alter strings to match the KeyCode naming style; removing the need for in-editor case-perfect naming. The use of KeyCodes enables portability with Unity's built-in Input class, handling keyboard-level I/O operations.

The InputAxis class contains input information in an axial format, using positive and negative key-bindings. UnityEvents are used within this class to link set-up inputs to actions within the inspector, enabling code layering and modularity. These events recieve the current state of their assigned input group as a float, ranging between -1 and +1. This value can be either snapped or lerped between key presses, depending on user-defined settings. Future plans aim to make this further customisable, allowing users to set the number of inputs and the values they will output.

## Installing
To install this package:
- Download the <code>InputHandler</code> folder from the main repository and save it somewhere in your <code>Assets</code> folder.
- Add the <code>InputHandler.cs</code> script to a <code>GameObject</code> within the scene.
- Set up the desired controls and their connected functionality.
- Hit play!
