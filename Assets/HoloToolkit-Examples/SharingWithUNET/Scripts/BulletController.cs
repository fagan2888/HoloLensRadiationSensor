// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using HoloToolkit.Unity.SpatialMapping;

namespace HoloToolkit.Examples.SharingWithUNET
{
    /// <summary>
    /// Controls little bullets fired into the world.
    /// </summary>
    public class BulletController : MonoBehaviour
    {
        private void Start()
        {
            // The bullet's transform should be in local space to the Shared Anchor.
            // Make the shared anchor the parent, but we don't want the transform to try
            // to 'preserve' the position, so we set false in SetParent.
            transform.SetParent(SharedCollection.Instance.transform, false);

            // The rigid body has a velocity that needs to be transformed into 
            // the shared coordinate system.
            Rigidbody rb = GetComponentInChildren<Rigidbody>();
            rb.velocity = transform.parent.TransformDirection(rb.velocity);

            // Start
            float MaxGazeDistance = 5.0f;
            LayerMask RaycastLayerMask = Physics.DefaultRaycastLayers;

            GameObject serverPlayer = GameObject.Find("Player(Clone)");
            Vector3 gazeOrigin = serverPlayer.transform.position;
            Vector3 gazeDirection = serverPlayer.transform.forward;

            RaycastHit hitInfo;

            bool Hit = Physics.Raycast(gazeOrigin,
               gazeDirection,
               out hitInfo,
               MaxGazeDistance,
               RaycastLayerMask);

            RaycastHit HitInfo = hitInfo;

            if (Hit)
            {
                Debug.Log("Hit");
                Debug.Log(HitInfo.transform.gameObject.name);
                if (!SpatialMappingManager.Instance.materialsChangeColor.ContainsKey(HitInfo.transform.gameObject.name))
                {
                    SpatialMappingManager.Instance.materialsChangeColor.Add(HitInfo.transform.gameObject.name, true);
                    Debug.Log(HitInfo.transform.gameObject.name);
                }
                // HitInfo.transform.gameObject.GetComponent<Renderer>().material = SpatialMappingManager.Instance.NewMaterial;
            }
        }
    }
}
