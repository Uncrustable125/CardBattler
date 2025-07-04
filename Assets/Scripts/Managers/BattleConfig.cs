using System.Collections.Generic;
using UnityEngine;

public class BattleConfig
{
    public GameObject cardPrefab;
    public GameObject enemyPrefab;
    public GameObject playerPrefab;
    public GameObject buttonPrefab;
    public Button endTurnButton;
    public PlayerCharacter character;
    public TMPro.TextMeshProUGUI gameOverText;
    public Transform cameraParent;

    public Deck playerDeckBase;
    public Deck discardPile;
    public Deck currentPlayerDeck;
    public Hand playerHand;
    public Hand tempCards;
    public CardIndex cardIndex;
    public Player player;
    public List<Enemy> enemies;
    public int rewardCards, extraDraw, drawCount;



    public BattleConfig(
    GameObject cardPrefab,
    GameObject enemyPrefab,
    GameObject playerPrefab,
    GameObject buttonPrefab,
    Button endTurnButton,
    PlayerCharacter character,
    TMPro.TextMeshProUGUI gameOverText,
    Transform cameraParent,
    Deck playerDeckBase,
    Deck discardPile,
    Deck currentPlayerDeck,
    Hand playerHand,
    Hand tempCards,
    CardIndex cardIndex,
    Player player,
    List<Enemy> enemies,
    int rewardCards,
    int extraDraw
)
    {
        this.cardPrefab = cardPrefab;
        this.enemyPrefab = enemyPrefab;
        this.playerPrefab = playerPrefab;
        this.buttonPrefab = buttonPrefab;
        this.endTurnButton = endTurnButton;
        this.character = character;
        this.gameOverText = gameOverText;
        this.cameraParent = cameraParent;

        this.playerDeckBase = playerDeckBase;
        this.discardPile = discardPile;
        this.currentPlayerDeck = currentPlayerDeck;
        this.playerHand = playerHand;
        this.tempCards = tempCards;
        this.cardIndex = cardIndex;
        this.player = player;
        this.enemies = enemies;
        this.rewardCards = rewardCards;
        this.extraDraw = extraDraw;
        this.drawCount = 5 + extraDraw;
    }

}
