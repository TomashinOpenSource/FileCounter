using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SimpleFileBrowser;
using UnityEngine;

public class Helper : MonoBehaviour
{
    public static async Task<string> SelectFile(FileBrowser.PickMode pickMode, string initialPath)
    {
        await FileBrowser.WaitForLoadDialog(pickMode, initialPath:initialPath);
        if (FileBrowser.Success)
        {
            return FileBrowser.Result[0];
        }
        else return null;
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
