public class ShopItem
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int Stock { get; set; }
    public int MinimumAge { get; set; }

    public ShopItem(string name, double price, int stock, int minimumAge = 0)
    {
        Name = name;
        Price = price;
        Stock = stock;
        MinimumAge = minimumAge;
    }

    public ShopItem(int id, string name, double price, int stock, int minimumAge = 0)
        : this(name, price, stock, minimumAge)
    {
        Id = id;
    }
}
