<h1>OoT NPC Maker</h1>
An NPC creation tool for The Legend of Zelda: Ocarina of Time.

<table>
  <tr>
    <td>  
      <img src="https://user-images.githubusercontent.com/43761362/148636741-7398b6b3-3fe6-4db6-83b6-13c1bf710d04.png">
    </td>
    <td> 
      <img src="https://user-images.githubusercontent.com/43761362/148636747-9fa7c9ac-9c9f-4e57-95e8-f121f7a4e488.png">
    </td> 
  </tr>
  <tr>
    <td>
      <img src="https://user-images.githubusercontent.com/43761362/148636751-7f6fd856-fab1-4b36-bb6f-791af81cfbcd.png">
    </td>
    <td>
      <img src="https://user-images.githubusercontent.com/43761362/148636806-9ec8bcbd-0ef1-49ea-afb9-3063a4b7744f.png">
    </td>
  </tr>
  <tr>
    <td>
      <img src="https://user-images.githubusercontent.com/43761362/148636887-9cd96338-72fc-4b1e-97e9-f41ba3b9762e.png">
    </td>
    <td>
      <img src="https://user-images.githubusercontent.com/43761362/148636964-d78800ba-6b33-4e0b-bce7-f937ebe19054.png">
    </td>
  </tr>
</table>

--------------------------------------------------------------
Instructions on how to add an actor to your game:

<h2>Custom Actor Toolkit</h2>
<b>Compiling the actor overlay</b><br><br>

- Download the tool.<br>
- Download <a href="https://hylianmodding.com/?p=217">CAT</a>.

Then, in CAT:<br>
- Select <i>Help->Download z64hdr</i>.<br>
- Select <i>File->Load ROM</i> and select the Master Quest Debug ROM.<br>
- Select <i>Import overlay</i> and select the <i>CustomActorToolkit.c</i> file from the <i>actor</i> folder.<br>
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
Using your scene editing tool, add the actor selected in CAT (for example, 3, as stated above), then fill out its data as follows:
- "Variable" should be set to the Object number the NPC Maker zobj was imported as (e.g 3A as proposed above)<br>
- "X Rotation" should be 0 (if set to 1, this turns on screen logging if the actor was compiled with LOG_TO_SCREEN defined as 1).<br>
- "Z Rotation" should be the NPC ID from NPC Maker - if using the Wallmaster example, this should stay 0. Further NPCs added to the file would be 1, 2, etc.<br>

You do not need to add the zobj from NPC Maker to the scene file list - it is loaded automatically.

The scene included in the Example folder has this set up for the Wallmaster NPC already.
After everything's set up, inject the scene into the game (make sure the "Auto Injection Offset" option is set in SharpOcarina when injecting)

You're done! <br>
![image](https://user-images.githubusercontent.com/43761362/148632569-57d34376-b8ee-4828-919f-843ad562ea42.png)

To edit the actor further, simply make the changes in NPC Maker, save the zobj and re-import it into the game.

<h2>ZZROMTOOL and ZZRTL</h2>
CAT is not the best tool to use this with - because you'll eventually run into file conflicts unless you're very diligent about your injection offsets. As such, it's recommended you use <a href="https://old.z64.me/tools/zzromtool.html">ZZROMTOOL</a> or <a href="https://old.z64.me/tools/zzrtl.html">ZZRTL</a> instead.
Instructions on setting up that environment are specified on the linked pages. 

Aftewards, setup the <a href="https://old.z64.me/guides/overlay-environment-setup-windows.html">overlay environment</a>. The tutorial uses z64ovl - which is an obsolete library. Substitute it with <a href="https://github.com/z64tools/z64hdr">z64hdr</a> instead. Compile the actor using the Makefile and put the NPC Maker zobj into a file folder.

Note: For ZZROMTOOL make sure <i>include/npc_maker.h</i> defines the "ZZROMTOOL" variable to 1.
  
<h2>OoT decompilation</h2>
Setting this up with the OoT decompilation can be done, but it wouldn't be very convenient and is not recommended right now.  
