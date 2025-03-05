using UnityEngine;
using UnityEngine.EventSystems;

public class ArPlacedCubeTreeSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Debug.Log("An Object was placed");

        // Prüfen, ob der Klick über einem UI-Element ist
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("UI wurde geklickt, kein Baum wird gespawnt.");
            return;
        }

        FindAnyObjectByType<TreePlantingManager>().PlaceTreeAtARHitLocation(this.transform);
        
        Destroy(gameObject, .1f);
    }


}
