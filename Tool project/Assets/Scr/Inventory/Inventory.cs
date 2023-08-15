using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] Camera mCamera;
    IdSystem idSystem;

    [SerializeField] GameObject slot, hand;
    [SerializeField] int numOfSlot;

    GameObject inHand;
    public List<Item> inv = new List<Item>();
    List<GameObject> slots = new List<GameObject>();
    int activeSlot = 0;
    Item nullItem = new Item();

    bool justStarted = false; // for a bug wich I don't know how to fix(
    void Start()
    {
        idSystem = GameObject.FindGameObjectsWithTag("Player").First(a => a.gameObject).GetComponent<IdSystem>();

        Image tempIMG = slot.GetComponent<Image>();
        Color newColor = tempIMG.color;
        newColor.a = 0.39f;
        tempIMG.color = newColor;

        inv.Clear();
        slot.GetComponent<RectTransform>().position = new Vector2((numOfSlot - 1) / 2 * 60 + 580, 45); 
        for (int i = 0; i < numOfSlot; i++)
        {
            inv.Add(nullItem);
            Vector2 pos = new Vector2(slot.GetComponent<RectTransform>().position.x - 60 * i, 45);
            slots.Add(Instantiate<GameObject>(slot, pos, Quaternion.identity, canvas.transform)); //creating slots and puting tham in "slots" List
        }
        justStarted = true;
    }

    private void Update()
    {
        if (inHand != null)
            inHand.transform.rotation = hand.transform.rotation;

        if (justStarted)//if you want to add some items at the start
        {
            justStarted = false;
            inv[0] = idSystem.ItemById(1);
            inv[0].amount = 9;

            ActiveSlot(activeSlot);
            VisualizeInv();
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // changing slots
            ActiveSlot(1);
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) 
            ActiveSlot(-1);
        if(Input.GetKeyDown(KeyCode.F)&& inv[activeSlot].prefab != null) // throwing away object in activeSlot
        {
            GameObject tempGO = Instantiate<GameObject>(inv[activeSlot].prefab);
            tempGO.AddComponent<BoxCollider>();
            tempGO.AddComponent<Rigidbody>();
            Vector3 vector = mCamera.transform.forward * 2;
            Vector3 vec = mCamera.transform.position;
            RaycastHit hit;
            Physics.Raycast(vec, mCamera.transform.forward, out hit, Mathf.Infinity, ~(1 << 30), QueryTriggerInteraction.Collide);
            if (hit.distance > vector.magnitude || hit.distance == Mathf.Infinity || hit.distance == 0)
                tempGO.transform.position = transform.position + mCamera.transform.forward * 2;
            else tempGO.transform.position = hit.point;
            Container tempContainer = tempGO.AddComponent<Container>();
            Item tempItem = new Item(idSystem.ItemById(inv[activeSlot].id));
            if (Input.GetKey(KeyCode.LeftShift))
            {
                tempItem.amount = inv[activeSlot].amount;
                inv[activeSlot].amount = 0;
            }
            else
            {
                inv[activeSlot].amount = inv[activeSlot].amount - 1;
                tempItem.amount = 1;
            }
            tempContainer.items.Add(tempItem);
            tempContainer.destroyOnPickUp = true;
            tempGO.layer = 17; // same as on the Container
            VisualizeInv();
            ActiveSlot(-1);
            ActiveSlot(1);
        }
    }
    void VisualizeInv() //Visualizing inventory item in slot (amount/sprite)
    {
        for (int i = 0; i < numOfSlot; i++)
        {
            if (inv[i].id != 0)
            {
                if (inv[i].amount == 0)
                {
                    inv[i] = nullItem;
                    slots[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = " ";
                    slots[i].GetComponent<Image>().sprite = null;
                }
                else
                {
                    slots[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = inv[i].amount.ToString();
                    slots[i].GetComponent<Image>().sprite = inv[i].sprite;
                }       
            }
        }
    }
    void ActiveSlot(int changeSlot) // changeing ActiveSlot
    {
        Image tempIMG = slots[activeSlot].GetComponent<Image>();
        Color newColor = tempIMG.color;
        newColor.a = 0.39f;
        tempIMG.color = newColor;
        activeSlot += changeSlot;
        if (activeSlot >= numOfSlot)
            activeSlot = 0;
        if (activeSlot < 0)
            activeSlot = numOfSlot - 1;
        tempIMG = slots[activeSlot].GetComponent<Image>();
        newColor = tempIMG.color;
        newColor.a = 0.58f;
        tempIMG.color = newColor;
        if(inHand!=null)
            Destroy(inHand);
        if (inv[activeSlot].inHandabel == 1&& inv[activeSlot].prefab != null)
            inHand = Instantiate<GameObject>(inv[activeSlot].prefab, hand.transform.position, hand.transform.rotation, hand.transform);
        VisualizeInv();
    }
    public bool AddItem(Item item)
    {
        for (int i = 0; i < numOfSlot; i++)
        {
            if (inv.ElementAt(i).id == item.id)
            {
                inv[i].amount += item.amount;
                VisualizeInv();
                return true;
            }
        }
            for (int i = 0; i < numOfSlot; i++)
            {
                if (inv.ElementAt(i) == nullItem)
                {
                    inv[i] = item;
                    VisualizeInv();
                    return true;
                }
            }
        return false;
    }

    //public void RemoveItem(Item item) { for (int i = 0; i < numOfSlot; i++) { inv[i] = (inv.ElementAt(i) == item) ? (inv[i] = (inv[i].amount <= 1) ? nullItem : new Item(inv[i].id, inv[i].amount - 1)) : inv[i]; } VisualizeInv(); }
}