using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    GameController gc;

    int extraDraw, rewardCards, standardDraw;
    public bool action, gameOver;

    public CardIndex cardIndex;
    public Deck playerDeckBase, currentPlayerDeck, discardPile;
    Hand playerHand, tempCards;
    public Player player; 
    public List<Enemy> enemies = new List<Enemy>();
    Enemy enemy0, enemy1, enemy2, enemy3;  
    public Button button, endTurnButton;
    private CardExecutor cardExecutor;
    private TurnManager turnManager;
    GameObject cardPrefab, enemyPrefab, playerPrefab, buttonPrefab,
        newEnemy0, newEnemy1, newEnemy2, newEnemy3, newPlayer;

    public PlayerCharacter playerCharacter;
    public BattleState battleState;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Avoid duplicates
        }

     }
    void Update()
    {
        if (action)
        {
            cardExecutor.TryPlayCards();
            ResolvePostAction();
        }

    }



    public void ResolvePostAction()
    {
        enemies.RemoveAll(e => e.isDead);

        if (enemies.Count == 0)
        {
            TurnEnd();
            EndEncounter();
        }
        action = false;
        CheckForGameOver();
    }
    public void CheckForGameOver()
    {
        if (player.health <= 0 && !gameOver)
        {
            gameOver = true;
            GameOver();
        }
    }

    /* void Update()
     {
         if (action)
         {
             for (int i = 0; i < playerHand.cards.Count; i++)
             {
                 if (playerHand.cards[i].currentCard)
                 {
                     if (player.mana >= playerHand.cards[i].cost)
                     {
                         if (playerHand.cards[i].noTarget)
                         {
                             enemies.ForEach(enemy => enemy.isTargeted = true);
                         }
                         //remove player energy
                         //remove health from enemy
                         //add block
                         playerHand.cards[i].currentCard = false;
                         AddToDiscardPile(playerHand.cards[i]);
                         OnAction?.Invoke(playerHand.cards[i]);
                         i--;
                     }
                     else
                     {
                         enemies.ForEach(enemy => enemy.isTargeted = false);
                         playerHand.cards[i].currentCard = false;
                         Debug.Log("Not Enough Mana!");
                         TargetingCard cardPos = playerHand.cards[i].
                             inGameObject.GetComponent<TargetingCard>();


                         cardPos.transform.position = cardPos.origin;
                     }



                 }
                 enemies.RemoveAll(e => e.isDead);
                 if (enemies.Count == 0)
                 {
                     TurnEnd();
                     EndEncounter();
                 }
             }
             action = false;
         }
         if (player.health <= 0 && !gameOver)
         {
             gameOver = true;
             GameOver();
         }

     }*/
    public void GameOver()
    {
        //Animate death by rotating guy
        battleState = BattleState.GameOver;

        gc.gameOverText.alpha = 1.0f;

        //Destroy all objects
        foreach (Enemy x in enemies)
        {
            x.Dispose();
        }
        enemies.Clear();
        player.Dispose();


        
        turnManager.DisposeHand(playerHand);
        turnManager.DisposeHand(tempCards);

       // playerHand.clear();
       // tempCards.clear();

        playerDeckBase.clear();

        currentPlayerDeck.clear();
        discardPile.clear();


        GameObject restartButton = Instantiate(buttonPrefab, new Vector2(0, -3.5f), Quaternion.identity);
        restartButton.transform.localScale = new Vector3(2f, 0.75f, 1f);
        button = restartButton.GetComponent<Button>();
        button.updateText(1);
        //Spawn in Game Over 

        action = false;
        endTurnButton.RemoveFromScreen();

    }
        
    public void restartGame()
    {      
        DisposeButton(button);
        gc.gameOverText.alpha = 0;

        battleState = BattleState.PrePostBattle;
        StartGame();
        
        endTurnButton.ReturnToOriginalPos();
        gameOver = false;
    }
    public void Action()
    {
        action = true;
    }


    public void CreateGame(PlayerCharacter character, GameController gc)
        
    {

        playerCharacter = character;
        gc.cardPrefab = cardPrefab;
        gc.enemyPrefab = enemyPrefab;
        gc.playerPrefab = playerPrefab;
        gc.buttonPrefab = buttonPrefab;
        battleState = BattleState.PrePostBattle;

        cardIndex = gameObject.AddComponent<CardIndex>();
        cardIndex.LoadCards(playerCharacter);

        endTurnButton = FindAnyObjectByType<Button>();
        


        player = gameObject.AddComponent<Player>();
        playerDeckBase = gameObject.AddComponent<Deck>();
        playerHand = gameObject.AddComponent<Hand>();
        tempCards = gameObject.AddComponent<Hand>();
        currentPlayerDeck = gameObject.AddComponent<Deck>();
        discardPile = gameObject.AddComponent<Deck>();
        cardExecutor = new CardExecutor(player, playerHand, discardPile, enemies);
        turnManager = new TurnManager(currentPlayerDeck, discardPile, playerHand, enemies, player, this);

        action = false;
        extraDraw = 0;
        rewardCards = 3;
        standardDraw = 5;
        StartGame();
    }

    void StartGame()
    {
        //If new game
        player.SetStats(playerCharacter);
        playerDeckBase.CreateDeck();
        //else save file - potentially
        StartFirstBattle();
    }
    void CopyAndShuffleDeck(Deck deck, Deck deckToCopy)
    {
        deck.CopyDeck(deckToCopy);
        deck.Shuffle();
    }
    public void StartFirstBattle()
    {
        SpawnPlayer();
        CopyAndShuffleDeck(currentPlayerDeck, playerDeckBase);
        NextEncounter();
        SpawnEnemyAttacks();
    }

    void SpawnEnemyAttacks()
    {
        foreach(Enemy x in enemies)
        {
            x.enemyAttackSelect();
        }
    }
    public void EndEncounter()
    {
        battleState = BattleState.PrePostBattle;
        turnManager.DisposeHand(playerHand);
        endTurnButton.RemoveFromScreen();
        SelectNewCard();
        
    }
    public void NextEncounter()
    {
        endTurnButton.ReturnToOriginalPos();
        ReShuffle();//with new card
        Encounter encounter = gameObject.AddComponent<Encounter>();
        //For Debug
            encounter.GetEncounter(UnityEngine.Random.Range(0, 3));
        
        SpawnEnemies(encounter);
        SpawnEnemyAttacks();
        turnManager.DrawCards();
        battleState = BattleState.Battle;
    }
    /*public void TurnEnd()
    {
        DisposeHand(playerHand);
        if (enemies.Count != 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].EnemyTurn();
            }
            DrawCards();
        }
        SpawnEnemyAttacks();
        player.playerTurnReset();
    }*/
    public void TurnEnd()
    {
        turnManager.EndTurn();
    }

    void ReShuffle()
    {
        if(battleState == BattleState.Battle)
        {
            CopyAndShuffleDeck(currentPlayerDeck, discardPile);
            //This is killing it
        }
        else if (battleState == BattleState.PrePostBattle)
        {
            //when do I use this?
            CopyAndShuffleDeck(currentPlayerDeck, playerDeckBase); 
        }
        discardPile.cards.Clear();
    }
    void DrawCards()
    {
        for (int currentDraw = 0; currentDraw < standardDraw + extraDraw; currentDraw++)
        {
            //Make sure this has the proper value for int currentDraw
            if (currentPlayerDeck.cards.Count != 0)
            {
                playerHand.cards.Add(currentPlayerDeck.cards[0]);
                DisplayCards(playerHand.cards[currentDraw], currentDraw, true);
                //Need to remove cards from the deck and add to Discard pile            
                currentPlayerDeck.cards.RemoveAt(0);
            }
            if (currentPlayerDeck.cards.Count == 0)//Reshuffle
            {
                ReShuffle();
            }
        }
    }



    void SpawnPlayer()
    {

        //Can add Things for new Characters
        newPlayer = Instantiate(playerPrefab,
                new Vector2(-7f, 2), Quaternion.identity);

        player.inGameActor = newPlayer.GetComponent<InGameActor>();
        player.UpdateTexts();
    }

    void SpawnEnemies(Encounter encounter)
    {
        if (encounter.enemy0 != null)
        {
            newEnemy0 = Instantiate(enemyPrefab,
            new Vector2(2f, 3), Quaternion.identity);
            enemy0 = new Enemy(encounter.enemy0, newEnemy0.GetComponent<InGameActor>());

            enemies.Add(enemy0);
        }
        if (encounter.enemy1 != null)
        {
            newEnemy1 = Instantiate(enemyPrefab,
                new Vector2(5f, 1), Quaternion.identity);

            enemy1 = new Enemy(encounter.enemy1, newEnemy1.GetComponent<InGameActor>());

            enemies.Add(enemy1);

        }
        if (encounter.enemy2 != null)
        {
            newEnemy2 = Instantiate(enemyPrefab,
               new Vector2(8f, 3), Quaternion.identity);

            enemy2 = new Enemy(encounter.enemy2, newEnemy2.GetComponent<InGameActor>());

            enemies.Add(enemy2);

        }
        if (encounter.enemy3 != null)
        {
            newEnemy3 = Instantiate(enemyPrefab,
               new Vector2(8f, 3), Quaternion.identity);

            enemy3 = new Enemy(encounter.enemy3, newEnemy3.GetComponent<InGameActor>());

            enemies.Add(enemy3);

        }
        
        
    }

    /*
    void DisposeHand(Hand hand)
    {
        for (int i = 0; i < hand.cards.Count; i++)
        {
            AddToDiscardPile(hand.cards[i]);
            hand.cards[i].DisposeInGameActor();
            hand.cards.RemoveAt(i);
            i--;
        }
    }*/
    void AddToDiscardPile(Card card)
    {
        discardPile.cards.Add(card);
    }

    public void DisplayCards(Card card, int cardDrawn, bool drawingHand)
    {
        if (drawingHand)
        {
            GameObject handCard = Instantiate(cardPrefab,
                new Vector2(-4f + cardDrawn * 2, -3), Quaternion.identity);
            InGameActor cardObj = handCard.GetComponent<InGameActor>();
            cardObj.objectName.text = card.cardName;
            cardObj.description.text = card.description;
            cardObj.description.fontSize = card.font;
            cardObj.costHealth.text = card.cost.ToString();
            cardObj.spriteRenderer.sprite = card.sprite;
            cardObj.card = card;
            card.inGameObject = cardObj;



        }
        else
        {
            GameObject handCard = Instantiate(cardPrefab, new Vector2
                (-4f + cardDrawn * 4f, 0), Quaternion.identity);
            handCard.transform.localScale = new Vector3(2f, 2f, 1f); 
            // Example scale values
            InGameActor cardObj = handCard.GetComponent<InGameActor>();
            cardObj.objectName.text = card.cardName;
            cardObj.description.text = card.description;
            cardObj.description.fontSize = card.font;
            cardObj.costHealth.text = card.cost.ToString();
            cardObj.spriteRenderer.sprite = card.sprite;
            cardObj.card = card;
            card.inGameObject = cardObj;

        }
    }
    void SelectNewCard()
    {
        for (int i = 0; i < rewardCards; i++)
        {
            int randomNum = UnityEngine.Random.Range(0, cardIndex.cardData.Length);
            Card card = new Card(cardIndex.cardData[randomNum]);
            tempCards.cards.Add(card);
            DisplayCards(tempCards.cards[i], i, false);

        }

        GameObject skipButton = Instantiate(buttonPrefab, new Vector2(0, -3.5f), Quaternion.identity);
        skipButton.transform.localScale = new Vector3(2f, 0.75f, 1f);
        button = skipButton.GetComponent<Button>();
        button.updateText(0);
    }
    public void AddCardDeck(Card card)
    {
        if(card != null)
        {
            playerDeckBase.cards.Add(card);
        }
        turnManager.DisposeHand(tempCards);
        DisposeButton(button);
        SelectTrinky();
        NextEncounter();

    }
    void DisposeButton(Button button)
    {

        UnityEngine.Object.Destroy(button.gameObject);
        button = null;        
    }
    void SelectTrinky()
    {
        //implement trinkets
    }



}
public enum PlayerCharacter { Hero, Null, TheVessel, TheEcho, WaterMage }
public enum BattleState { GameOver, Battle, PrePostBattle }