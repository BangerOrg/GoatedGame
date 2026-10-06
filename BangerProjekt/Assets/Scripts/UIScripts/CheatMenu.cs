using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheatMenu : MonoBehaviour
{
	[field: SerializeField] private AllItemsSO itemlist;
	[field: SerializeField] private TMP_InputField inputField;
	[field: SerializeField] private TMP_Text textLog;
	[SerializeField] private ScrollRect scrollRect;

	void OnEnable()
	{
		inputField.onSubmit.AddListener(ProcessCommand);
	}
	void OnDisable()
	{
		inputField.onSubmit.RemoveListener(ProcessCommand);
	}
	public void ProcessCommand(string input)
	{
		if (string.IsNullOrWhiteSpace(input)) return;
		if (!itemlist) { Log("No Item List found"); }
		Log($"\n> {input}");
		string[] args = input.Trim().Split(' ');
		string command = args[0].ToLower();

		switch (command)
		{
			case "give":
				HandleGiveItem(args);
				break;

			case "help":
				Log("Commands:\n - give <itemName>\n - clear\n -items");
				break;

			case "items":
				Log("Available Items:");
				foreach (Item item in itemlist.Items)
				{
					if (item != null) Log($"  - {item.name}");
				}
				break;

			case "clear":
				textLog.text = "";
				break;

			default:
				Log($"<color=red>Unknown command: '{command}'. Type 'help' for commands.</color>");
				break;
		}

		inputField.text = ""; //Clear input field
		inputField.ActivateInputField(); //Keep focus active for next command
	}

	private void Log(string message)
	{
		textLog.text += message + "\n";
		Canvas.ForceUpdateCanvases();
		StartCoroutine(ScrollToBottomNextFrame());
	}
	private void HandleGiveItem(string[] args)
	{
		if (args.Length < 2)
		{
			Log("<color=yellow>Usage: give <itemName></color>");
			return;
		}

		string targetName = args[1].ToLower();
		Item foundItem = itemlist.Items.Find(i => i != null && i.name.ToLower() == targetName);

		if (foundItem != null)
		{
			//Calling Inventory system to add the item directly
			if (InventoryLogic.ActiveInventory != null)
			{
				InventoryLogic.ActiveInventory.TryAddItem(foundItem);
				Log($"<color=green>Gave {foundItem.name}!</color>");
			}
			else
			{
				Log("<color=red>Inventory not assigned / existing!</color>");
			}
		}
		else
		{
			Log($"<color=red>Item '{targetName}' not found.</color>");
		}
	}

	private IEnumerator ScrollToBottomNextFrame()
	{
		yield return new WaitForEndOfFrame();
		if (scrollRect != null)
		{
			scrollRect.verticalNormalizedPosition = 0f;
		}
	}
}
