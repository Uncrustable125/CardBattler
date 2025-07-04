
using System.Collections.Generic;
using UnityEngine;


public class Deck : MonoBehaviour
{
    public List<Card> cards;
    public PlayerCharacter playerCharacter;
    List<Card> tempCards;
    bool debug;
    private void Awake()
    {
        debug = true;
        cards = new List<Card>();

    }
    public Deck CopyDeck(Deck deck)
    {
        cards = new List<Card>(deck.cards);
        playerCharacter = deck.playerCharacter;

        return deck;
    }

    public void CreateDeck()
    {
        CreateStarterDeck();
    }
    public void clear()
    {
        cards.Clear();
    }
    void CreateStarterDeck()
    {
        //Make a function that will find card by name

        cards = new List<Card>();
        if (debug)
        {
            foreach (var xc in GameController.Instance.cardIndex.cardData)
            {
                Card c = new Card(xc);
                cards.Add(c);
            }


        }
        else
        {
            foreach (var xc in GameController.Instance.cardIndex.cardData)
            {
                if (xc.cardName == "Strike")
                {
                    for (int j = 0; j < 5; j++)
                    {
                        Card c = new Card(xc);
                        cards.Add(c);
                    }
                }
                else if (xc.cardName == "Defend")
                {
                    for (int j = 0; j < 5; j++)
                    {
                        Card c = new Card(xc);
                        cards.Add(c);
                    }
                }
                else if (xc.cardName == "Shattered")
                {

                    Card c = new Card(xc);
                    cards.Add(c);

                }
            }
        }

    }
    public void DrawCard()
    {
        if (cards.Count == 0)
        {
            Shuffle();
        }
        Card drawnCard = cards[0];
        cards.RemoveAt(0);
        // Add the drawn card to the player's hand
    }

    public void Shuffle()
    {

        //No need to Seed the Shuffle
        //Just Save after the shuffle


        List<Card> tempCards = new List<Card>(cards);
        for (int i = cards.Count; i > 0; i--)
        {
            int randomNum = UnityEngine.Random.Range(0, i);
            tempCards[i - 1] = cards[randomNum];
            cards.RemoveAt(randomNum);
        }
        cards = tempCards;
    }

}