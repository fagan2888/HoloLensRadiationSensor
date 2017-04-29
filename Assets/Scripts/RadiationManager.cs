using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.VR.WSA;
using HoloToolkit.Unity.SpatialMapping;

public class RadiationManager : SpatialMappingSource {

    private SpatialMappingObserver spatialMappingObserver;
    protected virtual Material CustomMaterial { get { return SpatialMappingManager.Instance.NewMaterial; } }
    private Dictionary<string, bool> materialDone = new Dictionary<string, bool>();
    public Material testMaterial;

    private void Start()
    {
        spatialMappingObserver = GameObject.Find("SpatialMapping").GetComponent<SpatialMappingObserver>();
    }

    private void Update()
    {
        foreach (KeyValuePair<string, bool> entry in SpatialMappingManager.Instance.materialsChangeColor)
        {
            GameObject gameObjectToChangeColor = GameObject.Find(entry.Key);
            if (materialDone.ContainsKey(entry.Key))
            {
                continue;
            }
            if( gameObjectToChangeColor != null)
            {
                Debug.Log("FOUND");
                Debug.Log(entry.Key);
                Renderer renderer = gameObjectToChangeColor.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.sharedMaterial = testMaterial;
                }
                materialDone.Add(entry.Key, true);
            }
        }
    }

    public void GetSpatialMesh()
    {
        /*
        ReadOnlyCollection<SpatialMappingSource.SurfaceObject> surfaceObjects = SpatialMappingManager.Instance.GetSurfaceObjects();
        Debug.Log(SpatialMappingManager.Instance.GetSurfaceObjects()[surfaceObjects.Count - 1].Object.name);
        */
        Debug.Log("TRIGGERED");

        if (spatialMappingObserver.surfaceWorkQueue.Count > 0)
        {
            Debug.Log("At least 1 work queue");

            SurfaceId surfaceID = spatialMappingObserver.surfaceWorkQueue.Dequeue();

            string surfaceName = ("Surface-" + surfaceID.handle);

            SurfaceObject newSurface;
            WorldAnchor worldAnchor;

            if (spatialMappingObserver.spareSurfaceObject == null)
            {
                Debug.Log("NEW SURFACE");
                newSurface = CreateSurfaceObject(
                    mesh: null,
                    objectName: surfaceName,
                    parentObject: transform,
                    meshID: surfaceID.handle,
                    drawVisualMeshesOverride: true
                );
                UpdateOrAddSurfaceObject(newSurface);
                worldAnchor = newSurface.Object.AddComponent<WorldAnchor>();
            }
            else
            {
                Debug.Log("EXISTING SURFACE");
                newSurface = spatialMappingObserver.spareSurfaceObject.Value;
                spatialMappingObserver.spareSurfaceObject = null;

                Debug.Assert(!newSurface.Object.activeSelf);
                newSurface.Object.SetActive(true);

                Debug.Assert(newSurface.Filter.sharedMesh == null);
                Debug.Assert(newSurface.Collider.sharedMesh == null);
                newSurface.Object.name = surfaceName;
                Debug.Assert(newSurface.Object.transform.parent == transform);
                newSurface.ID = surfaceID.handle;
                newSurface.Renderer.sharedMaterial = CustomMaterial;
                // newSurface.Renderer.enabled = false;
                newSurface.Renderer.enabled = true;

                worldAnchor = newSurface.Object.GetComponent<WorldAnchor>();
                Debug.Assert(worldAnchor != null);
            }
        } else
        {
            Debug.Log("No work queue");
        }
    }

    public void DistanceFromCamera()
    {
        /*
        GameObject player = GameObject.Find("Player(Clone)");
        GameObject spatialMapping = GameObject.Find("SpatialMapping");
        for (int i = 0; i < spatialMapping.transform.childCount; i++)
        {
            Transform childMesh = spatialMapping.transform.GetChild(i);
            childMesh.TransformPoint(Vector3.zero);
            Vector3 distance = childMesh.position - player.transform.position;
            Debug.Log(distance);
        }
        */
    }

    public void RandomTests()
    {
        Debug.Log(SpatialMappingManager.Instance.GetMeshFilters()[0]);
    }
}
