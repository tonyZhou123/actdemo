using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class EditorUtils {

    static List<string> GetSelectedAllPaths(string extension)
    {
        List<string> list = new List<string>();
        Object[] objs = Selection.objects;
        for (int i = 0; i < objs.Length; i++)
        {
            string path = AssetDatabase.GetAssetPath(objs[i]);
            if (path.IndexOf(extension, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                list.Add(path);
            }
        }
        return list;
    }

    [MenuItem("Assets/MyTools/提取Mesh")]
    static public void ExtractMesh()
    {
        List<string> paths = GetSelectedAllPaths(".fbx");

        if (paths.Count > 0)
        {
            UnityEditor.EditorUtility.DisplayProgressBar("Extract Mesh", "", 0);
            for (int i = 0; i < paths.Count; ++i)
            {
                ExtractMesh(paths[i]);
                UnityEditor.EditorUtility.DisplayProgressBar("Extract Mesh", i + "/" + paths.Count, i / (float)paths.Count);
            }
            UnityEditor.EditorUtility.ClearProgressBar();
        }
        else
        {
            Debug.Log("ExtractMesh: No *.fbx selected...");
        }
    }

    static public void ExtractMesh(string assetPath)
    {
        UnityEngine.Object[] objs = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath);
        int index = 0;
        if (objs != null && objs.Length != 0)
        {
            foreach (var obj in objs)
            {
                index++;
                Mesh mesh = obj as Mesh;
                if (mesh != null)
                {
                    // 提取*.mesh
                    string filePath = System.IO.Path.GetDirectoryName(assetPath) + "/" + Path.GetFileNameWithoutExtension(assetPath) + index + ".asset";
                    UnityEngine.Mesh clone = UnityEngine.Object.Instantiate(mesh) as Mesh;
                    if (clone != null)
                    {
                        clone.colors = null;
                        clone.uv2 = null;
                        clone.uv3 = null;
                        clone.uv4 = null;

                        AssetDatabase.CreateAsset(clone, filePath);
                    }
                }
            }
        }
    }

    [MenuItem("Assets/MyTools/提取动画文件")]
    static public void ExtractAnimClip()
    {
        List<string> paths = GetSelectedAllPaths(".fbx");

        if (paths.Count > 0)
        {
            UnityEditor.EditorUtility.DisplayProgressBar("Extract AnimClip", "", 0);
            for (int i = 0; i < paths.Count; ++i)
            {
                ExtractAnimClip(paths[i]);

                UnityEditor.EditorUtility.DisplayProgressBar("Extract AnimClip", i + "/" + paths.Count, i / (float)paths.Count);
            }
            UnityEditor.EditorUtility.ClearProgressBar();
        }
        else
        {
            Debug.Log("ExtractAnimClip: No *.fbx selected...");
        }
        AssetDatabase.SaveAssets();
    }

    static public void ExtractAnimClip(string assetPath, bool bForceCreate = false)
    {
        UnityEngine.Object[] objs = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath);
        if (objs != null && objs.Length != 0)
        {
            foreach (UnityEngine.Object obj in objs)
            {
                AnimationClip clip = obj as AnimationClip;
                if (clip != null)
                {
                    //提取 *.anim
                    string filePath = System.IO.Path.GetDirectoryName(assetPath) + "/" + obj.name + ".anim";
                    if (AssetDatabase.LoadAssetAtPath<AnimationClip>(filePath) == null || bForceCreate)
                    {
                        UnityEngine.Object clone = UnityEngine.Object.Instantiate(clip);

                        // 优化精度
                        //OptimizeAnim(clone as AnimationClip);

                        string path = "Assets/" + obj.name + ".anim";
                        AssetDatabase.CreateAsset(clone, path);

                        string backUpPath = "Assets/Editor Default Resources/" + obj.name + ".anim";
                        string oldPath = path;
                        string newPath = filePath;

                        if (File.Exists(newPath))
                        {
                            string abBackupPath = Application.dataPath.Replace("Assets", "") + backUpPath;
                            string abOldPath = Application.dataPath.Replace("Assets", "") + oldPath;
                            string abNewPath = Application.dataPath.Replace("Assets", "") + newPath;

                            File.Replace(abOldPath, abNewPath, abBackupPath);
                            AssetDatabase.DeleteAsset(backUpPath);
                            AssetDatabase.DeleteAsset(oldPath);
                        }
                        else
                        {
                            AssetDatabase.MoveAsset(oldPath, newPath);
                        }
                    }
                }
            }
        }
    }
}
