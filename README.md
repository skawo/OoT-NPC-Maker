OoT NPC Maker

Instructions on how to add an actor to your game using <a href="https://hylianmodding.com/?p=217">Custom Actor Toolkit</a>

<b>Compiling the actor overlay</b><br>
1. Download this repository. '
2. Download CAT.

Then, in CAT:
3. Select <i>Help->Download z64hdr</i>.
3. Select <i>File->Load ROM</i> the Master Quest Debug ROM.
4. Select <i>Import overlay</i> and select the <i>CustomActorToolkit.c</i> file from the <i>actor</i> folder in this repository.
5. Select the actor ID to use, for example 3 (which is blank in the original game).
5. Check <i>use z64hdr</i> and then <i>Compile nOVL</i>. There should be a message stating that the compilation was successful in the terminal.
6. Click <i>Find Empty Space</i> and then <i>Inject to ROM</i>

<b>Creating an actor</b><br>
For the sake of this tutorial, we're going to use the example actor found in the "Example" folder of the repository.
Load it up in NPC Maker by going <i>File -> Open</i>.
You should see a Wallmaster NPC with ID 0.
Now, select <i>File -> Save binary</i> and save the file somewhere.

Then, in CAT:
1. Select the <i>Objects</i> tab.
2. Click <i>Import ZOBJ</i> and select the file you got from NPC Maker.
3. Select the object number, for example 3A (which is blank in the original game).
4. Click <i>Find empty space</i> and <i>Find original row</i>
5. Click <i>Inject to ROM</i>

<b>Adding actor to scene</b><br>
Add the actor selected in CAT (for example, 3, as stated above), then fill out its data as follows:
- "Variable" should be set to the Object number the NPC Maker zobj was imported as (e.g 3A as proposed above)
- "X Rotation" should be 0 (if set to 1, this turns on screen logging if the actor was compiled with LOG_TO_SCREEN defined as 1).
- "Z Rotation" should be the NPC ID from NPC Maker - if using the Wallmaster example, this should stay 0. Further NPCs added to the file would be 1, 2, etc.

The scene included in the Example folder has this set up for the Wallmaster NPC already.
After everything's set up, inject the scene into the game.
