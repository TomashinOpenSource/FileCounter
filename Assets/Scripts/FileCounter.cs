using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimpleFileBrowser;
using TMPro;
using UnityEngine;

public class FileCounter : MonoBehaviour
{
    private const string selectionButtonTextFormat = "Directory = {0}\nClick the button to select a directory...";
    
    [Header("Fields")]
    [SerializeField] private TMP_Text selectionButtonTextField;
    [SerializeField] private TMP_Text outputTextField;
    
    [Header("State")]
    [SerializeField] private string path;
    [SerializeField] private int depth;
    [SerializeField] private List<string> paths;

    public string Path
    {
        get => path;
        set
        {
            path = value;
            selectionButtonTextField.text =
                string.Format(selectionButtonTextFormat, !string.IsNullOrEmpty(value) ? value : "Null");
        }
    }

    public int Depth
    {
        get => depth;
        set => depth = value;
    }

    private void Start()
    {
        Path = null;
        Depth = 0;
    }

    public async void OnSelectDirectoryPressed()
    {
        Path = await Helper.SelectFile(FileBrowser.PickMode.Folders);
    }

    public void OnInputCountDepth(string value)
    {
        Depth = int.Parse(value);
    }

    public void OnCountButtonPressed()
    {
        paths = Directory.GetFiles(Path, "*.*", SearchOption.AllDirectories).ToList();
        int pathDepth = FolderDepth(Path);
        if (Depth > 0) paths = paths.Where(path => (FolderDepth(path) - pathDepth) <= Depth).ToList();
        outputTextField.text = $"Directory = {Path}\nDepth = {Depth}\nFiles = {paths.Count}\n\n";
        for (int i = 0; i < paths.Count; i++)
        {
            outputTextField.text += $"{i + 1} - {paths[i]} - Depth = {FolderDepth(paths[i]) - pathDepth}\n";
        }
    }
    public static int FolderDepth(string path)
    {
        if (string.IsNullOrEmpty(path))
            return 0;
        DirectoryInfo parent = Directory.GetParent(path);
        if (parent == null)
            return 1;
        return FolderDepth(parent.FullName) + 1;
    }
}
