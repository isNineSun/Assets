using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FileDialog : MonoBehaviour
{
    public string OpenFilePanel(string title, string defaultPath, string extension)
    {
        string selectedFilePath = string.Empty;
#if UNITY_EDITOR
        selectedFilePath = EditorUtility.OpenFilePanel(title, defaultPath, extension);
#elif UNITY_STANDALONE_WIN
        //For Windows
        selectedFilePath = OpenWindowsFileDialog(title, defaultPath, extension);
#endif

        if (!string.IsNullOrEmpty(selectedFilePath))
        {
            Debug.Log("Selected file: " + selectedFilePath);
        }
        else
        {
            Debug.Log("File selection canceled.");
        }

        return selectedFilePath;
    }


#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    //For Windows
    [DllImport("Comdlg32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern bool GetOpenFileName([In, Out] FileDialogWin.OPENFILENAME ofn);

    private string OpenWindowsFileDialog(string title, string defaultPath, string extension)
    {
        FileDialogWin.OPENFILENAME ofn = new FileDialogWin.OPENFILENAME();
        ofn.structSize = System.Runtime.InteropServices.Marshal.SizeOf(ofn);
        ofn.filter = $"{extension}\0*.{extension}\0All Files\0*.*\0\0";
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.title = title;
        ofn.initialDir = defaultPath;

        if (GetOpenFileName(ofn))
        {
            return ofn.file;
        }

        return null;
    }

    public static class FileDialogWin
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class OPENFILENAME
        {
            public int structSize = 0;
            public IntPtr dlgOwner = IntPtr.Zero;
            public IntPtr instance = IntPtr.Zero;

            public string filter = null;
            public string customFilter = null;
            public int maxCustFilter = 0;
            public int filterIndex = 0;

            public string file = null;
            public int maxFile = 0;

            public string initialDir = null;
            public string title = null;
            public int flags = 0;
            public short fileOffset = 0;
            public short fileExtension = 0;

            public string defExt = null;
            public IntPtr custData = IntPtr.Zero;
            public IntPtr hook = IntPtr.Zero;
            public string templateName = null;
            public IntPtr reservedPtr = IntPtr.Zero;
            public int reservedInt = 0;
            public int flagsEx = 0;
        }
    }
#endif
}
