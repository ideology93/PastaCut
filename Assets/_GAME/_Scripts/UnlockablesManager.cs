using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Reflection;

namespace NLO
{
    public class UnlockablesManager : MonoBehaviour
    {

        public Unlockables unlockables;
        public Sprite lockedIcon;
        public Button[] buttons;
        public Sprite[] images;
        public string unlockablesPath;
        public GameObject itemToUnlock;
        [SerializeField] GameObject itemUnlockUI;
        [SerializeField] GameFlow flow;
        private List<bool> bools = new List<bool>();
        int playerLevel;
        [SerializeField] GameObject endUI;
        private void Start()
        {

            playerLevel = PlayerPrefs.GetInt("level");
            Debug.Log("current playerlevel: " + playerLevel);
            unlockablesPath = $"{Application.persistentDataPath}/Unlockables.json";
            if (File.Exists(unlockablesPath))
            {
                string json = File.ReadAllText(unlockablesPath);
                unlockables = JsonUtility.FromJson<Unlockables>(json);
            }
            else
                Debug.Log("Doesn't Exist");
            CheckBools();
            RerenderShop();

        }
        private void CheckBools()
        {

            foreach (var property in unlockables.GetType().GetFields())
            {
                if ((property.GetValue(unlockables).ToString().ToLower() == "false"))
                {
                    bools.Add(false);
                }
                else
                    bools.Add(true);



            }
            foreach (var t in bools)
            {
                Debug.Log(t);
            }
        }

        private void RerenderShop()
        {
            for (int i = 0; i < bools.Count; i++)
            {
                if (bools[i])
                {
                    buttons[i].interactable = true;
                    buttons[i].GetComponent<Image>().sprite = images[i];
                }
                else
                {
                    buttons[i].interactable = false;
                    buttons[i].GetComponent<Image>().sprite = lockedIcon;
                }


            }
        }
        private void SaveToJson()
        {
            Debug.Log("Here");
            string json = JsonUtility.ToJson(unlockables);
            Debug.Log("Here2");
            File.WriteAllText(unlockablesPath, json);
            Debug.Log("Here3");
        }
        public void LevelUp()
        {
            if (playerLevel <= 4)
            {


                PlayerPrefs.SetInt("level", playerLevel);
                int i = 0;
                foreach (var property in unlockables.GetType().GetFields())
                {
                    if (playerLevel > 4)
                        return;
                    property.SetValue(unlockables, true);
                    if (i == playerLevel)
                    {
                        break;
                    }
                    i++;


                }
                foreach (var property in unlockables.GetType().GetFields())
                {
                    Debug.Log("Name: " + property.Name + " Value: " + property.GetValue(unlockables));
                }

                SaveToJson();
            }
        }
        public void SetItemToUnlock()
        {

            if (playerLevel < 4)
            {
                itemUnlockUI.SetActive(true);
                playerLevel++;
                itemToUnlock.GetComponent<Image>().sprite = images[playerLevel];
            }
            else
            {
                
                flow.EndPhaseFour();
                endUI.SetActive(true);
                
            }

        }


    }
}
