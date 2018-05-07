using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ControllerSelection
{
    public class OVRTouchInputDetectionTwoHands : MonoBehaviour
    {
        [System.Serializable]
        public class HoverCallback : UnityEvent<Transform, bool> { }
        [System.Serializable]
        public class SelectionCallback : UnityEvent<Transform, bool> { }

        [Header("(Optional) Tracking space")]
        [Tooltip("Tracking space of the OVRCameraRig.\nIf tracking space is not set, the scene will be searched.\nThis search is expensive.")]
        public Transform trackingSpace = null;

        public GameObject Visualizer;
        OVRPointerVisualizerTwoHands VisualizerScript;


        [Header("Selection")]
        [Tooltip("Primary selection button")]
        public OVRInput.Button primaryButton = OVRInput.Button.PrimaryIndexTrigger;
        [Tooltip("Layers to exclude from raycast")]
        public LayerMask excludeLayers;
        [Tooltip("Maximum raycast distance")]
        public float raycastDistance = 500;

        [Header("Hover Callbacks")]
        public OVRTouchInputDetectionTwoHands.HoverCallback onHoverEnter;
        public OVRTouchInputDetectionTwoHands.HoverCallback onHoverExit;
        public OVRTouchInputDetectionTwoHands.HoverCallback onHover;

        [Header("Selection Callbacks")]
        public OVRTouchInputDetectionTwoHands.SelectionCallback onSelect;
        public OVRTouchInputDetectionTwoHands.SelectionCallback onDeselect;

        //protected Ray pointer;
        protected Transform lastHit = null;
        //protected Transform triggerDown = null;
        protected Transform leftSelectedTransform = null;
        protected Transform rightSelectedTransform = null;

        private bool leftPress;
        private bool rightPress;

        [HideInInspector]
        public OVRInput.Controller activeController = OVRInput.Controller.None;

        private void Start()
        {

            VisualizerScript = Visualizer.GetComponent<OVRPointerVisualizerTwoHands>();

        }

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

        bool isLeftController(OVRInput.Controller controller)
        {
            return controller == OVRInput.Controller.LTouch || controller == OVRInput.Controller.LTrackedRemote;
        }

        void Update()
        {
            activeController = OVRInputHelpers.GetControllerForButton(OVRInput.Button.PrimaryIndexTrigger, activeController);
            if (leftPress) activeController = OVRInput.Controller.RTouch;
            if (rightPress) activeController = OVRInput.Controller.LTouch;
            VisualizerScript.setController(activeController);
            Debug.Log("active controller " + activeController);
            Debug.Log("lpress " + leftPress + ", rightPres " + rightPress);
           

            Ray pointer = OVRInputHelpers.GetSelectionRay(activeController, trackingSpace);

            RaycastHit hit; // Was anything hit?
            if (Physics.Raycast(pointer, out hit, raycastDistance, ~excludeLayers))
            {
                if (lastHit != null && lastHit != hit.transform) // if we hit a different object, exit hover for old object
                {
                    if (onHoverExit != null)
                    {
                        onHoverExit.Invoke(lastHit, isLeftController(activeController));
                    }
                    lastHit = null;
                }

                if (lastHit == null) // we hit something, enter hover
                {
                    if (onHoverEnter != null)
                    {
                        onHoverEnter.Invoke(hit.transform, isLeftController(activeController));
                    }
                }

                if (onHover != null) // hover (not enter hover, rather the callback called every time while hovering)
                {
                    onHover.Invoke(hit.transform, isLeftController(activeController));
                }

                lastHit = hit.transform;

                
                if (activeController != OVRInput.Controller.None) // Handle selection callbacks
                {
                    if (OVRInput.GetDown(primaryButton, activeController))
                    {
                     
                        if (isLeftController(activeController))
                        {
                            leftPress = true;
                            leftSelectedTransform = lastHit;
                        }
                        else
                        {
                            rightPress = true;
                            rightSelectedTransform = lastHit;
                        }

                        if (onSelect != null && lastHit != null)
                        {
                            onSelect.Invoke(lastHit, isLeftController(activeController));

                        }
                    }

                }
            }
            
            else if (lastHit != null) // Nothing was hit, handle exit callback
            {
                if (onHoverExit != null)
                {
                    onHoverExit.Invoke(lastHit, isLeftController(activeController));
                }
                lastHit = null;
            }
            
            if (OVRInput.GetUp(primaryButton, OVRInput.Controller.LTouch)) // handle deselect
            {

                    if (onDeselect != null && leftSelectedTransform != null)
                    {
                        onDeselect.Invoke(leftSelectedTransform, true);
                        leftSelectedTransform = null;
                        leftPress = false;
                    }              
            }
            if (OVRInput.GetUp(primaryButton, OVRInput.Controller.RTouch)) // handle deselect
            {

                if (onDeselect != null && rightSelectedTransform != null)
                {
                    onDeselect.Invoke(rightSelectedTransform, false);
                    rightSelectedTransform = null;
                    rightPress = false;
                }
            }
        }
    }
}