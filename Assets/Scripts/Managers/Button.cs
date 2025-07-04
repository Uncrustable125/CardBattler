using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI description;
    Vector3 originalPos;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        originalPos = transform.position;
    }

    private void OnMouseDown()
    {
        if (GameController.Instance.battleState == BattleState.Battle)
        {
            GameController.Instance.TurnEnd();
            Debug.Log("End Turn!");
            
        }
        else if (GameController.Instance.battleState == BattleState.PrePostBattle)
        {
            GameController.Instance.AddCardDeck(null);
            GameController.Instance.endTurnButton.transform.position = 
                GameController.Instance.endTurnButton.originalPos;
        }
        else if (GameController.Instance.battleState == BattleState.GameOver)
        {
            GameController.Instance.restartGame();
        }

    }
    public void RemoveFromScreen()
    {
        Vector3 x = new Vector3(15,0,10);
        transform.position = x;
    }
    public void ReturnToOriginalPos()
    {
        transform.position = originalPos;
    }

    public void updateText(int i)
    {
        if(i == 0)
        {
            description.text = "Skip ->";
        }
        else if (i == 1)
        {
            description.text = "Restart?";
        }
    }

}
