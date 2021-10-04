using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneratorController : MonoBehaviour
{
    [Header("Templates")]
    [SerializeField] private List<GameObject> terrainTemplates;
    [SerializeField] private float terrainTemplateWidth;

    [Header("Force Early Template")]
    [SerializeField] private List<GameObject> earlyTemplate;

    [Header("Generator Area")]
    [SerializeField] private Camera gameCamera;
    [SerializeField] private float areaStartOffset;
    [SerializeField] private float areaEndOffset;

    private const float debugLineHeight = 10.0f;

    private List<GameObject> spawnedTerrain;

    private float lastGeneratedPositionX;
    private float lastRemovedPositionX;

    private Dictionary<string, List<GameObject>> pool;

    private void Start()
    {
        pool = new Dictionary<string, List<GameObject>>();

        spawnedTerrain = new List<GameObject>();
        lastGeneratedPositionX = GetHorizontalPositionStart();
        lastRemovedPositionX = lastGeneratedPositionX - terrainTemplateWidth;

        foreach(GameObject item in earlyTemplate)
        {
            GenerateTerrain(lastGeneratedPositionX, item);
            lastGeneratedPositionX += terrainTemplateWidth;
        }

        while (lastGeneratedPositionX < GetHorizontalPositionEnd())
        {
            GenerateTerrain(lastGeneratedPositionX);
            lastGeneratedPositionX += terrainTemplateWidth;
        }
    }

    private void Update()
    {
        while (lastGeneratedPositionX < GetHorizontalPositionEnd())
        {
            GenerateTerrain(lastGeneratedPositionX);
            lastGeneratedPositionX += terrainTemplateWidth;
        }

        while(lastRemovedPositionX + terrainTemplateWidth < GetHorizontalPositionStart())
        {
            lastRemovedPositionX += terrainTemplateWidth;
            RemoveTerrain(lastRemovedPositionX);
        }
    }

    private float GetHorizontalPositionStart()
    {
        return gameCamera.ViewportToWorldPoint(new Vector2(0f, 0f)).x + areaStartOffset;
    }
    
    private float GetHorizontalPositionEnd()
    {
        return gameCamera.ViewportToWorldPoint(new Vector2(1f, 0f)).x + areaEndOffset;
    }

    private void GenerateTerrain(float posX, GameObject go=null)
    {
        GameObject newTerrain = go == null ? GenerateFromPool(terrainTemplates[Random.Range(0, terrainTemplates.Count)], transform)
            : GenerateFromPool(go, transform);

        newTerrain.transform.position = new Vector2(posX, 0f);
        spawnedTerrain.Add(newTerrain);
    }

    private void RemoveTerrain(float posX)
    {
        GameObject removed = null;
        foreach(GameObject item in spawnedTerrain)
        {
            if (item.transform.position.x == posX)
            {
                removed = item;
                break;
            }
        }
        if (removed != null)
        {
            spawnedTerrain.Remove(removed);
            ReturnToPool(removed);
        }
    }

    private GameObject GenerateFromPool(GameObject item, Transform prt)
    {
        if (pool.ContainsKey(item.name))
        {
            if (pool[item.name].Count > 0)
            {
                GameObject newItemFromPool = pool[item.name][0];
                pool[item.name].Remove(newItemFromPool);
                newItemFromPool.SetActive(true);
                return newItemFromPool;
            }
        }
        else
        {
            pool.Add(item.name, new List<GameObject>());
        }
        GameObject newItem = Instantiate(item, prt);
        newItem.name = item.name;
        return newItem;
    }

    private void ReturnToPool(GameObject item)
    {
        if (!pool.ContainsKey(item.name))
        {
            Debug.LogError("Invalid Pool Item!");
        }

        pool[item.name].Add(item);
        item.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Vector3 areaStartPosition = transform.position;
        Vector3 areaEndPosition = transform.position;

        areaStartPosition.x = GetHorizontalPositionStart();
        areaEndPosition.x = GetHorizontalPositionEnd();

        Debug.DrawLine(areaStartPosition + Vector3.up * debugLineHeight / 2, transform.position + Vector3.down * debugLineHeight / 2, Color.red);
    }
}