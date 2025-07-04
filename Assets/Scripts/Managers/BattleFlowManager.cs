using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

/// <summary>
/// Handles turn-based logic including ending a turn and resetting player state.
/// Extracted from BattleManager to separate responsibilities.
/// </summary>
public class BattleFlowManager
{
    private List<Enemy> enemies;
    private Player player;
    private BattleManager battleManager;
    private CardManager cardManager;
    private BattleConfig battleConfig;
    private SpawnManager spawnManager;
    private GameObject enemyPrefab, tempButton, buttonPrefab;
    private Button endTurnButton, skipButton, button;



    public BattleFlowManager(
        List<Enemy> enemies,
        Player player,
        BattleManager manager,
        CardManager cardManager,
        BattleConfig battleConfig,
        SpawnManager spawnManager,
        GameObject enemyPrefab, 
        GameObject buttonPrefab,
        Button endTurnButton)
    {
        this.enemies = enemies;
        this.player = player;
        this.battleManager = manager;
        this.cardManager = cardManager;
        this.battleConfig = battleConfig;
        this.spawnManager = spawnManager;
        this.enemyPrefab = enemyPrefab;
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
        spawnManager.DisposeButton(button);
        battleConfig.gameOverText.alpha = 0;
        BattleManager.Instance.battleState = BattleState.PrePostBattle;
        endTurnButton.ReturnToOriginalPos();
        BattleManager.Instance.gameOver = false;
    }
    //All button stuff here
    public void BattleGameOver()
    {
        cardManager.CardsGameOver();
        battleConfig.gameOverText.alpha = 1.0f;
        //Destroy all objects
        foreach (Enemy x in enemies)
        {
            x.Dispose();
        }
        enemies.Clear();
        player.Dispose();

        spawnManager.SpawnSkipButton(1);

        //Spawn in Game Over 

        BattleManager.Instance.action = false;
        endTurnButton.RemoveFromScreen();
        BattleManager.Instance.battleState = BattleState.GameOver;
    }

    public void NextEncounter()
    {
        endTurnButton.ReturnToOriginalPos();
        cardManager.ReShuffle();//with new card
        Encounter encounter = new Encounter(UnityEngine.Random.Range(0, 3));

        spawnManager.SpawnEnemies(encounter, enemyPrefab, enemies);
        CreateEnemyAttackList();
        cardManager.DrawCards();
        BattleManager.Instance.battleState = BattleState.Battle;
    }
    void CreateEnemyAttackList()
    {
        foreach (Enemy x in enemies)
        {
            x.enemyAttackSelect();
        }
    }
    public void CheckForGameOver()
    {
        if (player.health <= 0 && !BattleManager.Instance.gameOver)
        {
            BattleManager.Instance.gameOver = true;
            BattleGameOver();
        }
    }
    public void EndEncounter(Hand playerHand)
    {
        BattleManager.Instance.battleState = BattleState.PrePostBattle;
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
        BattleManager.Instance.action = false;
        CheckForGameOver();
    }

}
