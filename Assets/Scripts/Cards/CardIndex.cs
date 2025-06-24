using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardIndex : MonoBehaviour
{
    public CardData[] cardData;
    public void LoadCards(PlayerCharacter playerCharacter)
    {
        cardData = Resources.LoadAll("Cards/"+ playerCharacter.ToString(),
    typeof(CardData)).Cast<CardData>().ToArray();
       
        //foreach (var x in cardData)
          //  Debug.Log(x.name);
    }
}
