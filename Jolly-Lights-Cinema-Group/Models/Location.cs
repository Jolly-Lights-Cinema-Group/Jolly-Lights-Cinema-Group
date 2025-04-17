public class Location
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }

    public Location(string name, string address)
    {
        Name = name;
        Address = address;
    }

    public Location(int id, string name, string address)
        : this(name, address)
    {
        Id = id;
    }
}
