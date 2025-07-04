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
    //Enemy enemy0, enemy1, enemy2, enemy3;
    public Button endTurnButton;
    GameObject cardPrefab, enemyPrefab, playerPrefab, buttonPrefab, newPlayer;
    SpawnManager spawnManager; CardManager cardManager; BattleFlowManager battleFlowManager;

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
            battleFlowManager.ResolvePostAction(playerHand);
        }

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
		cardIndex = new CardIndex();
		cardIndex.LoadCards(playerCharacter);

		// Create game systems
		player = gameObject.AddComponent<Player>();
		playerDeckBase = gameObject.AddComponent<Deck>();
		currentPlayerDeck = gameObject.AddComponent<Deck>();
		discardPile = gameObject.AddComponent<Deck>();
		playerHand = gameObject.AddComponent<Hand>();
		tempCards = gameObject.AddComponent<Hand>();

		// Core logic handlers
		spawnManager = new SpawnManager(buttonPrefab, battleConfig.cameraParent);
		cardManager = new CardManager(playerDeckBase, discardPile, currentPlayerDeck, playerHand, tempCards, cardIndex, cardPrefab, spawnManager, player, enemies, 3, 0);
		battleFlowManager = new BattleFlowManager(enemies, player, this, cardManager, battleConfig, spawnManager, enemyPrefab, buttonPrefab, endTurnButton);
		cardManager.SetBattleFlowManager(battleFlowManager);

		// Battle flow config


		StartGame();
	}

	void StartGame()
	{
		//If new game
		player.SetStats(playerCharacter);
		playerDeckBase.CreateDeck();
		//else save file

		spawnManager.SpawnPlayer(playerPrefab, player);
		battleFlowManager.NextEncounter();
	}


    public void CheckForGameOver()
    {
        battleFlowManager.CheckForGameOver();
    }

    public void restartGame()
    {
        battleFlowManager.restartGame();
        StartGame();
    }
    public void Action()
    {
        action = true;
    }


    public void TurnEnd()
    {
        battleFlowManager.EndTurn();
    }

    public void AddCardDeck(Card card)
    {
        cardManager.AddCardDeck(card);
		battleFlowManager.NextEncounter();
	}

}
public enum PlayerCharacter { Hero, Null, TheVessel, TheEcho, WaterMage }
public enum BattleState { GameOver, Battle, PrePostBattle }