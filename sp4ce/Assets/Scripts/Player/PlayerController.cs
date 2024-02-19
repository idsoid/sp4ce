using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IHealth
{
    [SerializeField]
    private List<GameObject> inventory;

    [SerializeField]
    private GameObject inventoryObject;

    [SerializeField]
    private GameObject sightController;

    
    private int equippedIndex;
    private FlashlightItem flashlight;

    void Start()
    {
        InitHealth();
        GetInventory();

        equippedIndex = 0;
        foreach(GameObject obj in inventory)
        {
            obj.SetActive(false);
        }
        inventory[equippedIndex].SetActive(true);
    }

    void Update()
    {
        sightController.transform.position = transform.position;

        if(GameManager.instance.isInUI) return;

        if(canUse) {
            if(Input.GetMouseButtonDown(0))
            {
                inventory[equippedIndex].GetComponent<IItem>().OnPrimaryAction();

                //check in case for consumable
                if(inventory[equippedIndex].transform.parent == null)
                {
                    GetInventory();
                    SwapItem(false, true);
                }
            }
            if(Input.GetMouseButtonUp(0))
            {
                inventory[equippedIndex].GetComponent<IItem>().OnPrimaryActionRelease();
            }
            if(Input.GetMouseButtonDown(1))
            {
                inventory[equippedIndex].GetComponent<IItem>().OnSecondaryAction();
            }
            if(Input.GetMouseButtonUp(1))
        {
            inventory[equippedIndex].GetComponent<IItem>().OnSecondaryActionRelease();
        }
            if(Input.GetKeyDown(KeyCode.F))
            {
                flashlight.OnPrimaryAction();
            }
            foreach(GameObject obj in inventory)
            {
                if(obj.activeInHierarchy)
                    obj.GetComponent<IItem>()?.RunBackgroundProcesses();
            }
        }

        //weapon swapping
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            SwapItem(true, false);
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            SwapItem(false, false);
        }
    
        //TODO: remove pls. temporary
        if(Input.GetKeyDown(KeyCode.H))
        {
            GameManager.instance.Die();
        }
    }

    void GetInventory()
    {
        inventory.Clear();
        //get inventory
        foreach(Transform child in inventoryObject.transform)
        {
            child.GetComponent<IItem>().GetRequiredControllers(gameObject, sightController);
            inventory.Add(child.gameObject);

            FlashlightItem fs = child.GetComponent<FlashlightItem>();
            if(fs!=null) flashlight=fs;
        }
    }

    void SwapItem(bool front, bool isConsumed)
    {
        if(!isConsumed)
        {
            if(inventory[equippedIndex].GetComponent<IItem>().IsItemInUse())
            {
                return;
            }
            inventory[equippedIndex].SetActive(false);
        }
        if(front)
        {
            equippedIndex = (equippedIndex + 1) % inventory.Count;
        }
        else
        {
            equippedIndex = (equippedIndex + inventory.Count - 1) % inventory.Count;
        }
        inventory[equippedIndex].SetActive(true);
        inventory[equippedIndex].transform.position = transform.position + Camera.main.transform.right;
    }

    [SerializeField]
    private int maxHealth = 100;

    private int health;

    public int GetHealth()
    {
        return health;
    }
    public void UpdateHealth(int amt)
    {
        health += amt;
        if(health > maxHealth) health = maxHealth;
        else if (health <= 0)
        {
            health = 0;
            Color nigga = Color.black;
            Die();
        }
    }

    public void Die()
    {
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        GameManager.instance.isInUI = true;
        Camera.main.transform.parent = GameManager.instance.lastHitEnemy.transform;
        Camera.main.transform.localRotation = Quaternion.identity;
        Camera.main.transform.localPosition = Vector3.zero;
        yield return new WaitForSeconds(0.5f);
        Debug.Log("im dead");
        UIManager.instance.OnDie();
        GameManager.instance.Die();
    }

    public void InitHealth()
    {
        health = maxHealth;
    }

    bool canUse = true;
    public void DisableEquipment()
    {
        canUse = false;
        inventory[equippedIndex].GetComponent<IItem>()?.OnPrimaryActionRelease();
        inventory[equippedIndex].GetComponent<IItem>()?.OnSecondaryActionRelease();
        foreach(GameObject obj in inventory)
        {
            obj.GetComponent<IItem>()?.OnEMPTrigger();
        }
    }
}
