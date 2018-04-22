# sit305-project
RPG game
this is me
this is my team:
Dingan Ma (216328682)git hub username: martinanswer
Yuan Ren(215194192)git hub username: pomrp

Henry comments 13/April
(Henry also needs you to write a changelog.md and licenses.txt files)

Henry comments 17/April
# Henry Comments 17/April
- I'm still waiting on a channgelog.txt and licenses.txt file
- I still don't see a data/ directory (you actually need to explain all your directories in this readme file, as mentioned on a News post recently).
- I need to see many more commits + changelog items for you to Pass. I'll give you until Sunday to catch up for Week 3 + Week 4 + Week 5.

Compile instructions

The first step is that download the zip file from the github and unzip the file.
Then downloading an unity is necessary for operating this project.
After downloading an unity, just back to the unzip file and click the sit305project-sit305-Assets-test.unity.
Unity will operating the project directly, moreover, if the version of unity was not the same of the development version, some warning will popup out and just need to click the cancel button. By this way the projct will still operate.
There are three icons below the toolbar which mean play,pause and next when the projct was operated by unity.
Click the play icon to play in unity to play our rpg game.
Click "Start game" button to start the game, click the dialog box in bottom of the game all the time and you can find the background was changing all the time.
When the enemy come out, select the spells to beat the enemy.
Finally, when the game was over, users can select play again or quit the game.

Directory Structure 
After you open the project in unity you and find the Assets file in the project tag dialog box.
There are three main file in the Asset file which is Done, fungus and fungusExample.
In the Done file,you can find all the images we used in the game, when you click Done-Graphic and you can find all the sounds we used in Done-Audio.
In the Fungus file,you can find all the code when you click Fungus-Script.

Data structure
1.character:
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System;

namespace Fungus
{
    /// <summary>
    /// A Character that can be used in dialogue via the Say, Conversation and Portrait commands.
    /// </summary>
    [ExecuteInEditMode]
    public class Character : MonoBehaviour, ILocalizable
    {
        [Tooltip("Character name as displayed in Say Dialog.")]
        [SerializeField] protected string nameText; // We need a separate name as the object name is used for character variations (e.g. "Smurf Happy", "Smurf Sad")

        [Tooltip("Color to display the character name in Say Dialog.")]
        [SerializeField] protected Color nameColor = Color.white;

        [Tooltip("Sound effect to play when this character is speaking.")]
        [SerializeField] protected AudioClip soundEffect;

        [Tooltip("List of portrait images that can be displayed for this character.")]
        [SerializeField] protected List<Sprite> portraits;

        [Tooltip("Direction that portrait sprites face.")]
        [SerializeField] protected FacingDirection portraitsFace;

        [Tooltip("Sets the active Say dialog with a reference to a Say Dialog object in the scene. This Say Dialog will be used whenever the character speaks.")]
        [SerializeField] protected SayDialog setSayDialog;

        [FormerlySerializedAs("notes")]
        [TextArea(5,10)]
        [SerializeField] protected string description;

        protected PortraitState portaitState = new PortraitState();

        protected static List<Character> activeCharacters = new List<Character>();

        protected virtual void OnEnable()
        {
            if (!activeCharacters.Contains(this))
            {
                activeCharacters.Add(this);
            }
        }

        protected virtual void OnDisable()
        {
            activeCharacters.Remove(this);
        }

There is two characters in the game right now: female, male, both of them had two images.

2.Say:
namespace Fungus
{
    /// <summary>
    /// Writes text in a dialog box.
    /// </summary>
    [CommandInfo("Narrative", 
                 "Say", 
                 "Writes text in a dialog box.")]
    [AddComponentMenu("")]
    public class Say : Command, ILocalizable
    {
        // Removed this tooltip as users's reported it obscures the text box
        [TextArea(5,10)]
        [SerializeField] protected string storyText = "";

        [Tooltip("Notes about this story text for other authors, localization, etc.")]
        [SerializeField] protected string description = "";

        [Tooltip("Character that is speaking")]
        [SerializeField] protected Character character;

        [Tooltip("Portrait that represents speaking character")]
        [SerializeField] protected Sprite portrait;

        [Tooltip("Voiceover audio to play when writing the text")]
        [SerializeField] protected AudioClip voiceOverClip;

        [Tooltip("Always show this Say text when the command is executed multiple times")]
        [SerializeField] protected bool showAlways = true;

        [Tooltip("Number of times to show this Say text when the command is executed multiple times")]
        [SerializeField] protected int showCount = 1;

        [Tooltip("Type this text in the previous dialog box.")]
        [SerializeField] protected bool extendPrevious = false;

        [Tooltip("Fade out the dialog box when writing has finished and not waiting for input.")]
        [SerializeField] protected bool fadeWhenDone = true;

        [Tooltip("Wait for player to click before continuing.")]
        [SerializeField] protected bool waitForClick = true;

        [Tooltip("Stop playing voiceover when text finishes writing.")]
        [SerializeField] protected bool stopVoiceover = true;

        [Tooltip("Wait for the Voice Over to complete before continuing")]
        [SerializeField] protected bool waitForVO = false;

        //add wait for vo that overrides stopvo

        [Tooltip("Sets the active Say dialog with a reference to a Say Dialog object in the scene. All story text will now display using this Say Dialog.")]
        [SerializeField] protected SayDialog setSayDialog;
The thing female and male say:Find it in flowchat dialog box.

3.call:
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System;

namespace Fungus
{
    /// <summary>
    /// Supported modes for calling a block.
    /// </summary>
    public enum CallMode
    {
        /// <summary> Stop executing the current block after calling. </summary>
        Stop,
        /// <summary> Continue executing the current block after calling  </summary>
        Continue,
        /// <summary> Wait until the called block finishes executing, then continue executing current block. </summary>
        WaitUntilFinished
    }

    /// <summary>
    /// Execute another block in the same Flowchart as the command, or in a different Flowchart.
    /// </summary>
    [CommandInfo("Flow", 
                 "Call", 
                 "Execute another block in the same Flowchart as the command, or in a different Flowchart.")]
    [AddComponentMenu("")]
    public class Call : Command
    {
        [Tooltip("Flowchart which contains the block to execute. If none is specified then the current Flowchart is used.")]
        [SerializeField] protected Flowchart targetFlowchart;

        [FormerlySerializedAs("targetSequence")]
        [Tooltip("Block to start executing")]
        [SerializeField] protected Block targetBlock;

        [Tooltip("Label to start execution at. Takes priority over startIndex.")]
        [SerializeField] protected StringData startLabel = new StringData();

        [Tooltip("Command index to start executing")]
        [FormerlySerializedAs("commandIndex")]
        [SerializeField] protected int startIndex;
    
        [Tooltip("Select if the calling block should stop or continue executing commands, or wait until the called block finishes.")]
        [SerializeField] protected CallMode callMode
The thing to call next episode can be find in flowchart.