
using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    private Dictionary<string, int> resources = new Dictionary<string, int>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAllResources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadAllResources()
    {
        ResourceData[] allResources = Resources.LoadAll<ResourceData>("");

        foreach (ResourceData resource in allResources)
        {
            // Cargar datos guardados o usar valor por defecto
            resources[resource.resourceID] = PlayerPrefs.GetInt(resource.resourceID, resource.defaultAmount);
        }
    }

    public void AddResource(string resourceID, int amount)
    {
        if (resources.ContainsKey(resourceID))
        {
            resources[resourceID] += amount;
            SaveResource(resourceID);
        }
    }

    public int GetResourceAmount(string resourceID)
    {
        return resources.ContainsKey(resourceID) ? resources[resourceID] : 0;
    }

    private void SaveResource(string resourceID)
    {
        PlayerPrefs.SetInt(resourceID, resources[resourceID]);
        PlayerPrefs.Save();  // Importante en móvil para guardar inmediatamente
    }

    // Para limpiar datos (opcional)
    public void ResetAllResources()
    {
        PlayerPrefs.DeleteAll();
        LoadAllResources();
    }
}