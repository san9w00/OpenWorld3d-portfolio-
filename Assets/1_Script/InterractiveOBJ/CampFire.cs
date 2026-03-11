using UnityEngine;

public class CampFire : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject fireVFX;
    [SerializeField] private float healPerSecond = 10f;

    private PlayerStatus playerStatus;
    private InputHandler inputHandler;

    private bool isActivated = false;
    private bool isResting = false;

    private void Start()
    {
        fireVFX.SetActive(false);

        playerStatus = FindAnyObjectByType<PlayerStatus>();
        inputHandler = FindAnyObjectByType<InputHandler>();
    }

    private void Update()
    {
        if (isResting)
        {
            playerStatus.Heal(healPerSecond * Time.deltaTime);
        }
    }

    public void Interact()
    {
        if (!isActivated)
        {
            Activate();
            return;
        }
        
        if (!isResting)
        {
            StartRest();
        }
        else
        {
            StopRest();
        }
    }

    void Activate()
    {
        isActivated = true;
        fireVFX?.SetActive(true);

        Debug.Log("Ä·ÇÁÆÄÀÌ¾î È°¼ºÈ­!");
    }

    void StartRest()
    {
        isResting = true;
        inputHandler.SetInputEnabled(false);
        GameEvents.Reset();

        Debug.Log("ÈÞ½Ä ½ÃÀÛ");        
    }

    void StopRest()
    {
        isResting = false;
        inputHandler.SetInputEnabled(true);

        Debug.Log("ÈÞ½Ä Á¾·á");
    }

    public bool CanInteract() => true;  

    public string GetInteractText()
    {

        if (!isActivated)
            return "Active the CampFire";

        if (isResting)
            return "Out Rest";

        return "Rest & Reset Monsters";
    }
}
