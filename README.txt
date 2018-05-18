# sit305-project：RUNE FOREST
RPG game
this is me
this is my team:
Dingan Ma (216328682)git hub username: martinanswer link:https://github.com/pomrp/sit305-project
Yuan Ren(215194192)git hub username: pomrp link:https://github.com/pomrp/sit305-project


Henry comments 13/April
(Henry also needs you to write a changelog.md and licenses.txt files)

Henry comments 17/April
# Henry Comments 17/April
- I'm still waiting on a channgelog.txt and licenses.txt file
- I still don't see a data/ directory (you actually need to explain all your directories in this readme file, as mentioned on a News post recently).
- I need to see many more commits + changelog items for you to Pass. I'll give you until Sunday to catch up for Week 3 + Week 4 + Week 5.

# Henry comments 27/April
- Directory explanations should be based on the file system (not within the IDE), as I'll be checking from file system.
- Your changelog needs a lot of work (especially who did what task each day)
- Your data hasn't progressed enough. You need to get to this very quickly, as you can't start building your game until you can at least load the levels/world of your game. So work on getting loading working (from data files) asap.




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

Feature:
1. Combine card game, minne sweeper, brick game in the rpg game.
2. Use unity fungus to create rpg game.
3. Splash screen.
4. Collect the runes every where to win the game.
5. Amazing graphic design.

Data structure

1.character:
Female, zoombie, monster, witch, witch's dauther, card game, mine sweeper game, bricks game.
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

code for mine sweeper:

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elemet : MonoBehaviour {

    public bool mine;

    public Sprite[] emptyTextures;
    public Sprite mineTextures;

	// Use this for initialization
	void Start () {
        mine = Random.value < 0.15;

        int x = (int)transform.position.x;
        int y = (int)transform.position.y;
        Grid.elements[x, y] = this;
		
	}

    public void loadTexture(int adjacentCount) {
        if (mine)
            GetComponent<SpriteRenderer>().sprite = mineTextures;
        else
            GetComponent<SpriteRenderer>().sprite = emptyTextures[adjacentCount];


    }

    public bool isCovered() {
        return GetComponent<SpriteRenderer>().sprite.texture.name == "sweep";
    }

    private void OnMouseUpAsButton()
    {
        if (mine)
        {
            Grid.uncoverMines();
            print("you loose");
        }
        else {
            int x = (int)transform.position.x;
            int y = (int)transform.position.y;
            loadTexture(Grid.adjacentMines(x, y));
            Grid.FFuncover(x, y, new bool[Grid.w, Grid.h]);
            if (Grid.isFinished())
                print("you win");

        }
    }


    // Update is called once per frame

}

using UnityEngine;
using System.Collections;

public class Grid
{
? ? // The Grid itself
? ? public static int w = 5; // this is the width
? ? public static int h = 4; // this is the height
? ? public static Elemet[,] elements = new Elemet[w, h];

    public static void uncoverMines()
    {
        foreach (Elemet elem in elements)
            if (elem.mine)
                elem.loadTexture(0);
    }

    public static bool minAt(int x, int y) {
        if (x >= 0 && y >= 0 && x < w && y < h)
            return elements[x, y].mine;
        return false;
    }

    public static int adjacentMines(int x, int y) {
        int count = 0;
        if (minAt(x, y + 1)) ++count;
        if (minAt(x+1, y + 1)) ++count;
        if (minAt(x+1, y)) ++count;
        if (minAt(x+1, y - 1)) ++count;
        if (minAt(x, y - 1)) ++count;
        if (minAt(x-1, y - 1)) ++count;
        if (minAt(x-1, y)) ++count;
        if (minAt(x-1, y + 1)) ++count;
        return count;
    }

    public static void FFuncover(int x, int y, bool[,] visited)
    {
        if (x >= 0 && y >= 0 && x < w && y < h)
        {
            if (visited[x, y])
                return;

            elements[x, y].loadTexture(adjacentMines(x, y));

            if (adjacentMines(x, y) > 0)
                return;

            visited[x, y] = true;

            FFuncover(x - 1, y, visited);
            FFuncover(x + 1, y, visited);
            FFuncover(x, y - 1, visited);
            FFuncover(x, y + 1, visited);

        }
    }

    public static bool isFinished() {
        foreach (Elemet elem in elements)
            if (elem.isCovered() && !elem.mine)
                return false;
        return true;

    }
}

code for bricks:

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [Header("Horizontal speed")]
    public float speedX;
    Rigidbody2D playerRigidbody2D;

	void Start () {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
        moveLeftOrRight();

    }

    float LeftOrRight()
    {
        return Input.GetAxis("Horizontal");
    }

    void moveLeftOrRight()
    {
        playerRigidbody2D.velocity = LeftOrRight() * new Vector2(speedX, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour {

    public Text scoreText;

    int score;

    Rigidbody2D ballRigidbody2D;
    CircleCollider2D ballCircleCollider2D;


    [Header("Horizontal speed")]
    public float speedX;

    [Header("Vertical speed")]
    public float speedY;

    enum tags
    {
        Bricks,
        background
    }

    void Start()
    {
        ballRigidbody2D = GetComponent<Rigidbody2D>();
        ballCircleCollider2D = GetComponent<CircleCollider2D>();
        //ballRigidbody2D.velocity = new Vector2(speedX, speedY);
        scoreText.text = "Current score :";
        Invoke("ballStart", 3);

    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) )
        {
            ballStart();
        }
    }

    void ballStart()
        {
        if (isStop())
        {
            ballCircleCollider2D.enabled = true;
            transform.SetParent(null);
            ballRigidbody2D.velocity = new Vector2(speedX, speedY);
        }
        }
        bool isStop()
        {
        return ballRigidbody2D.velocity == Vector2.zero;
        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            lockSpeed();
            if (other.gameObject.CompareTag(tags.Bricks.ToString()))
            {
                other.gameObject.SetActive(false);
                score += 10;
                scoreText.text = "Current score :" + score;
            }
        }

        void lockSpeed()
        {
            Vector2 lockSpeed = new Vector2(resetSpeedX(), resetSpeedY());
            ballRigidbody2D.velocity = lockSpeed;
        }
        float resetSpeedX()
        {
            float currentSpeedX = ballRigidbody2D.velocity.x;
            if (currentSpeedX < 0)
            {
                return -speedX;

            }
            else
            {
                return speedX;
            }
        }
        float resetSpeedY()
        {
            float currentSpeedY = ballRigidbody2D.velocity.y;
            if (currentSpeedY < 0)
            {
                return -speedY;

            }
            else
            {
                return speedY;
            }
        }
    }

