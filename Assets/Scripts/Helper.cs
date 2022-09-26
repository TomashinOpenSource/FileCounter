using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SimpleFileBrowser;
using UnityEngine;

public class Helper : MonoBehaviour
{
    public static async Task<string> SelectFile(FileBrowser.PickMode pickMode)
    {
        await FileBrowser.WaitForLoadDialog(pickMode);
        if (FileBrowser.Success)
        {
            return FileBrowser.Result[0];
        }
        else return null;
    }
}
