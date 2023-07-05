
public class Car
// Define class variables
{
    public int Id { get; set; }

    public string Model { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public string Gear { get; set; }

    public ICollection<Part> Parts { get; set; }


}