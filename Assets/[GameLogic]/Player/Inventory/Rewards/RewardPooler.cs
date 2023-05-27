using System.Collections.Generic;
using UnityEngine;
using Zenject;

public sealed class RewardPooler : MonoBehaviour
{
    public static RewardPooler Instance;

    public List<GameObject> rewardsPrefabs = new List<GameObject>();
    public List<GameObject> rewards;
    public int amountToPoolEach;
    private GameObject parentObj;

    public ResourceConfig _resourceConfig;

    [Inject]
    public void Constructor(ResourceConfig resourceConfig)
    {
        _resourceConfig = resourceConfig;
    }
    
    private void Awake()
    {
        Instance = this;
        
        rewards = new List<GameObject>();

        parentObj = new GameObject();
        parentObj.name = "RewardPool";

        foreach (var rewardPrefab in rewardsPrefabs)
        {
            for (int i = 0; i < amountToPoolEach; i++)
            {
                var obj = Instantiate(rewardPrefab, parentObj.transform);
                obj.name = obj.name + "[" + i + "]";
                obj.SetActive(false);
                obj.GetComponent<IReward>().Constructor(_resourceConfig);
                rewards.Add(obj);
            }
        }
    }

    public void SpawnReward(Vector3 position)
    {
        foreach (var reward in rewards)
        {
            if (!reward.activeInHierarchy)
            {
                reward.SetActive(true);
                reward.transform.position = position;
                return;
            }
        }
    }
}
