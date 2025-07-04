using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// Centralized helper for instantiating and managing prefab spawns.
/// Avoids duplicated Instantiate code and keeps BattleManager clean.
/// </summary>
public class SpawnManager
{
    private Transform defaultParent;
    GameObject tempButton, buttonPrefab;
    public SpawnManager(GameObject buttonPrefab, Transform defaultParent = null)
    {
        this.defaultParent = defaultParent;
        this.buttonPrefab = buttonPrefab;
    }

    /// <summary>
    /// Spawns a prefab at a world position with optional parent override.
    /// </summary>
    public GameObject Spawn(GameObject prefab, Vector3 position, Transform parentOverride = null)
    {
        if (prefab == null)
        {
            Debug.LogError("Tried to spawn a null prefab.");
            return null;
        }

        Transform parent = parentOverride ?? defaultParent;
        GameObject obj = Object.Instantiate(prefab, position, Quaternion.identity, parent);

        //Scale to size
        obj.transform.localScale = prefab.transform.localScale;
        return obj;

    }
    public void SpawnSkipButton(int textSelection)
    {
        tempButton = Spawn(buttonPrefab, new Vector3(0f, -3.5f, 0f));
        tempButton.transform.localScale = new Vector3(2f, 0.75f, 1f);
        tempButton.GetComponent<Button>().updateText(textSelection);
    }
    public void SpawnPlayer(GameObject playerPrefab, Player player)
    {
        GameObject newPlayer = Spawn(playerPrefab, new Vector3(-7f, 2f, 0f));
        player.inGameActor = newPlayer.GetComponent<InGameActor>();
        player.UpdateTexts();
    }
    public void DisposeButton(Button button = null)
    {
        if (button == null)
        {
            button = tempButton.GetComponent<Button>();
        }
        UnityEngine.Object.Destroy(button.gameObject);
        button = null;
    }
    public void SpawnEnemies(Encounter encounter, GameObject enemyPrefab, List<Enemy> enemies)
    {
        if (encounter.enemy0 != null)
        {
            GameObject newEnemy0 = Spawn(enemyPrefab, new Vector3(2f, 3f, 0f));
            Enemy enemy0 = new Enemy(encounter.enemy0, newEnemy0.GetComponent<InGameActor>());
            enemies.Add(enemy0);
        }

        if (encounter.enemy1 != null)
        {
            GameObject newEnemy1 = Spawn(enemyPrefab, new Vector3(5f, 1f, 0f));
            Enemy enemy1 = new Enemy(encounter.enemy1, newEnemy1.GetComponent<InGameActor>());
            enemies.Add(enemy1);
        }

        if (encounter.enemy2 != null)
        {
            GameObject newEnemy2 = Spawn(enemyPrefab, new Vector3(8f, 3f, 0f));
            Enemy enemy2 = new Enemy(encounter.enemy2, newEnemy2.GetComponent<InGameActor>());
            enemies.Add(enemy2);
        }

        if (encounter.enemy3 != null)
        {
            GameObject newEnemy3 = Spawn(enemyPrefab, new Vector3(11f, 3f, 0f));
            Enemy enemy3 = new Enemy(encounter.enemy3, newEnemy3.GetComponent<InGameActor>());
            enemies.Add(enemy3);
        }
    }

    /// <summary>
    /// Spawns a prefab with localPosition instead of world space.
    /// Useful for UI or anchored transforms.
    /// </summary>
    public GameObject SpawnLocal(GameObject prefab, Vector3 localPosition, Transform parentOverride = null)
    {
        GameObject obj = Spawn(prefab, Vector3.zero, parentOverride);
        if (obj != null)
        {
            obj.transform.localPosition = localPosition;
        }
        return obj;
    }

    /// <summary>
    /// Quickly attach prefab to a parent without caring about position.
    /// </summary>
    public GameObject Attach(GameObject prefab, Transform parentOverride = null)
    {
        return Spawn(prefab, Vector3.zero, parentOverride);
    }
}
