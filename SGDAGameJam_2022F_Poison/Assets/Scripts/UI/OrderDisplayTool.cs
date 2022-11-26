using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderDisplayTool : MonoBehaviour
{
    [SerializeField] Ticket[] _tickets = null;
    [SerializeField] SpriteRenderer[] _ticketSprites = null;

    public Ticket[] Tickets
    {
        get => _tickets;
    }
    public SpriteRenderer[] TicketSprites
    {
        get => _ticketSprites;
    }

    public static OrderDisplayTool Instance = null;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void DisplayOrder(Order order, int index)
    {
        Instance.Tickets[index].gameObject.SetActive(true);
        Instance.TicketSprites[index].gameObject.SetActive(true);
        Instance.Tickets[index].DisplayOrder(order);
    }

    public static void HideTicket(int index)
    {
        Instance.Tickets[index].gameObject.SetActive(false);
        Instance.TicketSprites[index].gameObject.SetActive(false);
    }
}
