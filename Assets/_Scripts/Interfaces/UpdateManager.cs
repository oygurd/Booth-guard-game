using System;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    private static UpdateManager _instance;
    public static UpdateManager Instance => _instance;

    private readonly List<IUpdateable> updateables = new List<IUpdateable>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Register(IUpdateable updateable)
    {
        if (!updateables.Contains(updateable))
        {
            updateables.Add(updateable);
        }
    }

    public void unregister(IUpdateable updateable)
    {
        updateables.Remove(updateable);
    }

// Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;

        foreach (var updateable in updateables)
        {
            updateable.CustomUpdate(deltaTime);
        }
    }
}