using UnityEngine;

[CreateAssetMenu(fileName = "Item")]
public class Item : ScriptableObject {
	
	public string ID;
	public string Description;
	public Sprite sprite;
}