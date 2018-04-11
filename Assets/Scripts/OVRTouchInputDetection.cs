using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ControllerSelection
{
    public class OVRTouchInputDetection : MonoBehaviour
    {
        [System.Serializable]
        public class HoverCallback : UnityEvent<Transform> { }
        [System.Serializable]
        public class SelectionCallback : UnityEvent<Transform> { }

        [Header("(Optional) Tracking space")]
        [Tooltip("Tracking space of the OVRCameraRig.\nIf tracking space is not set, the scene will be searched.\nThis search is expensive.")]
        public Transform trackingSpace = null;


        [Header("Selection")]
        [Tooltip("Primary selection button")]
        public OVRInput.Button primaryButton = OVRInput.Button.PrimaryIndexTrigger;
        [Tooltip("Layers to exclude from raycast")]
        public LayerMask excludeLayers;
        [Tooltip("Maximum raycast distance")]
        public float raycastDistance = 500;

        [Header("Hover Callbacks")]
        public OVRTouchInputDetection.HoverCallback onHoverEnter;
        public OVRTouchInputDetection.HoverCallback onHoverExit;
        public OVRTouchInputDetection.HoverCallback onHover;

        [Header("Selection Callbacks")]
        public OVRTouchInputDetection.SelectionCallback onSelect;
        public OVRTouchInputDetection.SelectionCallback onDeselect;

        //protected Ray pointer;
        protected Transform lastHit = null;
        protected Transform triggerDown = null;

        [HideInInspector]
        public OVRInput.Controller activeController = OVRInput.Controller.None;

        void Awake()
        {
            if (trackingSpace == null)
            {
                Debug.LogWarning("OVRRawRaycaster did not have a tracking space set. Looking for one");
                trackingSpace = OVRInputHelpers.FindTrackingSpace();
            }
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (trackingSpace == null)
            {
                Debug.LogWarning("OVRRawRaycaster did not have a tracking space set. Looking for one");
                trackingSpace = OVRInputHelpers.FindTrackingSpace();
            }
        }

        void Update()
        {
            activeController = OVRInputHelpers.GetControllerForButton(OVRInput.Button.PrimaryIndexTrigger, activeController);
            Ray pointer = OVRInputHelpers.GetSelectionRay(activeController, trackingSpace);

            RaycastHit hit; // Was anything hit?
            if (Physics.Raycast(pointer, out hit, raycastDistance, ~excludeLayers))
            {
                if (lastHit != null && lastHit != hit.transform) // if we hit a different object, exit hover for old object
                {
                    if (onHoverExit != null)
                    {
                        onHoverExit.Invoke(lastHit);
                    }
                    lastHit = null;
                }

                if (lastHit == null) // we hit something, enter hover
                {
                    if (onHoverEnter != null)
                    {
                        onHoverEnter.Invoke(hit.transform);
                    }
                }

                if (onHover != null) // hover (not enter hover, rather the callback called every time while hovering)
                {
                    onHover.Invoke(hit.transform);
                }

                lastHit = hit.transform;

                
                if (activeController != OVRInput.Controller.None) // Handle selection callbacks
                {
                    if (OVRInput.GetDown(primaryButton, activeController))
                    {
                        triggerDown = lastHit;

                        if (onSelect != null)
                        {
                            onSelect.Invoke(triggerDown);
                        }
                    }

                }
            }
            
            else if (lastHit != null) // Nothing was hit, handle exit callback
            {
                if (onHoverExit != null)
                {
                    onHoverExit.Invoke(lastHit);
                }
                lastHit = null;
            }
            
            if (OVRInput.GetUp(primaryButton, activeController)) // handle deselect
            {

                    if (onDeselect != null && triggerDown != null)
                    {
                        onDeselect.Invoke(triggerDown);
                        triggerDown = null;
                    }
                
            }
        }
    }
}