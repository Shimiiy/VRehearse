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
    [SerializeField]
    string path = "D:/Assets/persona-avatar-blender/Shimiiy-VRM.vrm";

    private void Start()
    {
        LoadModelWithMigration(path, new VRMShaders.RuntimeOnlyAwaitCaller());
    }

    async void LoadModelWithMigration(string path, VRMShaders.IAwaitCaller awaitCaller)
    {
        try
        {
            Debug.LogFormat("{0}", path);
            var vrm10Instance = await Vrm10.LoadPathAsync(path,
                canLoadVrm0X: true,
                showMeshes: false,
                awaitCaller: awaitCaller,
                materialGenerator: new UrpVrm10MaterialDescriptorGenerator());

            // Setup FirstPerson for VR
            await vrm10Instance.Vrm.FirstPerson.SetupAsync(vrm10Instance.gameObject, awaitCaller);

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
