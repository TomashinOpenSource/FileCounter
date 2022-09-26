using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimpleFileBrowser;
using TMPro;
using Unity.VisualScripting;
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
        OnCountButtonPressed();
    }

    public void OnInputCountDepth(string value)
    {
        Depth = int.Parse(value);
    }

    public void OnCountButtonPressed()
    {
        CountFilesInEachFolder();
    }

    private void CountAllFilesInDirectory()
    {
        List<string> paths = Directory.GetFiles(Path, "*.*", SearchOption.AllDirectories).ToList();
        int pathDepth = Helper.FolderDepth(Path);
        if (Depth > 0) paths = paths.Where(path => (Helper.FolderDepth(path) - pathDepth) <= Depth).ToList();
        outputTextField.text = $"Directory = {Path}\nDepth = {Depth}\nFiles = {paths.Count}\n\n";
        for (int i = 0; i < paths.Count; i++)
        {
            outputTextField.text += $"{i + 1} - {paths[i]} - Depth = {Helper.FolderDepth(paths[i]) - pathDepth}\n";
        }
    }

    private void CountFilesInEachFolder()
    {
        List<DirectoryData> directoryDatas = GetDirectoryDatas(Path);
        outputTextField.text = "";
        foreach (var directoryData in directoryDatas)
        {
            outputTextField.text += directoryData.Show() + "\n";
        }
    }
    private List<DirectoryData> GetDirectoryDatas(string _path)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(_path);
        List<DirectoryData> directoryDatas = new List<DirectoryData>();
        int directoriesCount = directoryInfo.GetDirectories().Length;
        int filesCount = directoryInfo.GetFiles().Length;
        if (filesCount > 0) directoryDatas.Add(new DirectoryData(_path));
        foreach (var directories in directoryInfo.GetDirectories())
        {
            directoryDatas.AddRange(GetDirectoryDatas(directories.FullName));
        }

        return directoryDatas;
    }
}
