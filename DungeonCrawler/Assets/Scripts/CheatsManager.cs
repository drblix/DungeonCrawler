using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CheatsManager : MonoBehaviour
{   
    private Player player;
    private RoomGeneration roomGeneration;

    [SerializeField]
    private Transform consoleBarTrans;

    [SerializeField]
    private TMP_InputField consoleBar;

    [SerializeField]
    private TextMeshProUGUI consoleText;

    private bool consoleOpen = false;

    private readonly Vector3 openPos = new Vector3(200f, -211f, 0f);
    private readonly Vector3 closePos = new Vector3(200f, -243f, 0f);

    private enum NotiType
    {
        Success,
        Error
    }

    private void Awake() 
    {
        player = FindObjectOfType<Player>();
        roomGeneration = FindObjectOfType<RoomGeneration>();

        consoleBarTrans.localPosition = closePos;
    }

    private void Update() 
    {
        // Switch back to 'PlayerSettings.cheatEnabled' when done debugging
        if (Input.GetKeyDown(KeyCode.BackQuote) && PlayerSettings.cheatsEnabled) 
        {
            if (roomGeneration != null)
            {
                if (!roomGeneration.DoneLoading) { return; }
            }

            if (!consoleOpen)
            {
                consoleBarTrans.LeanMoveLocal(openPos, .2f);
                consoleBar.interactable = true;
            }
            else
            {
                consoleBarTrans.LeanMoveLocal(closePos, .2f);
                consoleBar.interactable = false;
            }

            consoleOpen = !consoleOpen;
            player.ToggleEnabled(!consoleOpen);
            consoleBar.text = null;
        }
    }

    public void CommandEntered()
    {
        string command = consoleBar.text.ToLower();
        consoleBar.text = null;

        if (string.IsNullOrEmpty(command) || !consoleBar.interactable) { return; }

        // Checks for type of command

        if (command.Contains("coins"))
        {
            string[] wrds = command.Split(' ');

            try
            {
                if (int.TryParse(wrds[1], out int result))
                {
                    FindObjectOfType<CoinManagement>().AddCoins(result);

                    StartCoroutine(MessageToConsole("Added " + result.ToString() + " coins!", NotiType.Success));
                }
                else
                {
                    StartCoroutine(MessageToConsole("Value could not be parsed to integer", NotiType.Error));
                }
            }
            catch (System.Exception)
            {
                StartCoroutine(MessageToConsole("ERR", NotiType.Error));
            }

            return;
        }

        if (command.Contains("loadscene"))
        {
            string[] wrds = command.Split(' ');

            try
            {
                if (int.TryParse(wrds[1], out int result))
                {
                    string newScene = SceneUtility.GetScenePathByBuildIndex(result);

                    if (!string.IsNullOrEmpty(newScene))
                    {
                        SceneManager.LoadScene(result);
                    }
                    else
                    {
                        StartCoroutine(MessageToConsole("Invalid scene", NotiType.Error));
                    }
                }
                else
                {
                    StartCoroutine(MessageToConsole("Value could not be parsed to integer", NotiType.Error));
                }
            }
            catch (System.Exception)
            {
                StartCoroutine(MessageToConsole("ERR", NotiType.Error));
            }

            return;
        }

        if (command.Contains("firerate"))
        {
            string[] wrds = command.Split(' ');

            try
            {
                if (float.TryParse(wrds[1], out float result))
                {
                    Player.shootCooldown = Mathf.Abs(result);

                    StartCoroutine(MessageToConsole("Changed firerate to " + Mathf.Abs(result).ToString(), NotiType.Success));
                }
                else
                {
                    StartCoroutine(MessageToConsole("Value could not be parsed to float", NotiType.Error));
                }
            }
            catch (System.Exception)
            {
                StartCoroutine(MessageToConsole("ERR", NotiType.Error));
            }

            return;
        }

        if (command.Contains("speed"))
        {
            string[] wrds = command.Split(' ');

            try
            {
                if (float.TryParse(wrds[1], out float result))
                {
                    FindObjectOfType<Player>().SetPlayerSpeed(result);

                    StartCoroutine(MessageToConsole("Set " + result.ToString() + " to player speed", NotiType.Success));
                }
                else
                {
                    StartCoroutine(MessageToConsole("Value could not be parsed to float", NotiType.Error));
                }
            }
            catch (System.Exception)
            {
                StartCoroutine(MessageToConsole("ERR", NotiType.Error));
            }

            return;
        }

        if (command == "godmode")
        {
            if (!PlayerHealth.godMode)
            {
                PlayerHealth.godMode = true;
                StartCoroutine(MessageToConsole("Godmode enabled", NotiType.Success));
            }
            else
            {
                PlayerHealth.godMode = false;
                StartCoroutine(MessageToConsole("Godmode disabled", NotiType.Success));
            }

            return;
        }

        if (command == "unlimitedpowa")
        {
            if (!PlayerMana.unlimitedMana)
            {
                PlayerMana.unlimitedMana = true;
                StartCoroutine(MessageToConsole("Unlimited mana enabled", NotiType.Success));
            }
            else
            {
                PlayerMana.unlimitedMana = false;
                StartCoroutine(MessageToConsole("Unlimited mana disabled", NotiType.Success));
            }

            return;
        }

        if (command == "robbery")
        {
            if (!ShopManagement.freeShopItems)
            {
                ShopManagement.freeShopItems = true;
                StartCoroutine(MessageToConsole("Wow :(", NotiType.Success));
            }
            else
            {
                ShopManagement.freeShopItems = false;
                StartCoroutine(MessageToConsole("Look at you being a law-abiding citizen", NotiType.Success));
            }

            return;
        }

        if (command == "noclip")
        {
            if (!Player.noclipEnabled)
            {
                player.ToggleNoclip(true);
                StartCoroutine(MessageToConsole("Noclip enabled", NotiType.Success));
            }
            else
            {
                player.ToggleNoclip(false);
                StartCoroutine(MessageToConsole("Noclip disabled", NotiType.Error));
            }

            return;
        }

        StartCoroutine(MessageToConsole("Invalid command", NotiType.Error));
    }

    private IEnumerator MessageToConsole(string message, NotiType type)
    {
        consoleBar.interactable = false;
        consoleBar.text = message;

        if (type == NotiType.Error)
        {
            consoleText.color = Color.red;
        }

        yield return new WaitForSeconds(1.5f);

        consoleBar.text = null;
        consoleText.color = Color.green;

        if (consoleOpen)
        {
            consoleBar.interactable = true;
        }
    }
}
