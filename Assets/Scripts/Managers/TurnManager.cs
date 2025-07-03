using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles turn-based logic including ending a turn and resetting player state.
/// Extracted from BattleManager to separate responsibilities.
/// </summary>
public class TurnManager
{
    private List<Enemy> enemies;
    private Player player;
    private BattleManager battleManager;
    private CardManager cardManager;
    private BattleConfig battleConfig;
    private PrefabManager prefabManager;
    private GameObject buttonPrefab;
    private Button endTurnButton, skipButton, button;
    private GameObject tempButton;



    public TurnManager(
        List<Enemy> enemies,
        Player player,
        BattleManager manager,
        CardManager cardManager,
        BattleConfig battleConfig,
        PrefabManager prefabManager,
        GameObject buttonPrefab, 
        Button endTurnButton)
    {
        this.enemies = enemies;
        this.player = player;
        this.battleManager = manager;
        this.cardManager = cardManager;
        this.battleConfig = battleConfig;
        this.prefabManager = prefabManager;
        this.buttonPrefab = buttonPrefab;
        
        this.endTurnButton = endTurnButton;

    }

    /// <summary>
    /// Called when the player ends their turn. Enemies take actions, cards are drawn, and player is reset.
    /// </summary>
    public void EndTurn()
    {
        cardManager.DisposeHand();
        if (enemies.Count != 0)
        {
            foreach (var enemy in enemies)
            {

                enemy.EnemyTurn();
                if (BattleManager.Instance.gameOver)
                {
                    break;
                }
            }

            cardManager.DrawCards();
        }

        foreach (var enemy in enemies)
        {

            enemy.enemyAttackSelect();
            if (BattleManager.Instance.gameOver)
            {
                break;
            }
        }

        player.playerTurnReset();
    }
    public void restartGame()
    {
        DisposeButton(button);
        battleConfig.gameOverText.alpha = 0;


        BattleManager.Instance.battleState = BattleState.PrePostBattle;

        endTurnButton.ReturnToOriginalPos();
        BattleManager.Instance.gameOver = false;
    }
    public void DisposeButton(Button button = null)
    {
        if(button == null)
        {
            button = tempButton.GetComponent<Button>();
        }
        UnityEngine.Object.Destroy(button.gameObject);
        button = null;
    }
    //All button stuff here
    public void GameOver()
    {
        battleConfig.gameOverText.alpha = 1.0f;
        //Destroy all objects
        foreach (Enemy x in enemies)
        {
            x.Dispose();
        }
        enemies.Clear();
        player.Dispose();

        SpawnSkipButton(1);

        //Spawn in Game Over 

        BattleManager.Instance.action = false;
        endTurnButton.RemoveFromScreen();
        BattleManager.Instance.battleState = BattleState.GameOver;
    }
    public void SpawnSkipButton(int textSelection)
    {
        tempButton = prefabManager.Spawn(buttonPrefab, new Vector3(0f, -3.5f, 0f));
        tempButton.transform.localScale = new Vector3(2f, 0.75f, 1f);
        tempButton.GetComponent<Button>().updateText(textSelection);
    }


}
