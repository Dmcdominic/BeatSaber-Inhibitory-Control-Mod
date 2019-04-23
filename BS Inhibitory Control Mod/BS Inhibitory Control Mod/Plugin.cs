using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using IllusionPlugin;
using System.Collections;
using System.Reflection;

namespace bs_flipped
{
    public class Plugin : IPlugin
    {
        public string Name => "Plugin Name";
        public string Version => "0.0.1";

        bool doesPluginExist;

        HashSet<int> sigma_direction;
        HashSet<int> sigma_color;

        bool flip_direction;
        bool flip_color;

        public void OnApplicationStart()
        {
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            //Checks if a IPlugin with the name in quotes exists, in case you want to verify a plugin exists before trying to reference it, or change how you do things based on if a plugin is present
            doesPluginExist = IllusionInjector.PluginManager.Plugins.Any(x => x.Name == "Saber Mod");


        }

        private void SceneManagerOnActiveSceneChanged(Scene oldScene, Scene newScene)
        {

            if (newScene.name == "Menu")
            {
                //Code to execute when entering The Menu


            }

            if (newScene.name == "GameCore")
            {
                //Code to execute when entering actual gameplay


            }


        }

        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            //Create GameplayOptions/SettingsUI if using either


        }

        public void OnApplicationQuit()
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        public void OnLevelWasLoaded(int level)
        {

        }

        public void OnLevelWasInitialized(int level)
        {
        }

        public void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                flip_color = !flip_color;

            if (Input.GetKeyDown(KeyCode.P))
                flip_direction = !flip_direction;
            var notes = UnityEngine.Object.FindObjectsOfType(typeof(GameNoteController));

            foreach(GameNoteController gnc in notes)
            {

                if(flip_color && !sigma_color.Contains(gnc.GetInstanceID()))
                {
                    gnc.noteData.SwitchNoteType();
                }

                if(flip_direction && !sigma_direction.Contains(gnc.GetInstanceID()))
                {
                    NoteCutDirection ncd = NoteCutDirection.Any;
                    switch (gnc.noteData.cutDirection)
                    {
                        case NoteCutDirection.Up:
                            ncd = NoteCutDirection.Down;
                            break;
                        case NoteCutDirection.Down:
                            ncd = NoteCutDirection.Up;
                            break;
                        case NoteCutDirection.Left:
                            ncd = NoteCutDirection.Right;
                            break;
                        case NoteCutDirection.Right:
                            ncd = NoteCutDirection.Left;
                            break;
                        case NoteCutDirection.UpLeft:
                            ncd = NoteCutDirection.DownRight;
                            break;
                        case NoteCutDirection.UpRight:
                            ncd = NoteCutDirection.DownLeft;
                            break;
                        case NoteCutDirection.DownLeft:
                            ncd = NoteCutDirection.UpRight;
                            break;
                        case NoteCutDirection.DownRight:
                            ncd = NoteCutDirection.UpLeft;
                            break;
                        case NoteCutDirection.Any:
                            ncd = NoteCutDirection.None;
                            break;
                        case NoteCutDirection.None:
                            ncd = NoteCutDirection.Any;
                            break;
                    }
                    gnc.noteData.GetType().GetField("cutDirection", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(gnc.noteData, ncd);
                    sigma_direction.Add(gnc.GetInstanceID());
                }
            }

        }

        public void OnFixedUpdate()
        {
        }
    }
}
