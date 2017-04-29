// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using UnityEngine.VR.WSA.Input;
using HoloToolkit.Unity.SpatialMapping;

namespace HoloToolkit.Examples.SpatialMappingComponent
{
    /// <summary>
    /// Simple test script for dropping cubes with physics to observe interactions
    /// </summary>
    public class TapOnMesh : MonoBehaviour
    {
        GestureRecognizer recognizer;

        private void Start()
        {
            recognizer = new GestureRecognizer();
            recognizer.SetRecognizableGestures(GestureSettings.Tap);
            recognizer.TappedEvent += Recognizer_TappedEvent;
            recognizer.StartCapturingGestures();
        }

        private void OnDestroy()
        {
            recognizer.TappedEvent -= Recognizer_TappedEvent;
        }

        private void Recognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
        {
            /*
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube); // Create a cube
            cube.transform.localScale = Vector3.one * 0.3f; // Make the cube smaller
            cube.transform.position = Camera.main.transform.position + Camera.main.transform.forward; // Start to drop it in front of the camera
            cube.AddComponent<Rigidbody>(); // Apply physics
            */

            float MaxGazeDistance = 5.0f;
            LayerMask RaycastLayerMask = Physics.DefaultRaycastLayers;

            Vector3 gazeOrigin = Camera.main.transform.position;
            Vector3 gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;

            bool Hit = Physics.Raycast(gazeOrigin,
               gazeDirection,
               out hitInfo,
               MaxGazeDistance,
               RaycastLayerMask);

            RaycastHit HitInfo = hitInfo;

            if (Hit)
            {
                Debug.Log(HitInfo.transform.gameObject.name);
                if (!SpatialMappingManager.Instance.materialsChangeColor.ContainsKey(HitInfo.transform.gameObject.name))
                {
                    SpatialMappingManager.Instance.materialsChangeColor.Add(HitInfo.transform.gameObject.name, true);
                }
                // HitInfo.transform.gameObject.GetComponent<Renderer>().material = SpatialMappingManager.Instance.NewMaterial;
            }
        }
    }
}