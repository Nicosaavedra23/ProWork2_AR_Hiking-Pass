using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class TestScript : MonoBehaviour
{
    [SerializeField] ARTrackedImageManager m_TrackedImageManager;
    [SerializeField] private GameObject[] placeablePrefabs; //our Prefabs
    private Dictionary<string, GameObject> m_spawnedPrefabs = new Dictionary<string, GameObject>(); //will call sporned prefabs out of Prefab Array

    void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

    void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;



    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            m_spawnedPrefabs[trackedImage.name].SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name; //temporarily store the name of the tracked image
        Vector3 position = trackedImage.transform.position;

        GameObject prefab = m_spawnedPrefabs[name]; //GameObject will be from our spawnedPrefabs Dictionary selected by the name
        prefab.transform.position = position;
        prefab.SetActive(true); //see the prefab linked to the current image

        foreach (GameObject go in m_spawnedPrefabs.Values)
        {
            if (go.name != name)
            {
                go.SetActive(
                    false); //ensure that all of the other prefabs we have activated go back to be hidden if we look at a new image
            }
        }


    }
}
