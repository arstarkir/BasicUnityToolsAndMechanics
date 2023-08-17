using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class ItemTool : EditorWindow
{
    bool newItem = false;
    string title;
    bool isStackabel = false;
    bool inHandabel = false;
    bool isSellabel = false;
    float cost = 0;

    [MenuItem("Tools/Item Tool")]
    public static void ShowWindow()
    {
        GetWindow<ItemTool>("Item Tool");
    }
    void OnGUI()
    {
        if (newItem)
            NewItem();
        else
            newItem = GUILayout.Button("New Item");
    }
    void NewItem()
    {
        EditorGUILayout.LabelField("Name of the item:");
        title = EditorGUILayout.TextField(title);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("isStackabel:", GUILayout.Width(80));
        isStackabel = EditorGUILayout.Toggle(isStackabel);
        EditorGUILayout.LabelField("inHandabel:", GUILayout.Width(80));
        inHandabel = EditorGUILayout.Toggle(inHandabel);
        EditorGUILayout.LabelField("isSellabel:", GUILayout.Width(80));
        isSellabel = EditorGUILayout.Toggle(isSellabel);
        EditorGUILayout.EndHorizontal();
        if(isSellabel)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("How much to buy this item:",GUILayout.Width(160));
            cost = EditorGUILayout.FloatField(cost, GUILayout.Width(30));
            EditorGUILayout.LabelField($"Sell cost = {(cost / 100) * 80}", GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
        }
        if(GUILayout.Button("Clear"))
        {
             newItem = false;
             title = "";
             isStackabel = false;
             inHandabel = false;
             isSellabel = false;
             cost = 0;
        }
        if (GUILayout.Button("Save Item"))
            SaveItem("Assets/Items.txt");
    }
    void SaveItem(string path)
    {
        StreamReader streamReader = new StreamReader(path);
        List<string> allLines = new List<string>();
        while (!streamReader.EndOfStream)
        {
            allLines.Add(streamReader.ReadLine());
        }
        MatchCollection matches = Regex.Matches(allLines[allLines.Count - 1], @"\w+[^\s]*\w+|\w");
        string cleanedString = matches[0].ToString();
        int id = Int32.Parse(cleanedString) + 1;
        int isStackabelInt = (isStackabel == true) ? 1 : 0;
        int inHandabelInt = (inHandabel == true) ? 1 : 0;
        int isSellabelInt = (isSellabel == true) ? 1 : 0;
        string data = $"{id} {title} {isStackabelInt} 1 {inHandabelInt} 1 {isSellabelInt} {cost}";
        streamReader.Close();
        
        
        allLines.Add(data);
        StreamWriter out_stm = new StreamWriter(path);
        foreach (string line in allLines)
        {
            out_stm.WriteLine(line);
        }
        out_stm.Close();
        newItem = false;
        title = "";
        isStackabel = false;
        inHandabel = false;
        isSellabel = false;
        cost = 0;
    }
}
