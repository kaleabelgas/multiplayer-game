using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles shop interface to player object and inventory.
/// </summary>
public class ShopEntity : MonoBehaviour
{
    [SerializeField] private InputListener inputListener;
    [SerializeField] private CurrencySystem currencySystem;
    [SerializeField] private ShopUI shopUI;

    private bool _playerNearby;
    private bool _shopIsOpen;
    private GameObject _player;

    private void Awake()
    {
        shopUI = FindObjectOfType<ShopUI>();
        inputListener.OnInteract += OpenShopMenu;
        inputListener.OnEscape += CloseShopMenu;
    }

    private void OpenShopMenu()
    {
        if (!_playerNearby) return;
        if (_shopIsOpen) return;

        Debug.Log("Shop Opened!");
        _shopIsOpen = true;

        inputListener.EnableInput = false;

        shopUI.SetupShop(currencySystem.GetBalance(_player), this);
    }

    private void CloseShopMenu()
    {
        shopUI.CloseShopUI();
        _shopIsOpen = false;
        inputListener.EnableInput = true;
    }

    /// <summary>
    /// Adds item to player inventory and subtracts the price from player balance.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="price"></param>
    public void TransferItemToInventory(Item item, int price)
    {
        currencySystem.RemoveFromBalance(_player, price);
        Debug.Log($"Bought {item.ItemName} for {price!} Remaining Balance: {currencySystem.GetBalance(_player)}");
        _player.GetComponent<InventoryController>().AddToInventory(item);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerNearby = true;
            _player = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerNearby = false;
            _player = null;
        }
    }
}
