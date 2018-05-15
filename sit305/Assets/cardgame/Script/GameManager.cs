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
