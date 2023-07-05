


public class Customer
{
    // Define class variables
    public int Id { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public ICollection<Sale> Sales { get; set; }

}