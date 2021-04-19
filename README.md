# Unity2021-Input-Handler
A custom input handler created in C# for the Unity Game Engine.

## Description
For this project, I wanted to create an easily reusable and useful tool that I could implement into future projects. With these prerequisites I decided to build an easily deployable Input Layer, avoiding messy class structures and hard-to-follow cross-module dependencies.

The core of this project comes from 3 main scripts: InputHandler, InputHandlerEditor and InputComponents.

The <code>InputHandler</code> script, and its <code>Editor</code> counterpart, act as an interface between Unity and the core <code>InputComponents</code> objects, housing all Unity inspector and MonoBehaviour functionality.

The <code>InputComponents<code> script stores the main bulk of the input processing, housing the <code>InputAxis</code> and <code>KeyFormatter</code> classes.

The <code>KeyFormatter</code> class handles string to KeyCode casting, utilising Regex formatting to alter strings to match the KeyCode naming style; removing the need for in-editor case-perfect naming. The use of KeyCodes enables portability with Unity's built-in Input class, handling keyboard-level I/O operations.

The <code>InputAxis</code> class contains input information in an axial format, using positive and negative key-bindings. UnityEvents are used within this class to link set-up inputs to actions within the inspector, enabling code layering and modularity. These events recieve the current state of their assigned input group as a float, ranging between -1 and +1. This value can be either snapped or lerped between key presses, depending on user-defined settings. Future plans aim to make this further customisable, allowing users to set the number of inputs and the values they will output.

## How to use
<details><summary><b>Demo without input setup.</b></summary>
  <p>
    Here, the inputs in the demo scene have been removed, resulting in no response to key presses while in play mode.
  </p>
  <img src="https://user-images.githubusercontent.com/23187869/109838525-283f8480-7c3e-11eb-9aac-87c5eca67fd7.gif">
</details>

<details><summary><b>Creating an input group.</b></summary>
  <p>
    To set up controls, first create a new input group inside the <c>InputHandler</c> component.
  </p>
  <img src="https://user-images.githubusercontent.com/23187869/109840233-d5ff6300-7c3f-11eb-80a2-0f4ebb4cef61.gif">
</details>

<details><summary><b>Assigning keys to the input group.</b></summary>
  <p>  
    Add your desired key bindings to this new input group, assigning each a display name and value. 
    The value variable can be either <code>Positive</code> or <code>Negative</code>, representing an output of +1 and -1 respectively whenever the given key is pressed.
  </p>
  <img src="https://user-images.githubusercontent.com/23187869/109860438-11a52780-7c56-11eb-8649-92d1a1219d24.gif">
</details>

<details><summary><b>Setting up additional input group variables.</b></summary>
  <p>
    Next, set up the other input group options by defining values for the <code>Step Size</code> and <code>Can Hold Input</code> variables.
    The <code>Step Size</code> determines how quickly the inputs will lerp from one to another. To make inputs snap, set the step size to 1.
    The <code>Can Hold Input</code> boolean will register inputs when the key is held if set to true, and will only register the key press itself when set to false.
  </p>
  <img src="https://user-images.githubusercontent.com/23187869/109861186-ec64e900-7c56-11eb-8875-8f3255054605.gif">
</details>

<details><summary><b>Assigning an output function to the input group.</b></summary>
  <p>
    Finally, assign a function to the input group that will recieve its output (a float between -1 and +1). 
    Ensure selected functions are from the <code>Dynamic</code> options, otherwise they will not recieve any input data.
  </p>
  <img src="https://user-images.githubusercontent.com/23187869/109858537-dbff3f00-7c53-11eb-984a-84b15ee11b33.gif">
</details>
  
<details><summary><b>Alternate input setup options.</b></summary>
  <p>
    Here, the <code>Jump</code> control should only have one key as an input. 
    This group should have no smoothing, and only call the output function when the key is first pressed. 
    Setting the <code>Keys</code> count to 1 limits the group to only one key, while setting the <code>Step Size</code> to 1 removes any input smoothing. 
    Setting the <code>Can Hold Input</code> variable to false will only call the output function when the key is first pressed.
  </p>
  <img src="https://user-images.githubusercontent.com/23187869/109862528-72356400-7c58-11eb-9b1d-bb6a14847de2.gif">
</details>

<details><summary><b>Editing inputs during Play Mode.</b></summary>
  <p>
    During Play Mode, inputs can be altered with immediate effect. 
    After altering an input, press the Update Input Settings button at the bottom of the <code>Input Handler</code> component.
    To do this programmatically, assigning a string value to the <code>Key.key</code> class variable will assign a new KeyCode to the <code>Key</code> object. 
  </p>
  <img src="https://user-images.githubusercontent.com/23187869/109863732-e7edff80-7c59-11eb-97d2-8e992d3244e9.gif">
  
  <p>
    The demo project contains scripts for generating UI based on the inputs defined at run-time, as well as for modifying those input settings in-game. 
  </p>
  <img src="https://user-images.githubusercontent.com/23187869/110020308-51851100-7d21-11eb-979f-7517ad83e293.gif">

</details>
  
## Installing
To install this package:
- Download the <code>InputHandler</code> folder from the main repository and save it somewhere in your <code>Assets</code> folder.
- Add the <code>InputHandler.cs</code> script to a <code>GameObject</code> within the scene.
- Set up the desired controls and their connected functionality.
- Hit play!
