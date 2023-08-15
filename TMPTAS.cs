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

        private DebugLine[] colliderDebugLines;
        private Collider2D[] colliders;

        private Material lineMaterial;

        private Transform m_GroundCheck;
        private Transform    m_CeilingCheck;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);

            lineMaterial = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));

            // get player
            TryGetPlayer();

            // Get all scene colliders (probably a bad idea)
            Collider2D[] colliders = GameObject.FindObjectsOfType<Collider2D>();
            colliderDebugLines = new DebugLine[colliders.Length];
            UpdateDebugColliders();

                // add text to scene for info
                Canvas canvas = new GameObject().AddComponent<Canvas>();
            playerPositionText = new GameObject().AddComponent<Text>();

            movementDebugLine.CreateLine(lineMaterial);
            groundDebugLine.CreateLine(lineMaterial);
            groundDebugLine.SetColor(Color.red);

            m_GroundCheck = player.transform.Find("GroundCheck");
            m_CeilingCheck = player.transform.Find("CeilingCheck");
        }

        public void TryGetPlayer()
        {
            if (player != null)
            {
                return;
            }

            GameObject playerGO = GameObject.FindWithTag("Player");
            if (playerGO != null)
            {
                player = playerGO.GetComponent<PlatformerCharacter2D>();
                playerControl = playerGO.GetComponent<Platformer2DUserControl>();
                if (player != null)
                {
                    playerRb = player.GetComponent<Rigidbody2D>();
                    playerCol = player.GetComponent<Collider2D>();
                }
            }
        }

        private bool m_Grounded;
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            // 22 PlatformerCharacter2D
            m_Grounded = false;
            Collider2D[] array = Physics2D.OverlapCircleAll(m_GroundCheck.position, 0.2f, LayerMask.GetMask("Ground"));
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].gameObject != player.gameObject)
                {
                    m_Grounded = true;
                }
            }
        }

        float horizontalVal = 0;
        TMPCustInput custInput;
        public override void OnUpdate()
        {
            base.OnUpdate();
            TryGetPlayer();
            UpdateDebugColliders();

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

                // Show ground check
                groundDebugLine.SetColor(m_Grounded ? Color.green : Color.red);
                groundDebugLine.SetPositions(player.transform.position, m_GroundCheck.position);
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

        void UpdateDebugColliders()
        {
            if(colliders == null)
            {
                return;
            }

            for (int i = 0; i < colliderDebugLines.Length; i++)
            {
                colliderDebugLines[i] = new DebugLine();
                colliderDebugLines[i].CreateLine(lineMaterial);
                colliderDebugLines[i].SetColor(Color.blue);

                switch (colliders[i])
                {
                    case PolygonCollider2D poly:
                        colliderDebugLines[i].SetPositions(poly.points);
                        break;
                    case BoxCollider2D box:
                        Vector2[] points = new Vector2[]
                        {
                            box.transform.position + new Vector3(box.size.x/2f,-box.size.y / 2f),
                            box.transform.position + new Vector3(-(box.size.x/2f),-box.size.y / 2f),

                            box.transform.position + new Vector3(box.size.x/2f,box.size.y / 2f),
                            box.transform.position + new Vector3(-(box.size.x/2f),box.size.y / 2f),
                        };

                        colliderDebugLines[i].SetPositions(points);
                        break;
                }
            }
        }
    }
}
