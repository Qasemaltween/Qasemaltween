

public class Sale
{
    // Define class variables
    public int Id { get; set; }

    public decimal Total { get; set; }

    public int CarId { get; set; }

    public Car Car { get; set; }

    public int CustomerId { get; set; }

    public Customer Customer { get; set; }

}