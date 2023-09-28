using System;
using HarmonyLib;
using BepInEx;
using UnityEngine;
using System.IO;
using System.Collections;
using emotitron.Compression;

namespace SkyboxChanger
{
    [BepInPlugin(PluginInfo.modGUID, PluginInfo.modName, PluginInfo.modVersion)]
    public class MainPatch : BaseUnityPlugin
    {
        // public void Awake()
        //{
        // var harmony = new Harmony(PluginInfo.modGUID);
        //harmony.PatchAll(Assembly.GetExecutingAssembly());
        // }

        static Material SkyboxMaterial;

        static GameObject obj = null;

        static bool LoadingImage;


        void Start()
        {
        }

        void Update()
        {
            //Checks if image is loaded and skybox is loaded
            if (!LoadingImage && GameObject.Find("Environment Objects/LocalObjects_Prefab/Standard Sky") != null)
            {
                //sets skybox material
                SkyboxMaterial = new Material(Shader.Find("GorillaTag/UberShader"));
                //tells the shader to use the texture
                SkyboxMaterial.shaderKeywords = new string[]
                {
                    "_USE_TEXTURE",
                };
                //SkyboxMaterial.color = Color.white;
                LoadingImage = true;
                //path to the file
                string AppPath = Application.dataPath;
                AppPath = AppPath.Replace("/Gorilla Tag_Data", "");
                string path = AppPath + @"/BepInEx/plugins/Skybox/";
                Debug.Log (path);
                //loads the file as bytes
                byte[] bytes = File.ReadAllBytes(Directory.GetFiles(path, "*.jpeg")[0]);
                Texture2D loadTexture = new Texture2D(4096, 4096);
                ImageConversion.LoadImage(loadTexture, bytes);
                loadTexture.Apply();
                //byte[] debugImage = loadTexture.EncodeToPNG();
                //File.WriteAllBytes(path + "/../Img1.png", debugImage);
                //applies the texture
                SkyboxMaterial.mainTexture = loadTexture;
                //finds the skybox and sets its material to the new one
                obj = GameObject.Find("Environment Objects/LocalObjects_Prefab/Standard Sky");
                obj.GetComponent<MeshRenderer>().material = SkyboxMaterial;
                obj.GetComponentInChildren<MeshRenderer>().material = SkyboxMaterial;
            }
        }
    }
}