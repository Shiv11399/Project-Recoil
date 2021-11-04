using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAndThrow : MonoBehaviour
{
    Camera playerCamera;
    public Material highlitedMaterial;
    public Material defaultMaterial;
    public GameObject objectHolder;
    GameObject player;
    private string selectableTag = "Throwable";
    bool cursor = false;
    private Transform _selection;
    private bool carryObject;
    
    
    // Update is called once per frame
    void Update()
    {
        if (_selection != null)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
            //_selection = null;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && cursor == true)//unlock the cursor
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursor = false;
        }
        if (playerCamera == null)
        {
            objectHolder = GameObject.FindGameObjectWithTag("WeaponHolder");
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
                return;
            playerCamera = player.GetComponentInChildren<Camera>();

        }
        var ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit))
        {
            var selection = hit.transform;
            if (selection.CompareTag(selectableTag))
            {
                var selectionRenderer = selection.GetComponent<Renderer>();
                if (selectionRenderer != null)
                {
                    selectionRenderer.material = highlitedMaterial;
                    if (Input.GetKeyDown(KeyCode.E) && carryObject == false)
                    {
                        carryObject = true;
                        selection.SetParent(objectHolder.transform);
                        selection.position = objectHolder.transform.position;
                        selection.rotation = objectHolder.transform.rotation;
                        selection.GetComponent<Rigidbody>().isKinematic = true;
                        selection.GetComponent<Rigidbody>().useGravity = false;
                        //selection.GetComponent<Collider>().enabled = false;
                        return;

                    }
                }
                _selection = selection;
            }

        }
        if (Input.GetKeyDown(KeyCode.E) && carryObject == true)
        {
            carryObject = false;

            objectHolder.transform.DetachChildren();
            _selection.GetComponent<Rigidbody>().isKinematic = false;
            _selection.GetComponent<Rigidbody>().useGravity = true;
            _selection.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 1000f);
            _selection = null;

        }
    }
}
