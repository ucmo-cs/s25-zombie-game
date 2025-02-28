using UnityEngine;

public class Script_Saloon : MonoBehaviour
{
    [SerializeField] GameObject saloonDoor;
    [SerializeField] GameObject kickoutPoint;
    private bool playerInSaloon = false;

    [SerializeField] Script_BarMenu barMenu;
    [SerializeField] Script_ScrapMenu scrapMenu;
    private Script_Bar bar;
    private Script_Mechanic mechanic;

    void Start()
    {
        bar = GetComponentInChildren<Script_Bar>();
        mechanic = GetComponentInChildren<Script_Mechanic>();
    }

    public void DoorToggle(bool open){
        if (open){
            saloonDoor.SetActive(false);
        }
        else{
            saloonDoor.SetActive(true);
        }
    }

    public void KickOutPlayer(){
        if (playerInSaloon){
            Debug.Log("Kicking player out");
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = kickoutPoint.transform.position;
            player.GetComponent<CharacterController>().enabled = true;
            playerInSaloon = false;
            bar.SetPlayerIsAtBar(false);
            bar.DisablePrompt();
            mechanic.SetPlayerIsAtMech(false);
            mechanic.DisablePrompt();
            if (barMenu.isActiveAndEnabled)
                barMenu.Close();
            if (scrapMenu.isActiveAndEnabled)
                scrapMenu.Close();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player"){
            playerInSaloon = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player"){
            playerInSaloon = false;
        }
    }
}
