using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;
using System.Text.RegularExpressions;

namespace CrazyLabsHubs.Editor
{
    public class CLIKQuickSetupForCPITestEditor : EditorWindow
    {

        SceneAsset gameSceneAsset;
        string teamName;
        string gameName;

        [MenuItem("Quick CLIK/Copy BundleID to clipboard")]
        static void CopyBundleIDToClipboard()
        {
            var bundleId = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);
            GUIUtility.systemCopyBuffer = bundleId;
        }

        [MenuItem("Quick CLIK/Quick setup for CPI test")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            CLIKQuickSetupForCPITestEditor window = (CLIKQuickSetupForCPITestEditor)EditorWindow.GetWindow(typeof(CLIKQuickSetupForCPITestEditor));
            window.Show();
            window.teamName = PlayerSettings.companyName;
            window.gameName = PlayerSettings.productName;
        }

        void OnGUI()
        {
            //first level scene

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Step 1", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("First Level (Game Scene)");

            gameSceneAsset = (SceneAsset)EditorGUILayout.ObjectField(gameSceneAsset, typeof(SceneAsset), false);
            EditorGUILayout.EndHorizontal();
            var hasScene = gameSceneAsset != null;
            if (!hasScene)
            {
                EditorGUILayout.HelpBox("You need to specify first level (scene) of your game!", MessageType.Error);
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            //bundle id check

            EditorGUILayout.LabelField("Step 2", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            var hasDefaultCompanyAndProductName = string.Equals(teamName, "Default Company") || string.Equals(teamName, "DefaultCompany");

            EditorGUILayout.BeginHorizontal();

            var defaultLabelWidth = EditorGUIUtility.labelWidth;
            teamName = EditorGUILayout.TextField(teamName);
            gameName = EditorGUILayout.TextField(gameName);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Company Name (Team)");
            EditorGUILayout.LabelField("Product Name (Game)");

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.HelpBox("Your bundle id will look like this:", MessageType.Info);
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            var x = teamName.Trim();
            x = Regex.Replace(x, @"\s+", "");
            var y = gameName.Trim();
            y = Regex.Replace(y, @"\s+", "");
            var bundleId = string.Format("com.{0}.{1}", x, y).ToLower();
            EditorGUILayout.LabelField(bundleId);
            GUI.enabled = true;
            if (GUILayout.Button("copy to clipboard"))
            {
                GUIUtility.systemCopyBuffer = bundleId;
            }
            EditorGUILayout.EndHorizontal();

            if (hasDefaultCompanyAndProductName)
            {
                EditorGUILayout.HelpBox("You need to specify valid bundle id! (usualy in format com.TeamName.GameName", MessageType.Error);
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            //finish
            EditorGUILayout.LabelField("Step 3", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            GUI.enabled = hasScene && !hasDefaultCompanyAndProductName;

            if (GUILayout.Button("Finish"))
            {
                var currentPlatform = EditorUserBuildSettings.activeBuildTarget;
                if (currentPlatform != BuildTarget.Android)
                {
                    EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
                }

                //Create the empty scene with tt plugin
                var scenePath = @"Assets/QuickSetupScene.unity";
                var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

                if (!hasDefaultCompanyAndProductName)
                {
                    PlayerSettings.companyName = teamName;
                    PlayerSettings.productName = gameName;
                    PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, string.Format("com.{0}.{1}", teamName, gameName).ToLower());
                }

                //Create a GameObject that will contain loader script for ttplugins
                GameObject go = new GameObject();
                go.name = "QuickSetupManager";
                var quickCLICK = go.AddComponent<QuickLoadCLIK>();
                quickCLICK.sceneToLoadName = gameSceneAsset.name;

                //Add component here for the activating the tt plugins
                EditorSceneManager.SaveScene(scene, scenePath);

                //Add created Scene to the build settings
                var existingScenes = EditorBuildSettings.scenes;
                var withoutDuplicates = existingScenes.GroupBy(x => x.path).Select(y => y.First()).ToList();

                var alreadyContainsScene = withoutDuplicates.Any(x => string.Equals(x.path, scenePath));

                if (alreadyContainsScene)
                {
                    var sceneToRemove = withoutDuplicates.SingleOrDefault(x => string.Equals(x.path, scenePath));
                    if (sceneToRemove != null)
                    {
                        withoutDuplicates.Remove(sceneToRemove);
                    }
                }

                EditorBuildSettings.scenes = AddSceneToBuildAtIndex(AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath), 0).ToArray();

                EditorBuildSettings.scenes = AddSceneToBuildAtIndex(gameSceneAsset, 1).ToArray();

                //Change the unity build settings for android
                //Unity supported versions as per https://sites.google.com/tabtale.com/clhelpcenter/clik-plugin
                //2019.2.17, 2019.3.15, 2019.4.28, 2020.1.14 

#if UNITY_ANDROID

                PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
                PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Android, 0);
                PlayerSettings.stripEngineCode = false;

                PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel23;
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;

#if UNITY_2020_1_OR_NEWER

                PlayerSettings.Android.minifyWithR8 = false;
                PlayerSettings.Android.minifyDebug = false;

                PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel30;
#endif

#if UNITY_2020_3_OR_NEWER
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;
#endif

                // #if UNITY_2019_4
                //             PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel30;
                // #endif

                // #if UNITY_2019_2 || UNITY_2019_3
                //             PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel28;
                // #endif

#endif
            }

            GUI.enabled = true;
        }

        public List<EditorBuildSettingsScene> AddSceneToBuildAtIndex(SceneAsset whichScene, int slot)
        {
            var existingScenes = EditorBuildSettings.scenes.ToList();

            //clamp the slot if we are trying to add a scene at slot 5 lets say but there are only two scenes
            slot = Mathf.Clamp(slot, 0, existingScenes.Count);

            //if the scene exists already we just need to move it to the index
            var scene = existingScenes.FirstOrDefault(x => x.path == AssetDatabase.GetAssetOrScenePath(whichScene));
            var sceneAlreadyInExisting = scene != null;

            if (sceneAlreadyInExisting)
            {
                existingScenes.Remove(scene);

            }
            else
            {
                scene = new EditorBuildSettingsScene(AssetDatabase.GetAssetOrScenePath(whichScene), true);

            }
            existingScenes.Insert(slot, scene);
            return existingScenes;
        }
    }
}