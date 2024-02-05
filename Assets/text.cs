using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class text : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pickupTxt;
    [SerializeField] private TextMeshProUGUI dropTxt;
   
    public float itemPickupDistance;

    


    void Start()
    {
        
    pickupTxt.gameObject.SetActive(false);
        dropTxt.gameObject.SetActive(false);
    }

    void Update()
    {
        Movement script1Reference1 = FindObjectOfType<Movement>();
        Transform camera = script1Reference1?.playerCamera;
        RaycastHit hit;
        bool cast = Physics.Raycast(camera.position, camera.forward, out hit, itemPickupDistance);

        Movement script1Reference = FindObjectOfType<Movement>();
        Transform attached = script1Reference?.attachedObject;

        // Show pickup text when looking at object with "pickup" tag and nothing is attached
        if (cast && hit.transform.CompareTag("pickup") && attached == null)
        {
            pickupTxt.gameObject.SetActive(true);
            dropTxt.gameObject.SetActive(false);
        }
        else
        {
            pickupTxt.gameObject.SetActive(false);
        }

        // Show drop text when something is attached
        if (attached != null)
        {
            dropTxt.gameObject.SetActive(true);
        }
        else
        {
            dropTxt.gameObject.SetActive(false);
        }
    }
}

