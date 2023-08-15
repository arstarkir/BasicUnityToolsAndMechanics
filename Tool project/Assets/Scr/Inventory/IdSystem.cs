using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;
using UnityEngine.Rendering.VirtualTexturing;

[ExecuteInEditMode] //needed for future conveniences
public class IdSystem : MonoBehaviour
{
    public List<Item> idSystem = new List<Item>();
    private List<string> itemData = new List<string>();
    void OnValidate() //needed for future conveniences
    {
        readTextFile("Assets/Items.txt");
        ConstructItemDatabase();
    }
    void Start()
    {
        idSystem.Clear();
        itemData.Clear();
        readTextFile("Assets/Items.txt");
        ConstructItemDatabase();
    }
    void readTextFile(string file_path)
    {
        StreamReader inp_stm = new StreamReader(file_path);

        while (!inp_stm.EndOfStream)
        {
            itemData.Add(inp_stm.ReadLine());
        }

        inp_stm.Close();
    }

    public Item ItemById(int id)
    {
        for (int i = 0; i < idSystem.Count; i++) 
        {
            if (idSystem[i].id == id)
            {
                return idSystem[i];
            }
        }

        return null;
    }
    public int NumOfItems()
    {
        return idSystem.Count;
    }

    void ConstructItemDatabase()
    {
        for (int j = 0; j < itemData.Count; j++)
        {
            var matches = Regex.Matches(itemData[j], @"\w+[^\s]*\w+|\w");
            
            Item newItem = new Item();
            int i = 0;
            foreach (Match match in matches)
            {
                var part = match.Value;
                switch (i)
                {
                    case 0:
                        newItem.id = Int32.Parse(part);
                        break;
                    case 1:
                        newItem.title = part.ToString();
                        break;
                    case 2:
                        if (part == "1")
                            newItem.isStackabel = true;
                        else newItem.isStackabel = false;
                        break;
                    case 3:
                        if (part == "1")
                            newItem.sprite = Resources.Load<Sprite>("Sprites/Items/" + newItem.title); // file of the sprite should be in folder "Assets/Resources/Sprites/Items/"
                        break;
                    case 4:
                        newItem.inHandabel = Int32.Parse(part);    
                        break;
                    case 5:
                        newItem.prefab = Resources.Load<GameObject>("GameObjects/" + newItem.title); // file of the sprite should be in folder "Assets/Resources/GameObject/Items/"
                        break;
                    case 6:
                        if (part == "1")
                            newItem.isSellabel = true;
                        else newItem.isSellabel = false;
                        break;
                    case 7:
                        if (newItem.isSellabel == true)
                            newItem.cost = float.Parse(part);
                        break;
                 
                }
                i++;
            }
            
            idSystem.Add(newItem);
        }
    }
}

public class Item
{
    public int id { get; set; }
    public string title { get; set; } // title = the name of a sprite and gameobject
    public bool isStackabel { get; set; }
    public int amount { get; set; }
    public Sprite sprite { get; set; }
    public GameObject prefab { get; set; }
    public int inHandabel { get; set; }
    public bool isSellabel { get; set; }
    public float cost { get; set; }
    IdSystem idSystem;
    public Item(int givenItemID = 0, int givenAmount = 0, string givenTitel = null, Sprite givenSprite = null, GameObject givenPrefab = null, float givenCost = 0)
    {
        cost = givenCost;
        amount = givenAmount;
        id = givenItemID;
        title = givenTitel;
        sprite = givenSprite;
        prefab = givenPrefab;
    }
    public Item(Item item)
    {
        id = item.id;
        title = item.title;
        sprite = item.sprite;
        prefab = item.prefab;
        isStackabel = item.isStackabel;
        cost = item.cost;
        amount = item.amount;
        inHandabel = item.inHandabel;
        isSellabel = item.isSellabel;
    } 
}