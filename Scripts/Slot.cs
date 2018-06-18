using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {


	public Item assignedTo;
	public iconHolder holder;
	inventory inv;
	public int slot;

	public GameObject descBox;
	public Text desc;

	public void OnPointerDown(PointerEventData eventData)
	{
		holder = GameObject.FindGameObjectWithTag ("InventoryCursor").GetComponent<iconHolder> ();
		inv = GameObject.FindGameObjectWithTag ("Inventory").GetComponent<inventory> ();
		descBox = holder.descBox;
		desc = holder.desc;

		if(!holder.holding){
			if (assignedTo.name != "nullItem") {
				holder.held = assignedTo;
				holder.holding = true;
				holder.GetComponent<Image> ().enabled = true;
				holder.GetComponent<Image> ().sprite = assignedTo.sprite;
				inv.RemoveItematPoint (assignedTo,slot);
			}
		} else if (holder.holding) {
			if (assignedTo.name != "nullItem") {
				inv.AddItem (holder.held);
				holder.held = assignedTo;
				holder.holding = false;
				holder.GetComponent<Image> ().enabled = true;
				holder.GetComponent<Image> ().sprite = assignedTo.sprite;

			} else if(assignedTo.name == "nullItem") {
				inv.AddItematPoint (holder.held,slot);
				holder.held = assignedTo;
				holder.holding = false;
				holder.GetComponent<Image> ().enabled = false;
				holder.GetComponent<Image> ().sprite = null;
			}
		}
	}

	public void OnPointerEnter(PointerEventData dataName)
	{
		holder = GameObject.FindGameObjectWithTag ("InventoryCursor").GetComponent<iconHolder> ();
		inv = GameObject.FindGameObjectWithTag ("Inventory").GetComponent<inventory> ();
		descBox = holder.descBox;
		desc = holder.desc;

		if (assignedTo.name != "nullItem") {

			descBox.SetActive (true);
			desc.text = assignedTo.Description;
		}
	}

	public void OnPointerExit(PointerEventData dataName)
	{
		holder = GameObject.FindGameObjectWithTag ("InventoryCursor").GetComponent<iconHolder> ();
		inv = GameObject.FindGameObjectWithTag ("Inventory").GetComponent<inventory> ();
		descBox = holder.descBox;
		desc = holder.desc;

		if (assignedTo.name != "nullItem") {

			descBox.SetActive (false);
			desc.text = null;
		}
	}
}
