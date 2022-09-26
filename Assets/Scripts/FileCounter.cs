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

    private void Start()
    {
    }

    public async void OnSelectDirectoryPressed()
    {
        path = await Helper.SelectFile(FileBrowser.PickMode.Folders);
        selectionButtonTextField.text =
            string.Format("Directory = {0}\nClick the button to select a directory...", !string.IsNullOrEmpty(path) ? path : "Null");
        OnCountButtonPressed();
    }

    public void OnInputCountDepth(string value)
    {
        depth = int.Parse(value);
    }

    public void OnCountButtonPressed()
    {
        CountFilesInEachFolder();
    }

    private void CountAllFilesInDirectory()
    {
        List<string> paths = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).ToList();
        int pathDepth = Helper.FolderDepth(path);
        if (depth > 0) paths = paths.Where(path => (Helper.FolderDepth(path) - pathDepth) <= depth).ToList();
        outputTextField.text = $"Directory = {path}\nDepth = {depth}\nFiles = {paths.Count}\n\n";
        for (int i = 0; i < paths.Count; i++)
        {
            outputTextField.text += $"{i + 1} - {paths[i]} - Depth = {Helper.FolderDepth(paths[i]) - pathDepth}\n";
        }
    }

    private void CountFilesInEachFolder()
    {
        List<DirectoryData> directoryDatas = GetDirectoryDatas(path);
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
        if (directoryInfo.GetFiles().Length > 0) directoryDatas.Add(new DirectoryData(_path));
        foreach (var directories in directoryInfo.GetDirectories())
        {
            directoryDatas.AddRange(GetDirectoryDatas(directories.FullName));
        }
        return directoryDatas;
    }

    public async void OnSaveButtonPressed()
    {
        if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(outputTextField.text)) return;
        path = await Helper.SelectFile(FileBrowser.PickMode.Folders);
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        StreamWriter streamWriter = new StreamWriter(Path.Combine(directoryInfo.FullName, $"{directoryInfo.Name}_Report.txt"));
        streamWriter.WriteLine(outputTextField.text);
        streamWriter.Close();
    }
}
