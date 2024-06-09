using UnityEngine;
using System;
using System.IO;
using UniGLTF;
using VRM;
using VRMShaders;
using UniVRM10;
using System.Threading;


public class VRMManager : MonoBehaviour
{
    RuntimeGltfInstance gltfInstance;

    [SerializeField]
    string path = "D:/Assets/persona-avatar-blender/Shimiiy-VRM.vrm";

    private void Start()
    {
        // LoadModel();
        LoadModelWithMigration(path);
    }

    async void LoadModel()
    {
        Debug.Log(path);
        this.gltfInstance = await VrmUtility.LoadAsync(path, new RuntimeOnlyAwaitCaller());
        this.gltfInstance.ShowMeshes();
    }

    async void LoadModelWithMigration(string path)
    {
        try
        {
            Debug.LogFormat("{0}", path);
            var vrm10Instance = await Vrm10.LoadPathAsync(path,
                canLoadVrm0X: true,
                showMeshes: false,
                materialGenerator: new UrpVrm10MaterialDescriptorGenerator());

            var instance = vrm10Instance.GetComponent<RuntimeGltfInstance>();
            instance.ShowMeshes();
            instance.EnableUpdateWhenOffscreen();
        }
        catch (Exception ex)
        {
            if (ex is Exception)
            {
                Debug.LogWarning($"Canceled to Load: {path}");
            }
            else
            {
                Debug.LogError($"Failed to Load: {path}");
                Debug.LogException(ex);
            }
        }
    }

}
