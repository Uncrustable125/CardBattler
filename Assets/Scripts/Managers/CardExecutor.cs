using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Handles card execution logic: validating mana, applying effects, and handling turn end/game over.
/// Extracted from BattleManager for clarity and modularity.
/// </summary>
public class CardExecutor
{
    private Player player;
    private Hand playerHand;
    private Deck discardPile;
    private List<Enemy> enemies;
    public static event Action<Card> OnPlayerAction;

    public CardExecutor(Player player, Hand hand, Deck discardPile, List<Enemy> enemies)
    {
        this.player = player;
        this.playerHand = hand;
        this.discardPile = discardPile;
        this.enemies = enemies;
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
}