code for card game:

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public CardState cardState;
    public CardPattern cardPattern;
    public GameManager gameManager;

    void Start()
    {
        cardState = CardState.未翻牌;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnMouseUp()
    {
        if (cardState.Equals(CardState.已翻牌))
        {
            return;
        }

        if (gameManager.ReadyToCompareCards)
        {
            return;
        }

        OpenCard();
        gameManager.AddCardInCardComparison(this);
        gameManager.CompareCardsInList();
    }
    void OpenCard()
    {
        transform.eulerAngles = new Vector3(0, 180, 0);
        cardState = CardState.已翻牌;
    }
}

public enum CardState
{
    未翻牌, 已翻牌, 配對成功
}

public enum CardPattern
{
    無, 奇異果, 柳橙, 橘子, 水蜜桃, 芭樂, 葡萄, 蘋果, 西瓜
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("比對卡牌的清單")]
    public List<Card> cardComparison;

    [Header("卡牌種類清單")]
    public List<CardPattern> cardsToBePutIn;

    public Transform[] positions;

    [Header("已配對的卡牌數量")]
    public int matchedCardsCount = 0;

    void Start()
    {
        //SetupCardsToBePutIn();
        //AddNewCard(CardPattern.水蜜桃);
        GenerateRandomCards();
    }

    void SetupCardsToBePutIn()//Enum轉List
    {
        Array array = Enum.GetValues(typeof(CardPattern));
        foreach (var item in array)
        {
            cardsToBePutIn.Add((CardPattern)item);
        }
        cardsToBePutIn.RemoveAt(0);//刪掉Cardpattern.無
    }

    void GenerateRandomCards()//發牌
    {
        int positionIndex = 0;

        for (int i = 0; i < 2; i++)
        {
            SetupCardsToBePutIn();//準備卡牌
            int maxRandomNumber = cardsToBePutIn.Count;//最大亂數不超過8
            for (int j = 0; j < maxRandomNumber; maxRandomNumber--)
            {
                int randomNumber = UnityEngine.Random.Range(0, maxRandomNumber);//0到8之間產生亂數 最小是0 最大是7
                AddNewCard(cardsToBePutIn[randomNumber], positionIndex);//抽牌
                cardsToBePutIn.RemoveAt(randomNumber);
                positionIndex++;
            }
        }
    }

    void AddNewCard(CardPattern cardPattern, int positionIndex)
    {
        GameObject card = Instantiate(Resources.Load<GameObject>("Prefabs/牌"));
        card.GetComponent<Card>().cardPattern = cardPattern;
        card.name = "牌_" + cardPattern.ToString();
        card.transform.position = positions[positionIndex].position;

        GameObject graphic = Instantiate(Resources.Load<GameObject>("Prefabs/Pic"));
        graphic.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Graphic/" + cardPattern.ToString());
        graphic.transform.SetParent(card.transform);//變成牌的子物件
        graphic.transform.localPosition = new Vector3(0, 0, 0.1f);//設定座標
        graphic.transform.eulerAngles = new Vector3(0, 180, 0);//順著Y軸轉180度 翻牌時不會左右顛倒
    }

    public void AddCardInCardComparison(Card card)
    {
        cardComparison.Add(card);
    }

    public bool ReadyToCompareCards
    {
        get
        {
            if (cardComparison.Count == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void CompareCardsInList()
    {
        if (ReadyToCompareCards)
        {
            //Debug.Log("可以比對卡牌了");
            if (cardComparison[0].cardPattern == cardComparison[1].cardPattern)
            {
                Debug.Log("兩張牌一樣");
                foreach (var card in cardComparison)
                {
                    card.cardState = CardState.配對成功;
                }

                ClearCardComparison();
                matchedCardsCount = matchedCardsCount + 2;
                if (matchedCardsCount >= positions.Length)
                {
                    StartCoroutine(ReloadScene());
                }
            }
            else
            {
                Debug.Log("兩張牌不一樣");
                StartCoroutine(MissMatchCards());
                //TurnBackCards();
                //ClearCardComparison();
            }
        }
    }

    void ClearCardComparison()
    {
        cardComparison.Clear();
    }

    void TurnBackCards()
    {
        foreach (var card in cardComparison)
        {
            card.gameObject.transform.eulerAngles = Vector3.zero;
            //card.gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
            card.cardState = CardState.未翻牌;
        }
    }

    IEnumerator MissMatchCards()
    {
        yield return new WaitForSeconds(1.5f);
        TurnBackCards();
        ClearCardComparison();
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}


