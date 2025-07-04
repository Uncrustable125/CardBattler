using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Handles card execution logic: validating mana, applying effects, and handling turn end/game over.
/// Extracted from BattleManager for clarity and modularity.
/// </summary>
public class CardManager
{

    private Deck playerDeckBase;
    private Deck discardPile;
    private Deck currentPlayerDeck;
    private Hand playerHand;
    private Hand tempCards;
    private CardIndex cardIndex;
    private GameObject cardPrefab;
    private SpawnManager spawnManager;
    private BattleManager battleFlowManager;
    private GameController battleManager;
    private Player player;
    private List<Enemy> enemies;
    public int rewardCards, extraDraw, drawCount;
    public static event Action<Card> OnPlayerAction;

    public CardManager(BattleConfig battleConfig,
        SpawnManager spawnManager)
    {
        this.playerDeckBase = battleConfig.playerDeckBase;
        this.discardPile = battleConfig.discardPile;
        this.currentPlayerDeck = battleConfig.currentPlayerDeck;
        this.playerHand = battleConfig.playerHand;
        this.tempCards = battleConfig.tempCards;
        this.cardIndex = battleConfig.cardIndex;
        this.cardPrefab = battleConfig.cardPrefab;
        this.spawnManager = spawnManager;
        this.rewardCards = battleConfig.rewardCards;
        this.player = battleConfig.player;
        this.enemies = battleConfig.enemies;
        this.extraDraw = battleConfig.extraDraw;
        drawCount = 5 + battleConfig.extraDraw; //5 is standard draw
    }

    // All methods go here (SelectNewCard, DisplayCard, AddCardDeck, etc.)
    public void SetBattleManager(BattleManager battleFlowManager)
    {
        this.battleFlowManager = battleFlowManager;
    }

    public void SelectNewCard()
    {
        for (int i = 0; i < rewardCards; i++)
        {
            int randomNum = UnityEngine.Random.Range(0, cardIndex.cardData.Length);
            Card card = new Card(cardIndex.cardData[randomNum]);
            tempCards.cards.Add(card);
            DisplayCard(tempCards.cards[i], i, false);

        }
		spawnManager.SpawnSkipButton(0);

    }

    /// <summary>
    /// Draws cards into the player's hand from the current deck.
    /// </summary>
    public void DrawCards()
    {
        

        for (int currentDraw = 0; currentDraw < drawCount; currentDraw++)
        {
            if (currentPlayerDeck.cards.Count == 0)
            {
                ReShuffle();
            }

            if (currentPlayerDeck.cards.Count == 0) break; // no cards left to draw

            Card drawnCard = currentPlayerDeck.cards[0];
            currentPlayerDeck.cards.RemoveAt(0);
            playerHand.cards.Add(drawnCard);
            DisplayCard(drawnCard, currentDraw, true);
        }
    }

    public void AddCardDeck(Card card)
    {
        if (card != null)
        {
            playerDeckBase.cards.Add(card);
        }
        DisposeHand(tempCards);
        spawnManager.DisposeButton();
    }
    /// <summary>
    /// Transfers all cards in the player's hand to the discard pile and disposes their visuals.
    /// </summary>
    public void DisposeHand(Hand hand = null)
    {
        if(hand == null)
        {
            hand = playerHand;
        }
        for (int i = 0; i < hand.cards.Count; i++)
        {
            Card card = hand.cards[i];
            discardPile.cards.Add(card);
            card.DisposeInGameActor();
        }

        hand.cards.Clear();
    }
    /// <summary>
    /// Attempts to play all active cards in the player's hand.
    /// Handles card execution, enemy targeting, win condition, and game over.
    /// </summary>
    public void TryPlayCards()
    {
        for (int i = 0; i < playerHand.cards.Count; i++)
        {
            Card card = playerHand.cards[i];

            if (!card.currentCard) continue;

            if (player.mana >= card.cost)
            {
                if (card.noTarget)
                {
                    enemies.ForEach(e => e.isTargeted = true);
                }

                card.currentCard = false;
                discardPile.cards.Add(card);
                OnPlayerAction?.Invoke(card);
                i--; // Adjust index after removal
            }
            else
            {
                card.currentCard = false;
                enemies.ForEach(e => e.isTargeted = false);
                Debug.Log("Not Enough Mana!");

                var cardPos = card.inGameObject.GetComponent<TargetingCard>();
                cardPos.transform.position = cardPos.origin;
            }
        }

    }
    public void DisplayCard(Card card, int cardDrawn, bool drawingHand)
    {
        if (drawingHand)
        {
            GameObject handCard = spawnManager.Spawn(cardPrefab, new Vector3(-4f + cardDrawn * 2f, -3f, 0f));
            InGameActor cardObj = handCard.GetComponent<InGameActor>();
            cardObj.objectName.text = card.cardName;
            cardObj.description.text = card.description;
            cardObj.description.fontSize = card.font;
            cardObj.costHealth.text = card.cost.ToString();
            cardObj.spriteRenderer.sprite = card.sprite;
            cardObj.card = card;
            card.inGameObject = cardObj;
        }
        else // Picking New Card
        {
            GameObject handCard = spawnManager.Spawn(cardPrefab, new Vector3(-4f + cardDrawn * 4f, 0f, 0f));
            handCard.transform.localScale = new Vector3(2f, 2f, 1f);
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
    /// <summary>
    /// Shuffles the discard pile back into the main deck.
    /// </summary>
    public void ReShuffle()
    {
        if (GameController.Instance.battleState == BattleState.Battle)
        {
            CopyAndShuffleDeck(currentPlayerDeck, discardPile);
        }
        else if (GameController.Instance.battleState == BattleState.PrePostBattle)
        {
            CopyAndShuffleDeck(currentPlayerDeck, playerDeckBase);
        }
        discardPile.cards.Clear();
    }
    public void CopyAndShuffleDeck(Deck deck, Deck deckToCopy)
    {
        deck.CopyDeck(deckToCopy);
        deck.Shuffle();
    }

    public void CardsGameOver()
    {
        DisposeHand(playerHand);
        DisposeHand(tempCards);

        playerDeckBase.clear();
        currentPlayerDeck.clear();
        discardPile.clear();
    }
}

