using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles turn-based logic including ending a turn, drawing cards, and resetting player state.
/// Extracted from BattleManager to separate responsibilities.
/// </summary>
public class TurnManager
{
    private Deck deck;
    private Deck discardPile;
    private Hand playerHand;
    private List<Enemy> enemies;
    private Player player;
    private BattleManager battleManager;

    private int standardDraw = 5;
    private int extraDraw = 0;

    public TurnManager(
        Deck deck,
        Deck discardPile,
        Hand hand,
        List<Enemy> enemies,
        Player player,
        BattleManager manager,
        int standardDraw = 5,
        int extraDraw = 0)
    {
        this.deck = deck;
        this.discardPile = discardPile;
        this.playerHand = hand;
        this.enemies = enemies;
        this.player = player;
        this.battleManager = manager;
        this.standardDraw = standardDraw;
        this.extraDraw = extraDraw;
    }

    /// <summary>
    /// Called when the player ends their turn. Enemies take actions, cards are drawn, and player is reset.
    /// </summary>
    public void EndTurn()
    {
        DisposeHand(playerHand);

        if (enemies.Count != 0)
        {
            foreach (var enemy in enemies)
            {
                enemy.EnemyTurn();
            }

            DrawCards();
        }

        foreach (var enemy in enemies)
        {
            enemy.enemyAttackSelect();
        }

        player.playerTurnReset();
    }

    /// <summary>
    /// Draws cards into the player's hand from the current deck.
    /// </summary>
    public void DrawCards()
    {
        int drawCount = standardDraw + extraDraw;

        for (int currentDraw = 0; currentDraw < drawCount; currentDraw++)
        {
            if (deck.cards.Count == 0)
            {
                Reshuffle();
            }

            if (deck.cards.Count == 0) break; // no cards left to draw

            Card drawnCard = deck.cards[0];
            deck.cards.RemoveAt(0);
            playerHand.cards.Add(drawnCard);

            battleManager.DisplayCards(drawnCard, currentDraw, true);
        }
    }

    /// <summary>
    /// Transfers all cards in the player's hand to the discard pile and disposes their visuals.
    /// </summary>
    public void DisposeHand(Hand hand)
    {
        for (int i = 0; i < hand.cards.Count; i++)
        {
            Card card = hand.cards[i];
            discardPile.cards.Add(card);
            card.DisposeInGameActor();
        }

        hand.cards.Clear();
    }

    /// <summary>
    /// Shuffles the discard pile back into the main deck.
    /// </summary>
    private void Reshuffle()
    {
        deck.cards.AddRange(discardPile.cards);
        discardPile.cards.Clear();
        deck.Shuffle();
    }
}
