using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class Looting : MonoBehaviour
{
    public GameObject loot_UI;
    public GameObject rTUI;
    public LayerMask objectMask;
    public Camera mCamera;
    public Canvas canvas;
    IdSystem idSystem;
    public Inventory inventory;
    GameObject tempGm;
    GameObject tempUI;
    RaycastHit hit1;

    List<GameObject> itemsInCont = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        idSystem = GameObject.FindGameObjectsWithTag("Player").First(a => a.gameObject).GetComponent<IdSystem>();
        tempGm = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        BoxFinder();
    }
    void BoxFinder()
    {
        if (Physics.SphereCast(mCamera.transform.position, 1, mCamera.transform.forward, out hit1, 3, objectMask, QueryTriggerInteraction.Collide))
        {
            if (tempGm != hit1.transform.gameObject)
            {   
                if (tempGm != null)
                {
                    Destroy(tempUI);
                    tempUI = null;
                }
                tempGm = hit1.transform.gameObject;
                tempUI = Instantiate<GameObject>(rTUI, hit1.point, Quaternion.identity, canvas.transform);
            }
        }
        else
        {
            tempGm = this.gameObject;
            Destroy(tempUI);
            tempUI = null;
            for (int i = 0; i < itemsInCont.Count; i++)
            {
                Destroy(itemsInCont[i]);
            }
            itemsInCont.Clear();
        }

        if (tempUI != null && tempGm != this.gameObject) 
        { 
            Tracker();
            Taking();
        }
    }
    void Taking() 
    {
        for (int i = 0; i < itemsInCont.Count; i++)
        {
            Destroy(itemsInCont[i]);
        }
        itemsInCont.Clear();
        
        if(Input.GetKeyDown(KeyCode.E) && 0 < tempGm.GetComponent<Container>().items.Count)
        {
            if (inventory.AddItem(tempGm.GetComponent<Container>().items[0]))
                tempGm.GetComponent<Container>().items.Remove(tempGm.GetComponent<Container>().items[0]);   
        }
        Container container = tempGm.GetComponent<Container>();
        List<Item> tempItems = container.items;
        for (int i = 0; i < tempItems.Count; i++)
        {
            Vector2 pos = new Vector2(loot_UI.GetComponent<RectTransform>().position.x, loot_UI.GetComponent<RectTransform>().position.y  -50*i) + new Vector2(tempUI.transform.position.x, tempUI.transform.position.y);
            itemsInCont.Add(Instantiate<GameObject>(loot_UI, pos, Quaternion.identity, canvas.transform));
            var images = itemsInCont[i].GetComponentsInChildren<Image>();
            Image img = (Image)images.Where(a => a.GetComponentInChildren<Image>() != itemsInCont[i].GetComponent<Image>()).FirstOrDefault();
            img.sprite = tempItems[i].sprite;
            itemsInCont[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = tempItems[i].title;
        }
    }
    void Tracker()
    {
        Vector3 targPos = tempGm.transform.position;
        Vector3 camForward = mCamera.transform.forward;
        Vector3 camPos = mCamera.transform.position + camForward;
        float distInFrontOfCamera = Vector3.Dot(targPos - camPos, camForward);
        if (distInFrontOfCamera < 0f)
        {
            targPos -= camForward * distInFrontOfCamera;
        }
        Vector2 pos = RectTransformUtility.WorldToScreenPoint(mCamera, targPos);
        tempUI.transform.position = pos;
    }
}
