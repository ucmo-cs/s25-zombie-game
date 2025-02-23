using UnityEngine;

public class Script_Saloon : MonoBehaviour
{
    [SerializeField] GameObject saloonDoor;
    [SerializeField] GameObject kickoutPoint;
    private bool playerInSaloon = false;

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
