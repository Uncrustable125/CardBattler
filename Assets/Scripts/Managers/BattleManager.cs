using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

/// <summary>
/// Handles turn-based logic including ending a turn and resetting player state.
/// Extracted from BattleManager to separate responsibilities.
/// </summary>
public class BattleManager
{
    private List<Enemy> enemies;
    private Player player;
    private CardManager cardManager;
    private TMPro.TextMeshProUGUI gameOverText;
    private SpawnManager spawnManager;
    private GameObject enemyPrefab, tempButton, buttonPrefab;
    private Button endTurnButton, skipButton, button;



    public BattleManager(BattleConfig battleConfig, 
        CardManager cardManager,
        SpawnManager spawnManager)
    {
        this.enemies = battleConfig.enemies;
        this.player = battleConfig.player;
        this.cardManager = cardManager;
        this.spawnManager = spawnManager;
        this.enemyPrefab = battleConfig.enemyPrefab;
        this.buttonPrefab = battleConfig.buttonPrefab;
        this.endTurnButton = battleConfig.endTurnButton;
        this.gameOverText = battleConfig.gameOverText;
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
                if (GameController.Instance.gameOver)
                {
                    break;
                }
            }

            cardManager.DrawCards();
        }

        foreach (var enemy in enemies)
        {

            enemy.EnemyAttackSelect();
            if (GameController.Instance.gameOver)
            {
                break;
            }
        }

        player.playerTurnReset();
    }
    public void restartGame()
    {
        spawnManager.DisposeButton(button);
        gameOverText.alpha = 0;
        GameController.Instance.battleState = BattleState.PrePostBattle;
        endTurnButton.ReturnToOriginalPos();
        GameController.Instance.gameOver = false;

    }
    //All button stuff here
    public void BattleGameOver()
    {
        cardManager.CardsGameOver();
        gameOverText.alpha = 1.0f;
        //Destroy all objects
        foreach (Enemy x in enemies)
        {
            x.Dispose();
        }
        enemies.Clear();
        player.Dispose();

        spawnManager.SpawnSkipButton(1);

        //Spawn in Game Over 

        GameController.Instance.action = false;
        endTurnButton.RemoveFromScreen();
        GameController.Instance.battleState = BattleState.GameOver;
    }

    public void NextEncounter()
    {
        player.playerLevelReset();
        endTurnButton.ReturnToOriginalPos();
        cardManager.ReShuffle();//with new card
        Encounter encounter = new Encounter(UnityEngine.Random.Range(0, 3));

        spawnManager.SpawnEnemies(encounter, enemyPrefab, enemies);
        CreateEnemyAttackList();
        cardManager.DrawCards();
        GameController.Instance.battleState = BattleState.Battle;
    }
    void CreateEnemyAttackList()
    {
        foreach (Enemy x in enemies)
        {
            x.EnemyAttackSelect();
        }
    }
    public void CheckForGameOver()
    {
        if (player.health <= 0 && !GameController.Instance.gameOver)
        {
            GameController.Instance.gameOver = true;
            BattleGameOver();
        }
    }
    public void EndEncounter(Hand playerHand)
    {
        GameController.Instance.battleState = BattleState.PrePostBattle;
        cardManager.DisposeHand(playerHand);
        endTurnButton.RemoveFromScreen();
        cardManager.SelectNewCard();
    }
    public void ResolvePostAction(Hand playerHand)
    {
        enemies.RemoveAll(e => e.isDead);

        if (enemies.Count == 0)
        {
            EndTurn();
            EndEncounter(playerHand);
        }
        GameController.Instance.action = false;
        CheckForGameOver();
    }

}
