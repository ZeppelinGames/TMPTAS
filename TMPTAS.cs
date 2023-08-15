using System;

using MelonLoader;
using TMPTAS;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[assembly: MelonInfo(typeof(TMPTAS.TMPTAS), "TMPTAS", "1.0.0", "Zeppelin")]

namespace TMPTAS
{
    public struct TimeSlowKey
    {
        public KeyCode key;
        public float slowSpeed;

        public TimeSlowKey(KeyCode key, float slowSpeed)
        {
            this.key = key;
            this.slowSpeed = slowSpeed;
        }
    }

    public class TMPTAS : MelonMod
    {
        // setup time controls
        private TimeSlowKey[] timeSlowKeys =
        {
            new TimeSlowKey(KeyCode.Keypad1, 0.5f),
            new TimeSlowKey(KeyCode.Keypad2, 0.25f),
            new TimeSlowKey(KeyCode.Keypad3, 0.1f),
        };
        private bool timeSlowed;
        private bool timeFrozen;

        private bool frameStep;

        private PlatformerCharacter2D player;
        private Platformer2DUserControl playerControl;
        private Rigidbody2D playerRb;
        private Collider2D playerCol;

        private bool showDebugInfo;

        private Text playerPositionText;

        private DebugLine movementDebugLine = new DebugLine();
        private DebugLine groundDebugLine = new DebugLine();

        private Material lineMaterial;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);

            lineMaterial = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));

            // get player
            TryGetPlayer();

            // add recording icon to loaded scene

            // add text to scene for info
            Canvas canvas = new GameObject().AddComponent<Canvas>();
            playerPositionText = new GameObject().AddComponent<Text>();

            movementDebugLine.CreateLine(lineMaterial);
            groundDebugLine.CreateLine(lineMaterial);
            groundDebugLine.SetColor(Color.red);
        }

        public void TryGetPlayer()
        {
            if (player != null)
            {
                return;
            }

            LoggerInstance.Msg("Getting player");
            GameObject playerGO = GameObject.FindWithTag("Player");
            if (playerGO != null)
            {
                Component[] components = playerGO.GetComponents<Component>();
                for (int i = 0; i < components.Length; i++)
                {
                    LoggerInstance.Msg(components[i].GetType().Name);
                }
                player = playerGO.GetComponent<PlatformerCharacter2D>();
                playerControl = playerGO.GetComponent<Platformer2DUserControl>();
                if (player != null)
                {
                    playerRb = player.GetComponent<Rigidbody2D>();
                    playerCol = player.GetComponent<Collider2D>();
                }
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }

        float horizontalVal = 0;
        TMPCustInput custInput;
        public override void OnUpdate()
        {
            base.OnUpdate();
            TryGetPlayer();

            if (player != null)
            {
                playerPositionText.text = $"{player.transform.position.x}, {player.transform.position.y}";

                // 57 Platformer2DUserControl
                if (custInput == null)
                {
                    custInput = new TMPCustInput();
                    custInput.Player.Enable();
                }

                float target = custInput.Player.Horizontal.ReadValue<float>();
                horizontalVal = Mathf.MoveTowards(horizontalVal, target, playerControl.horizontalSmooth * Time.deltaTime);
                movementDebugLine.SetPositions(player.transform.position, (Vector2)player.transform.position + (Vector2.right * horizontalVal));
            }

            // do input checks for time control inputs (numpad)

            // Instant change scenes
            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                int loadScene = GameControl.control.currentScene - 1;
                loadScene = loadScene < 1 ? 1 : loadScene;
                GameControl.control.newSceneNumber = loadScene;
                GameControl.control.afterFade();
            }

            if(Input.GetKeyDown(KeyCode.Keypad6))
            {
                int loadScene = GameControl.control.currentScene + 1;
                loadScene = loadScene > SceneManager.sceneCountInBuildSettings ? SceneManager.sceneCountInBuildSettings : loadScene;
                GameControl.control.newSceneNumber = loadScene;
                GameControl.control.afterFade();
            }

            // pause/resume time
            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                timeFrozen = !timeFrozen;
                Time.timeScale = timeFrozen ? 0 : 1;
            }

            //Step (update) frame
            if (frameStep)
            {
                Time.timeScale = 0;
                frameStep = false;
                timeFrozen = true;
            }
            if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                frameStep = true;
                timeFrozen = false;
                Time.timeScale = 1;
            }

            // slow time
            for (int i = 0; i < timeSlowKeys.Length; i++)
            {
                if (Input.GetKeyDown(timeSlowKeys[i].key))
                {
                    float slowSpeed = timeSlowKeys[i].slowSpeed;
                    timeSlowed = (Time.timeScale != slowSpeed) ? true : !timeSlowed;
                    Time.timeScale = (Time.timeScale != slowSpeed) ? slowSpeed : (timeSlowed ? slowSpeed : 1f);
                }
            }
        }
    }
}
