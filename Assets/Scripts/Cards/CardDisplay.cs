using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public CardData cardData;

    public Image artworkImage;
    public TMP_Text cardNameText;
    public TMP_Text cardCostText;
    public TMP_Text cardDescriptionText;

    void Start()
    {
        artworkImage.sprite = cardData.sprite;
        cardNameText.text = cardData.cardName;
        cardCostText.text = cardData.cost.ToString();
        cardDescriptionText.text = cardData.description;
    }
}
