using UnityEngine;

public class LevelDoor : MonoBehaviour, IInteract
{
    [SerializeField] private bool startOpen;
    [SerializeField] private float openSpeed;
    [SerializeField] private Transform door;
    [SerializeField] private Transform closePosition;
    [SerializeField] private Transform openPosition;

    private BoxCollider collider;

    private bool open;

    private void Start()
    {
        collider = GetComponent<BoxCollider>();
        open = startOpen;

        if (open)
        {
            door.position = openPosition.position;
            collider.center = openPosition.localPosition;
        }
        else
        {
            door.position = closePosition.position;
            collider.center = closePosition.localPosition;
        }
    }

    private void Update()
    {
        if (open)
        {
            door.position = Vector3.Slerp(door.position, openPosition.position, openSpeed);
            collider.center = Vector3.Slerp(collider.center, openPosition.localPosition, openSpeed);
        }
        else
        {
            door.position = Vector3.Slerp(door.position, closePosition.position, openSpeed);
            collider.center = Vector3.Slerp(collider.center, closePosition.localPosition, openSpeed);
        }
    }

    private void ToggleDoor()
    {
        open = !open;
    }

    public string GetItemName()
    {
        return null;
    }

    public void OnHover()
    {
        
    }

    public void OnInteract(GameObject inventory)
    {
        ToggleDoor();
    }
}
