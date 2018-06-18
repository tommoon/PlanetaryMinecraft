using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventory : MonoBehaviour {

	public GameObject slotPrefab;
	public RectTransform Hotbar;
	public RectTransform InventoryCase;

	public Sprite nullSprite;

	int hotbarnum;
	int itemNum;

	bool invActive = false;

	public Item[] items;
	public GameObject[] slots;
	public Item nullItem;

	// Use this for initialization
	void Start () {
		

		hotbarnum = 3;

		InventoryCase.gameObject.SetActive (false);

		itemNum = hotbarnum + (hotbarnum * hotbarnum);
		Debug.Log (itemNum);
		items = new Item[itemNum];
		slots = new GameObject[itemNum];

		initiateSlots (hotbarnum,hotbarnum*hotbarnum);
	}

	void initiateSlots (int hotNum, int invNum) {
		for (int i = 0; i < hotNum; i++) {
			int val = i;
			GameObject invslot = Instantiate<GameObject> (slotPrefab, transform.position,Quaternion.identity);
			slots [i] = invslot.GetComponentInChildren<Slot> ().gameObject;
			invslot.transform.SetParent (Hotbar);
			invslot.name = "hotbar " + i;
			if(items[i] == null){
				items [i] = nullItem;
			}
			slots [i].GetComponent<Slot>().assignedTo = items [i];
		}

		for (int i = 0; i < invNum; i++) {
			int num = hotNum + i;
			GameObject invslot = Instantiate<GameObject> (slotPrefab, transform.position,Quaternion.identity);
			invslot.transform.SetParent (InventoryCase);
			invslot.name = "Inventory " + num;
			if(items[num] == null)
				items [num] = nullItem;
			slots [num] = invslot.GetComponentInChildren<Slot> ().gameObject;
			slots [num].GetComponent<Slot> ().assignedTo = items [num];
			slots [num].name = "slot " + num; 
		}

		for (int i = 0; i < itemNum; i++) {
			slots [i].GetComponent<Slot> ().slot = i;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.I)) {
			if (invActive) {
				Time.timeScale = 1;
				InventoryCase.gameObject.SetActive (false);
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				invActive = false;
				GetComponent<Image> ().enabled = false;

			} else if (!invActive) {
				Time.timeScale = 0;
				InventoryCase.gameObject.SetActive (true);
				Cursor.lockState = CursorLockMode.Confined;
				Cursor.visible = true;
				invActive = true;
				GetComponent<Image> ().enabled = true;

			}
		}
	}

	public void AddItem(Item itemToAdd)
	{
		for (int i = 0; i < items.Length; i++)
		{
			if (items[i] == nullItem)
			{
				items[i] = itemToAdd;
				slots [i].GetComponent<Slot> ().assignedTo = itemToAdd;
				slots [i].GetComponent<Image> ().sprite = itemToAdd.sprite;
				slots [i].transform.parent.gameObject.GetComponent<Image> ().enabled = true;
				return;
			}
		}
	}

	public void AddItematPoint(Item itemToAdd, int pos)
	{
			items[pos] = itemToAdd;
			slots [pos].GetComponent<Slot> ().assignedTo = itemToAdd;
			slots [pos].GetComponent<Image> ().sprite = itemToAdd.sprite;
			slots [pos].transform.parent.gameObject.GetComponent<Image> ().enabled = true;
			return;
	}

	public void RemoveItematPoint (Item itemToRemove, int pos)
	{
		items[pos] = nullItem;
		slots [pos].GetComponent<Slot> ().assignedTo = nullItem;
		slots [pos].GetComponent<Image> ().sprite = nullSprite;
		slots [pos].transform.parent.gameObject.GetComponent<Image> ().enabled = false;
			return;
	}

	public void RemoveItem (Item itemToRemove)
	{
		for (int i = 0; i < items.Length; i++)
		{
			if (items[i] == itemToRemove)
			{
				items[i] = nullItem;
				slots [i].GetComponent<Slot> ().assignedTo = nullItem;
				slots [i].GetComponent<Image> ().sprite = nullSprite;
				slots [i].transform.parent.gameObject.GetComponent<Image> ().enabled = false;
				return;
			}
		}
	}
}
