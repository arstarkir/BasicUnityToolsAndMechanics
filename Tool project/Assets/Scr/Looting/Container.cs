using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;


public class Container : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    IdSystem idSystem;
    public bool isRandom = false;
    public bool destroyOnPickUp = false;
    bool wasNotEmpty = false;
    private void Start()
    {
        idSystem = GameObject.FindGameObjectsWithTag("Player").First(a => a.gameObject).GetComponent<IdSystem>();
        if (isRandom == true)
        {
            int inContainer = UnityEngine.Random.Range(1, 3);
            for (int i = 0; i < inContainer; i++)
            {
                int numOfItems = idSystem.NumOfItems();
                Item tempItem = idSystem.ItemById(2);
                tempItem.amount = 1;
                items.Add(tempItem);
            }
        }
    }
    private void Update()
    {
        if (items.Count > 0 && destroyOnPickUp == true)
            wasNotEmpty = true;
        if (items.Count == 0 && wasNotEmpty == true)
            Destroy(this.gameObject);
    }
}
