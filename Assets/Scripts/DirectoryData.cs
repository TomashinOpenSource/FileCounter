using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public struct DirectoryData
{
    public string Path;
    public int FileCount;

    public DirectoryData(string path)
    {
        Path = path;
        FileCount = new DirectoryInfo(path).GetFiles().Length;
    }

    public string Show()
    {
        return $"{Path} = {FileCount}";
    }
}
