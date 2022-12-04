#if !CRAZY_LABS_CLIK
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEditor;

namespace Tabtale.TTPlugins
{
    [InitializeOnLoad]
    public class CoreConfigurationDownloader
    {

        private const string CORE_URL_ADDITION = "/global/";
        private const string CORE_JSON_FN = "global";
        private const string STRICT_MODE_JSON_FN = "androidStrictMode";
        private const string ADDITIONAL_CONFIG_JSON_FN = "additionalConfig";
        
#if UNITY_ANDROID
        public const string CONFIGURATIONS_PATH = "Assets/Plugins/Android/assets/ttp/configurations";
#else
        public const string CONFIGURATIONS_PATH = "Assets/StreamingAssets/ttp/configurations";
#endif
            
        static CoreConfigurationDownloader()
        {
            TTPMenu.OnDownloadConfigurationCommand += DownloadConfiguration;
        }

        private static void AndoridWebDebug()
        {
            Debug.Log("CoreConfigurationDownloader:: AndoridWebDebug");
            if (TTPAndroidWebDebugPostProcess.IsAndroidWebDebugEnabled())
            {
                Debug.Log("CoreConfigurationDownloader:: AndoridWebDebug 2");
                var pathToAndroidManifest = "Assets/Plugins/Android/AndroidManifest.xml";
                if (File.Exists(pathToAndroidManifest))
                {
                    Debug.Log("CoreConfigurationDownloader:: AndoridWebDebug 3");
                    var xmlDocument = new XmlDocument();
                    xmlDocument.Load(pathToAndroidManifest);
                    var node = xmlDocument.DocumentElement;
                    var applicationNode = node.SelectSingleNode("application");
                    Debug.Log(applicationNode.Name);
                    var netSecConfigAttr = xmlDocument.CreateAttribute("andorid", "networkSecurityConfig","http://schemas.android.com/apk/res/android");
                    netSecConfigAttr.Value = "@xml/network_security_config";
                    var andoridDebugAttr = xmlDocument.CreateAttribute("andorid", "debuggable","http://schemas.android.com/apk/res/android");
                    andoridDebugAttr.Value = "true";
                    if (applicationNode.Attributes == null) return;
                    Debug.Log("CoreConfigurationDownloader:: AndoridWebDebug 4");
                    applicationNode.Attributes.Append(netSecConfigAttr);
                    applicationNode.Attributes.Append(andoridDebugAttr);
                    xmlDocument.Save(pathToAndroidManifest);
                }
            }
        }
        
        private static void DownloadConfiguration(string domain)
        {
            string store = "google";
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
            {
                store = "apple";
            }
            string url = domain + CORE_URL_ADDITION + store + "/" + PlayerSettings.applicationIdentifier;
            bool result = TTPMenu.DownloadConfiguration(url, CORE_JSON_FN);
            if (!result)
            {
                Debug.LogWarning("CoreConfigurationDownloader:: DownloadConfiguration: failed to download global configuration.");
            }
#if UNITY_ANDROID
            string strictModeConfigUrl = domain + "/" + STRICT_MODE_JSON_FN + "/" + store + "/" + PlayerSettings.applicationIdentifier;
            bool strictModeResult = TTPMenu.DownloadConfiguration(strictModeConfigUrl, STRICT_MODE_JSON_FN);
            if (!strictModeResult)
            {
                Debug.LogWarning("CoreConfigurationDownloader:: DownloadConfiguration: failed to download configuration for strict mode android");
            }
#endif
            var additionalConfigUrl = domain + "/" + ADDITIONAL_CONFIG_JSON_FN + "/" + store + "/" + PlayerSettings.applicationIdentifier;
            var additionalConfigResult = TTPMenu.DownloadConfiguration(additionalConfigUrl, ADDITIONAL_CONFIG_JSON_FN);
            if (!additionalConfigResult)
            {
                Debug.LogWarning("CoreConfigurationDownloader:: DownloadConfiguration: failed to download additional configuration");
            }
#if UNITY_IOS
            else
            {
                var json = File.ReadAllText(CONFIGURATIONS_PATH + "/" + CORE_JSON_FN + ".json");
                if (json != null)
                {
                    var dict = TTPJson.Deserialize(json) as Dictionary<string, object>;
                    if (dict != null)
                    {
                        var conversionModelType = "A";
                        if (dict.ContainsKey("conversionModelType"))
                        {
                            conversionModelType = dict["conversionModelType"] as string;
                        }
                        var conversionRulesUrl =
 "http://promo-images.ttpsdk.info/conversionJS/"+conversionModelType+"/conversion.js";
                        TTPEditorUtils.DownloadStringToFile(conversionRulesUrl, "Assets/StreamingAssets/ttp/conversion/conversion.js");
                    }
                }
            }
#endif
            AndoridWebDebug();
        }
        
    }
}
#endif