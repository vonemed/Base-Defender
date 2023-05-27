using System.Collections.Generic;
using UnityEngine;

public class VisualEffectsPooler : MonoBehaviour
{
    public static VisualEffectsPooler Instance;

    public List<GameObject> visualEffectsPrefabs;
    public List<GameObject> visualEffects;
    public int amountToPoolEach;
    private GameObject parentObj;
    private void Awake()
    {
        Instance = this;
        
        visualEffectsPrefabs = new List<GameObject>();

        parentObj = new GameObject();
        parentObj.name = "VisualEffectsPool";

        foreach (var visualEffectsPrefab in visualEffectsPrefabs)
        {
            for (int i = 0; i < amountToPoolEach; i++)
            {
                var obj = Instantiate(visualEffectsPrefab, parentObj.transform);
                obj.name = obj.name + "[" + i + "]";
                obj.SetActive(false);
                visualEffects.Add(obj);
            }
        }
    }

    public void CheckIfEffectsStopped()
    {
        foreach (var visualEffect in visualEffects)
        {
            if (visualEffect.GetComponent<ParticleSystem>().isStopped)
            {
                visualEffect.gameObject.SetActive(false);
            }
        }
    }
    

    //TODO: separate by type later
    public void SpawnVisualEffect(Vector3 position)
    {
        CheckIfEffectsStopped();
        
        foreach (var visualEffect in visualEffects)
        {
            if (!visualEffect.activeInHierarchy)
            {
                visualEffect.SetActive(true);
                visualEffect.transform.position = position;
                //Trash, for test
                visualEffect.GetComponent<ParticleSystem>().Play();
                return;
            }
        }
    }
}
