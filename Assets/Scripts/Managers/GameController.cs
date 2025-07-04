using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public static GameController Instance;

    BattleConfig battleConfig;
    ///public int extraDraw, rewardCards, standardDraw; implement later
    public bool action, gameOver;

    public CardIndex cardIndex;
    public Deck playerDeckBase, currentPlayerDeck, discardPile;
    Hand playerHand, tempCards;
    public Player player;
    public List<Enemy> enemies = new List<Enemy>();
    GameObject newPlayer;
    SpawnManager spawnManager; CardManager cardManager; BattleManager battleFlowManager;

    public PlayerCharacter playerCharacter;
    public BattleState battleState;

    Deck playerDeck;
    Deck savedCards;
    bool newGame;
  
    ///GameState gameState; //Implement pausing

    [SerializeField] GameObject cardPrefab, enemyPrefab, playerPrefab, buttonPrefab;
    [SerializeField] public Button endTurnButton;
    [SerializeField] TMPro.TextMeshProUGUI gameOverText;
    [SerializeField] private Transform cameraParent;
    void Start()
    {
        CreateGame();
    }

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
	public void CreateGame()
	{

        playerCharacter = PlayerCharacter.Hero;
        battleState = BattleState.PrePostBattle;

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


        //BattleConfig
        battleConfig = new BattleConfig(
            cardPrefab,
            enemyPrefab,
            playerPrefab,
            buttonPrefab,
            endTurnButton,
            playerCharacter,
            gameOverText,
            cameraParent,
            playerDeckBase,
            discardPile,
            currentPlayerDeck,
            playerHand,
            tempCards,
            cardIndex,
            player,
            enemies,
            3,
            0
        );
        // Core logic handlers
        spawnManager = new SpawnManager(buttonPrefab, battleConfig.cameraParent);
		cardManager = new CardManager(battleConfig, spawnManager);
		battleFlowManager = new BattleManager(battleConfig, cardManager, spawnManager );
		cardManager.SetBattleManager(battleFlowManager);

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