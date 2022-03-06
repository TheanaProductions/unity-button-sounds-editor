Button sounds editor
--------------------
Editor extension for
[Unity](http://unity3d.com/)
engine.

Editor helps to assign click sound to all buttons in the current scene. It lists all buttons in one window, and it allows to edit sounds of those buttons.

Non-official Version 1.3 by TheanaProductions:</br>
Possibility to add/remove/refresh button elements at run-time </br>
Fixed obsolete script to work with Unity 2021.1.16f1 </br>
Possibility to chose only the buttons outside of prefabs in the scene, or all buttons

Version 1.2:</br>
Possibility to add click sound to Toggle UI components.</br>
Several filters for editor which will help to organize sounds when you have many buttons in the scene.

Version 1.1:</br>
Possibility to add click sound for any game object which has EventTrigger with PointerClick event.

Downloads
----------
Please check out 
[releases](//github.com/nubick/unity-button-sounds-editor/releases)
for the latest version of the official editor.

Documentation
-------------
Please check out following page for documentation:
[Button Sounds Editor for Unity.](http://nubick.ru/button-sounds-editor-for-unity/)

To use the non-official Version 1.3 run-time component you just need to put it on your AudioSource component and add the AudioClip through the inspector. There's no need to use the editor window if you use the ButtonClickSoundRunTime component. 
To call it from script, just use the namespace Assets.Plugins.ButtonSoundsEditor and call it with ButtonClickSoundRunTime.instance)

Preview
-------
How editor looks.

![Editor example](http://nubick.github.com/readme/button-sounds-editor.png)
