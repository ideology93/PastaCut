#if !CRAZY_LABS_CLIK
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Tabtale.TTPlugins;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class TTPAndroidWebDebugPostProcess
{
    public static bool IsAndroidWebDebugEnabled()
    {
#if UNITY_ANDROID && !CRAZY_LABS_CLIK
        Debug.Log("TTPAndroidWebDebugPostProcess:: IsAndroidWebDebugEnabled 1");
        var pathToAdditionalConfig = CoreConfigurationDownloader.CONFIGURATIONS_PATH + "/additionalConfig.json";
        if (File.Exists(pathToAdditionalConfig))
        {
            Debug.Log("TTPAndroidWebDebugPostProcess:: IsAndroidWebDebugEnabled 2");
            var jsonStr = File.ReadAllText(pathToAdditionalConfig);
            if (!string.IsNullOrEmpty(jsonStr) && jsonStr.Trim() != "{}")
            {
                Debug.Log("TTPAndroidWebDebugPostProcess:: IsAndroidWebDebugEnabled 3");
                var dict = TTPJson.Deserialize(jsonStr) as Dictionary<string, object>;
                if (dict != null)
                {
                    Debug.Log("TTPAndroidWebDebugPostProcess:: IsAndroidWebDebugEnabled 4");
                    object val = null;
                    if (dict.ContainsKey("androidWebDebug") && dict.TryGetValue("androidWebDebug", out val) &&
                        val is bool && (bool) val)
                    {
                        Debug.Log("TTPAndroidWebDebugPostProcess:: IsAndroidWebDebugEnabled 5");
                        return true;
                    }
                }
            }
        }
#endif
        return false;
    }

    [PostProcessBuild(0)]
    private static void OnPostProcess(BuildTarget target, string pathToBuiltProject)
    {
        Debug.Log("TTPAndroidWebDebugPostProcess:: OnPostProcess 1 pathToBuiltProject = " + pathToBuiltProject);
        if (IsAndroidWebDebugEnabled())
        {
            CreateOrMergeNetworkSecurityXml(TTPUtils.CombinePaths(new List<string>() { pathToBuiltProject, "RootProject",  "app", "src", "main", "res"}));
#if UNITY_2019_3_OR_NEWER
            CreateOrMergeNetworkSecurityXml(TTPUtils.CombinePaths(new List<string>() { pathToBuiltProject, "RootProject", "app", "launcher",  "src", "main", "res"}));
            CreateOrMergeNetworkSecurityXml(TTPUtils.CombinePaths(new List<string>() { pathToBuiltProject, "RootProject", "app", "unityLibrary",  "src", "main", "res"}));
#endif
        }
    }

    private static void CreateOrMergeNetworkSecurityXml(string pathToRes)
    {
        Debug.Log("TTPAndroidWebDebugPostProcess:: CreateOrMergeNetworkSecurityXml: " +pathToRes);
        if (!Directory.Exists(pathToRes))
        {
            Debug.Log("TTPAndroidWebDebugPostProcess:: CreateOrMergeNetworkSecurityXml: Ensured res folder");
            Directory.CreateDirectory(pathToRes);
        }
        var pathToXmlRes = Path.Combine(pathToRes, "xml");
        if (!Directory.Exists(pathToXmlRes))
        {
            Debug.Log("TTPAndroidWebDebugPostProcess:: CreateOrMergeNetworkSecurityXml: Creating xml folder in res");
            Directory.CreateDirectory(pathToXmlRes);
        }
        var pathToNetworkSecurityXml = Path.Combine(pathToXmlRes, "network_security_config.xml");
        var xmlDocument = new XmlDocument();
        if (File.Exists(pathToNetworkSecurityXml))
        {
            Debug.Log("TTPAndroidWebDebugPostProcess:: CreateOrMergeNetworkSecurityXml: Loading XML file");
            xmlDocument.Load(pathToNetworkSecurityXml);
        }
        else
        {
            Debug.Log("TTPAndroidWebDebugPostProcess:: CreateOrMergeNetworkSecurityXml: Creating XML file");
            var xmlDeclaration = xmlDocument.CreateXmlDeclaration( "1.0", "UTF-8", null );
            var root = xmlDocument.CreateElement("network-security-config");
            xmlDocument.AppendChild(root);
            xmlDocument.InsertBefore( xmlDeclaration, root );
        }
        var networkSecConfig = xmlDocument.DocumentElement;
        var debugOverridesNode = AddXmlElement(xmlDocument, "debug-overrides", networkSecConfig);
        var trustAnchors = AddXmlElement(xmlDocument, "trust-anchors", debugOverridesNode);
        var attr = xmlDocument.CreateAttribute("src");
        attr.Value = "user";
        var certificates = AddXmlElement(xmlDocument, "certificates", trustAnchors);
        if (certificates.Attributes != null)
        {
            certificates.Attributes.Append(attr);
        }
        Debug.Log("TTPAndroidWebDebugPostProcess:: CreateOrMergeNetworkSecurityXml: Saving XML file = " + xmlDocument.ToString());
        xmlDocument.Save(pathToNetworkSecurityXml);
    }

    private static XmlNode AddXmlElement(XmlDocument xmlDocument, string elementName, XmlNode parentElement)
    {
        XmlNode node = null;
        try
        {
            node = parentElement.SelectSingleNode(elementName);
        }
        catch (Exception e)
        {
            Debug.Log("TTPAndroidWebDebugPostProcess:: OnPostProcess: did not find element " + elementName);
            Console.WriteLine(e);
        }
        if (node == null)
        {
            node = xmlDocument.CreateElement(elementName);
            Debug.Log("TTPAndroidWebDebugPostProcess:: OnPostProcess: node = " + node + " parentElement = " + parentElement);
            parentElement.AppendChild(node);
        }

        return node;
    }
}
#endif