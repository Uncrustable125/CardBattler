using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    // GameController gc;
    BattleConfig battleConfig;

    int extraDraw, rewardCards, standardDraw;
    public bool action, gameOver;

    public CardIndex cardIndex;
    public Deck playerDeckBase, currentPlayerDeck, discardPile;
    Hand playerHand, tempCards;
    public Player player;
    public List<Enemy> enemies = new List<Enemy>();
    Enemy enemy0, enemy1, enemy2, enemy3;
    public Button endTurnButton;
    GameObject cardPrefab, enemyPrefab, playerPrefab, buttonPrefab,
        newEnemy0, newEnemy1, newEnemy2, newEnemy3, newPlayer;
    PrefabManager prefabManager; CardManager cardManager; TurnManager turnManager;

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
            cardManager.TryPlayCards();
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
    public void GameOver()
    {
        cardManager.GameOver();
        turnManager.GameOver();
    }
    void DisposeHand(Hand hand)
    {
        cardManager.DisposeHand(hand);
    }
    public void restartGame()
    {
        turnManager.restartGame();
        StartGame();
    }
    public void Action()
    {
        action = true;
    }


    public void CreateGame(BattleConfig battleConfig)
    {
        battleState = BattleState.PrePostBattle;
        // Load prefabs from GameController
        this.battleConfig = battleConfig;
        cardPrefab = battleConfig.cardPrefab;
        enemyPrefab = battleConfig.enemyPrefab;
        playerPrefab = battleConfig.playerPrefab;
        buttonPrefab = battleConfig.buttonPrefab;
        endTurnButton = battleConfig.endTurnButton;
        playerCharacter = battleConfig.character;
        if (endTurnButton == null)
            Debug.LogError("End Turn Button not found!");

        // Load card index and player-specific cards
        cardIndex = gameObject.AddComponent<CardIndex>();
        cardIndex.LoadCards(playerCharacter);

        // Create game systems
        player = gameObject.AddComponent<Player>();
        playerDeckBase = gameObject.AddComponent<Deck>();
        currentPlayerDeck = gameObject.AddComponent<Deck>();
        discardPile = gameObject.AddComponent<Deck>();
        playerHand = gameObject.AddComponent<Hand>();
        tempCards = gameObject.AddComponent<Hand>();

        // Core logic handlers
        prefabManager = new PrefabManager(battleConfig.cameraParent);
        cardManager = new CardManager(playerDeckBase, discardPile, currentPlayerDeck, playerHand, tempCards, cardIndex, cardPrefab, prefabManager, player, enemies, 3, 0);
        turnManager = new TurnManager(enemies, player, this, cardManager, battleConfig, prefabManager, buttonPrefab, endTurnButton);
        cardManager.SetTurnManager(turnManager);

        // Battle flow config


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

    public void StartFirstBattle()
    {
        SpawnPlayer();
        NextEncounter();
        SpawnEnemyAttacks();
    }

    void SpawnEnemyAttacks()
    {
        foreach (Enemy x in enemies)
        {
            x.enemyAttackSelect();
        }
    }
    public void EndEncounter()
    {
        battleState = BattleState.PrePostBattle;
        DisposeHand(playerHand);
        endTurnButton.RemoveFromScreen();
        SelectNewCard();

    }
    void SelectNewCard()
    {
        cardManager.SelectNewCard();
    }
    public void NextEncounter()
    {
        endTurnButton.ReturnToOriginalPos();
        ReShuffle();//with new card
        Encounter encounter = gameObject.AddComponent<Encounter>();
        encounter.GetEncounter(UnityEngine.Random.Range(0, 3));

        SpawnEnemies(encounter);
        SpawnEnemyAttacks();
        DrawCards();
        battleState = BattleState.Battle;
    }
    void DrawCards()
    {
        cardManager.DrawCards();
    }

    public void TurnEnd()
    {
        turnManager.EndTurn();
    }

    void ReShuffle()
    {
        cardManager.ReShuffle();
    }





    void SpawnPlayer()
    {
        newPlayer = prefabManager.Spawn(playerPrefab, new Vector3(-7f, 2f, 0f));
        player.inGameActor = newPlayer.GetComponent<InGameActor>();
        player.UpdateTexts();
    }



    void SpawnEnemies(Encounter encounter)
    {
        if (encounter.enemy0 != null)
        {
            newEnemy0 = prefabManager.Spawn(enemyPrefab, new Vector3(2f, 3f, 0f));
            enemy0 = new Enemy(encounter.enemy0, newEnemy0.GetComponent<InGameActor>());
            enemies.Add(enemy0);
        }

        if (encounter.enemy1 != null)
        {
            newEnemy1 = prefabManager.Spawn(enemyPrefab, new Vector3(5f, 1f, 0f));
            enemy1 = new Enemy(encounter.enemy1, newEnemy1.GetComponent<InGameActor>());
            enemies.Add(enemy1);
        }

        if (encounter.enemy2 != null)
        {
            newEnemy2 = prefabManager.Spawn(enemyPrefab, new Vector3(8f, 3f, 0f));
            enemy2 = new Enemy(encounter.enemy2, newEnemy2.GetComponent<InGameActor>());
            enemies.Add(enemy2);
        }

        if (encounter.enemy3 != null)
        {
            newEnemy3 = prefabManager.Spawn(enemyPrefab, new Vector3(11f, 3f, 0f));
            enemy3 = new Enemy(encounter.enemy3, newEnemy3.GetComponent<InGameActor>());
            enemies.Add(enemy3);
        }
    }


    public void DisplayCard(Card card, int cardDrawn, bool drawingHand)
    {
        cardManager.DisplayCard(card, cardDrawn, drawingHand);
    }


    public void AddCardDeck(Card card)
    {
        cardManager.AddCardDeck(card);
        NextEncounter();
    }
    void SelectTrinky()
    {
        //implement trinkets
    }



}
public enum PlayerCharacter { Hero, Null, TheVessel, TheEcho, WaterMage }
public enum BattleState { GameOver, Battle, PrePostBattle }