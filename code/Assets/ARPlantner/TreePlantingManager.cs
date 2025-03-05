using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhacedTouch = UnityEngine.InputSystem.EnhancedTouch;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation.Samples;
using TMPro;

public class TreePlantingManager : MonoBehaviour
{
    public ARRaycastHitEventAsset m_RaycastHitEvent;

    public GameObject[] treePrefabs;
    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;

    public GameObject[] activateableTrees;

    public GameObject draftTreesFolder;
    public GameObject placedTreesFolder;

    public Camera arCamera;
    public TMP_InputField txt_JSON;

    private LineRenderer cameraLineRenderer;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public List<PlacedTreeSerializeable> placedTreeSerializeables = new List<PlacedTreeSerializeable>();


    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
    }

    private void onEnable()
    {
        EnhacedTouch.TouchSimulation.Enable();
        EnhacedTouch.EnhancedTouchSupport.Enable();
        EnhacedTouch.Touch.onFingerDown += OnFingerDown;

        m_RaycastHitEvent.eventRaised += PlaceTreeAtARHitLocation;
    }

    private void onDisable()
    {
        EnhacedTouch.TouchSimulation.Disable();
        EnhacedTouch.EnhancedTouchSupport.Disable();
        EnhacedTouch.Touch.onFingerDown -= OnFingerDown;

        m_RaycastHitEvent.eventRaised -= PlaceTreeAtARHitLocation;
    }

    private void OnFingerDown(EnhacedTouch.Finger finger)
    {
        if (finger.index != 0) { return; }

        if (raycastManager.Raycast(finger.screenPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            Instantiate(treePrefabs[0], hitPose.position, hitPose.rotation);
        }
    }

    public void PlaceTree(int treeId)
    {
        RaycastHit hitInfo;
        Physics.Raycast(arCamera.transform.position, arCamera.transform.forward, out hitInfo);

        if (hitInfo.transform != null)
        {
            Instantiate(treePrefabs[treeId], hitInfo.transform.position, Quaternion.identity);
        }
    }

    public void OpenCloseDraft()
    {
        DeleteAllPlacedTrees();
        draftTreesFolder.SetActive(!draftTreesFolder.activeSelf);
    }

    private int activeTrees = 0;
    public void ActivateTree()
    {
        if (activeTrees >= activateableTrees.Length)
        {
            for (int i = 0; i < activateableTrees.Length; i++)
            {
                activateableTrees[i].SetActive(false);
            }
            activeTrees = 0;
        }

        activateableTrees[activeTrees].SetActive(true);
        activeTrees++;
    }


    GameObject m_SpawnedObject;
    public float preFabScalingFactor = .5f;

    public void PlaceTreeAtARHitLocation(object sender, ARRaycastHit hit)
    {
        Debug.Log("Klicked somewhere. Placing Tree Nr. " + TreeSelectionUiManager.selectedTree);


        m_SpawnedObject = Instantiate(treePrefabs[TreeSelectionUiManager.selectedTree], hit.trackable.transform.parent);

        var forward = hit.pose.rotation * Vector3.up;
        var offset = forward * preFabScalingFactor;
        m_SpawnedObject.transform.position = hit.pose.position + offset;
        m_SpawnedObject.transform.parent = hit.trackable.transform.parent;
    }

    public void PlaceTreeAtARHitLocation(Transform position)
    {
        Debug.Log("Klicked somewhere. Placing Tree Nr. " + TreeSelectionUiManager.selectedTree);

        m_SpawnedObject = Instantiate(treePrefabs[TreeSelectionUiManager.selectedTree], position.parent);
        

        var forward = position.rotation * Vector3.up;
        var offset = forward * preFabScalingFactor;
        m_SpawnedObject.transform.position = position.position; // + offset;
        m_SpawnedObject.transform.localScale = m_SpawnedObject.transform.localScale * preFabScalingFactor;
        m_SpawnedObject.transform.parent = placedTreesFolder.transform;

        placedTreeSerializeables.Add(new PlacedTreeSerializeable
        {
            position = m_SpawnedObject.transform.position,
            treeId = TreeSelectionUiManager.selectedTree
        }) ;
    }

    public void DeleteAllPlacedTrees()
    {
        var trees = placedTreesFolder.transform.GetComponentsInChildren<Transform>();
        foreach (var tree in trees)
        {
            Destroy(tree.gameObject);
        }
        placedTreeSerializeables.Clear();
    }

    public void GenerateJSON()
    {
        Debug.Log("Generating JSON");

        // Verpacke die Liste in die Wrapper-Klasse
        PlacedTreeList wrappedList = new PlacedTreeList(placedTreeSerializeables);

        // Konvertiere sie in JSON
        string json = JsonUtility.ToJson(wrappedList, true); // true = schön formatiertes JSON

        Debug.Log(json);
        txt_JSON.text = json;
    }

    private void Update()
    {
        // cameraLineRenderer.startWidth = .1f;
        // cameraLineRenderer.endWidth = .1f;

        // cameraLineRenderer.SetPositions(new Vector3[] { arCamera.transform.position, arCamera.transform.position + arCamera.transform.forward * 20 });
    }

    [System.Serializable]
    public class PlacedTreeSerializeable
    {
        public Vector3 position;
        public int treeId;
    }

    [System.Serializable]
    public class PlacedTreeList
    {
        public List<PlacedTreeSerializeable> trees;

        public PlacedTreeList(List<PlacedTreeSerializeable> trees)
        {
            this.trees = trees;
        }
    }
}
