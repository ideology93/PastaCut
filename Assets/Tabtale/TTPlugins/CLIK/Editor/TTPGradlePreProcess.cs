using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class TTPGradlePreProcess : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(BuildReport report)
    {
        Debug.Log("TTPGradlePreProcess::OnPreprocessBuild for target " + report.summary.platform + " at path " + report.summary.outputPath);
#if UNITY_ANDROID
        Debug.Log("TTPGradlePreProcess::OnPreprocessBuild:Android && CLIK");
        var gradleTemplatesPath = Path.Combine(Application.dataPath, "Tabtale/TTPlugins/CLIK/gradletemplates");
        string unityVersion = null;
#if UNITY_2020_3_OR_NEWER
        Debug.Log("TTPGradlePreProcess::OnPreprocessBuild:UNITY_2020_1_OR_NEWER");
        unityVersion = "Unity.2020.3.11f1";
#elif UNITY_2020_1_OR_NEWER
        Debug.Log("TTPGradlePreProcess::OnPreprocessBuild:UNITY_2020_1_OR_NEWER");
        unityVersion = "Unity.2020.1.14f1";
#elif UNITY_2019_3_OR_NEWER
        Debug.Log("TTPGradlePreProcess::OnPreprocessBuild:UNITY_2019_1_OR_NEWER");
        unityVersion = "Unity.2019.3.15f1";
#endif
        if (string.IsNullOrEmpty(unityVersion))
        {
            return;
        }
        //todo in first time, back up original templates
        string launcherTemplateDestinationPath = Path.Combine(Application.dataPath, "Plugins/Android/launcherTemplate.gradle");
        string mainTemplateDestinationPath = Path.Combine(Application.dataPath, "Plugins/Android/mainTemplate.gradle");
        string launcherTemplateSourcePath = Path.Combine(gradleTemplatesPath, unityVersion + "/launcherTemplate.gradle");
        string mainTemplateSourcePath = Path.Combine(gradleTemplatesPath, unityVersion + "/mainTemplate.gradle");

        if (File.Exists(launcherTemplateDestinationPath))
        {
            string templateGradleText = File.ReadAllText(launcherTemplateDestinationPath);
            if (!templateGradleText.Contains("**TTPGradleTemplate**"))
            {
                Debug.LogWarning("TTPGradlePreProcess::OnPreprocessBuild:original launcherTemplate saved in launcherTemplate.gradle.bak");
                File.Copy(launcherTemplateDestinationPath, launcherTemplateDestinationPath + ".bak", true);
            }
        }

        if (File.Exists(mainTemplateDestinationPath))
        {
            string templateGradleText = File.ReadAllText(mainTemplateDestinationPath);
            if (!templateGradleText.Contains("**TTPGradleTemplate**"))
            {
                Debug.LogWarning("TTPGradlePreProcess::OnPreprocessBuild:original mainTemplate saved in mainTemplate.gradle.bak");
                File.Copy(mainTemplateDestinationPath, mainTemplateDestinationPath + ".bak", true);
            }
        }

        Debug.Log("TTPGradlePreProcess::OnPreprocessBuild:copy launcherTemplate from " + launcherTemplateSourcePath + " to " + launcherTemplateDestinationPath);
        File.Copy(launcherTemplateSourcePath, launcherTemplateDestinationPath, true);
        Debug.Log("TTPGradlePreProcess::OnPreprocessBuild:copy mainTemplate from " + mainTemplateSourcePath + " to " + mainTemplateDestinationPath);
        File.Copy(mainTemplateSourcePath, mainTemplateDestinationPath, true);
#endif
    }
}