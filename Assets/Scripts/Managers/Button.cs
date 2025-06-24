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
        if (BattleManager.Instance.gameState == GameState.Battle)
        {
            BattleManager.Instance.TurnEnd();
            Debug.Log("End Turn!");
            
        }
        else if (BattleManager.Instance.gameState == GameState.PrePostBattle)
        {
            BattleManager.Instance.AddCardDeck(null);
            BattleManager.Instance.endTurnButton.transform.position = 
                BattleManager.Instance.endTurnButton.originalPos;
        }
        else if (BattleManager.Instance.gameState == GameState.GameOver)
        {
            BattleManager.Instance.restartGame();
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
