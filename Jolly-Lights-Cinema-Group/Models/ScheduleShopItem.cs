public class ScheduleShopItem
{
    public int? Id { get; set; }
    public int ShopItemId { get; set; }
    public int ReservationId { get; set; }

    public ScheduleShopItem(int shopItemId, int reservationId)
    {
        ShopItemId = shopItemId;
        ReservationId = reservationId;
    }

    public ScheduleShopItem(int id, int shopItemId, int reservationId)
        : this(shopItemId, reservationId)
    {
        Id = id;
    }
}
