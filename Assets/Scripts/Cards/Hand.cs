using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public List<Card> cards;

    void OnEnable()
    {
        BattleManager.OnAction += CurrentAction;
    }

    public void clear()
    {
        cards.Clear();
    }
    void OnDisable()
    {
        BattleManager.OnAction -= CurrentAction;
    }
    public Hand()
    {      
        this.cards = new List<Card>();
    }
    public void CurrentAction(Card card)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (UnityEngine.Object.ReferenceEquals(card, cards[i]))
            {
                card.DisposeInGameActor();
                cards.RemoveAt(i);
            }


        }
    }
}