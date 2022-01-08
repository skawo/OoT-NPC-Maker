<h1>OoT NPC Maker</h1>
An NPC creation tool for The Legend of Zelda: Ocarina of Time.

--------------------------------------------------------------

Instructions on how to add an actor to your game using <a href="https://hylianmodding.com/?p=217">Custom Actor Toolkit</a>:

<b>Compiling the actor overlay</b><br>
- Download this repository.
- Download CAT.

Then, in CAT:<br>
- Select <i>Help->Download z64hdr</i>.<br>
- Select <i>File->Load ROM</i> and select the Master Quest Debug ROM.<br>
- Select <i>Import overlay</i> and select the <i>CustomActorToolkit.c</i> file from the <i>actor</i> folder in this repository.<br>
- Select the actor ID to use, for example 3 (which is blank in the original game).<br>
- Check <i>Use z64hdr</i> and then click <i>Compile nOVL</i>. There should be a message stating that the compilation was successful in the terminal.<br>
- Click <i>Find Empty Space</i> and then <i>Inject to ROM</i><br>

Note: you can make the actor smaller by going into <i>Settings->Change C compile flags</i> and changing <i>-O1</i> into <i>-Os</i>

<b>Creating an actor</b><br>
For the sake of this tutorial, we're going to use the example actor found in the "Example" folder of the repository.
Load it up in NPC Maker by going <i>File -> Open</i> and select ActorData.json.
You should see a Wallmaster NPC with ID 0.
Now, select <i>File -> Save binary</i> and save the file somewhere.

Then, in CAT:
- Select the <i>Objects</i> tab.<br>
- Click <i>Import ZOBJ</i> and select the file you got from NPC Maker.<br>
- Select the object number, for example 3A (which is blank in the original game).<br>
- Click <i>Find empty space</i>
- Click <i>Inject to ROM</i><br>

<b>Adding actor to scene</b><br>
Add the actor selected in CAT (for example, 3, as stated above), then fill out its data as follows:
- "Variable" should be set to the Object number the NPC Maker zobj was imported as (e.g 3A as proposed above)<br>
- "X Rotation" should be 0 (if set to 1, this turns on screen logging if the actor was compiled with LOG_TO_SCREEN defined as 1).<br>
- "Z Rotation" should be the NPC ID from NPC Maker - if using the Wallmaster example, this should stay 0. Further NPCs added to the file would be 1, 2, etc.<br>

The scene included in the Example folder has this set up for the Wallmaster NPC already.
After everything's set up, inject the scene into the game (make sure the "Auto Injection Offset" option is set in SharpOcarina when injecting)

You're done! <br>
![image](https://user-images.githubusercontent.com/43761362/148632569-57d34376-b8ee-4828-919f-843ad562ea42.png)

