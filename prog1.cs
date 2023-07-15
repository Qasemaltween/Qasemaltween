
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


//(Car ,Part, Supplier,Sale,Customer ) Model Classes
public class Car
{
    public int Id { get; set; }
    public string Model { get; set; }
    public decimal Price { get; set; }
    public int Qunt { get; set; }
    public string Gear { get; set; }
    public ICollection<Part> Parts { get; set; }
}
public class Part
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Qunt { get; set; }
    public ICollection<Car> Cars { get; set; }
}
public class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Addres { get; set; }
  
}
public class Sale
{
    public int Id { get; set; }
    public decimal All { get; set; }
    public int CarId { get; set; }
    public Car Car { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
}
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Addres { get; set; }

}

// class   :AutoPartsStore

public class AutoPartsStore : DbContext
{
    public DbSet<Car> Cars { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Part> Parts { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<Customer> Customers { get; set; }
// تكوين اتصال قاعدة البيانات
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       
        optionsBuilder.UseSqlServer("YourConnectionString");
    }
// تكوين العلاقات بين الكيانات
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
        modelBuilder.Entity<Part>()
            .HasOne(p => p.Supplier)
            .WithMany()
            .HasForeignKey(p => p.SupplierId);

        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Car)
            .WithMany()
            .HasForeignKey(s => s.CarId);

        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Customer)
            .WithMany()
            .HasForeignKey(s => s.CustomerId);

        modelBuilder.Entity<Car>()
            .HasMany(c => c.Parts)
            .WithMany(p => p.Cars)
            .UsingEntity(j => j.ToTable("CarPart"));
    }
}






[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly AutoPartsStore secured;

    public CarsController(AutoPartsStore _secured)
    {
        secured = _secured;
    }

     // get: api/cars/
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Car>>> GetCars()
    {
        return await secured.Cars.ToListAsync();
    }

  // get  &{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Car>> GetCar(int id)
    {
        var car = await secured.Cars.FindAsync(id);

        if (car == null)
        {
            return NotFound();
        }

        return car;
    }

    // post: api/cars ...
    [HttpPost]
    public async Task<ActionResult<Car>> PostCar(Car car)
    {
        secured.Cars.Add(car);
        await secured.SaveChangesAsync();

        return CreatedAtAction("GetCar", new { id = car.Id }, car);
    }

    // put: .... &{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCar(int id, Car car)
    {
        if (id != car.Id)
        {
            return BadRequest();
        }

        secured.Entry(car).State = EntityState.Modified;

        try
        {
            await secured.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CarExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // delete:....{id}    api/Cars
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCar(int id)
    {
        var car = await secured.Cars.FindAsync(id);
        if (car == null)
        {
            return NotFound();
        }

        secured.Cars.Remove(car);
        await secured.SaveChangesAsync();

        return NoContent();
    }

    private bool CarExists(int id)
    {
        return secured.Cars.Any(e => e.Id == id);
    }
}

//

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly AutoPartsStore secured;

    public SuppliersController(AutoPartsStore _secured)
    {
        secured = _secured;
    }

    // get: suppliers(api...)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers()
    {
        return await secured.Suppliers.ToListAsync();
    }

    // get: .... {id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Supplier>> GetSupplier(int id)
    {
        var supplier = await secured.Suppliers.FindAsync(id);

        if (supplier == null)
        {
            return NotFound();
        }

        return supplier;
    }

    // post:suppliers  (api... )
    [HttpPost]
    public async Task<ActionResult<Supplier>> PostSupplier(Supplier supplier)
    {
        secured.Suppliers.Add(supplier);
        await secured.SaveChangesAsync();

        return CreatedAtAction("GetSupplier", new { id = supplier.Id }, supplier);
    }

    // put: {id}   supplier ..
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSupplier(int id, Supplier supplier)
    {
        if (id != supplier.Id)
        {
            return BadRequest();
        }

        secured.Entry(supplier).State = EntityState.Modified;

        try
        {
            await secured.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SupplierExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: {id}   supllier (api.....)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSupplier(int id)
    {
        var supplier = await secured.Suppliers.FindAsync(id);
        if (supplier == null)
        {
            return NotFound();
        }

        secured.Suppliers.Remove(supplier);
        await secured.SaveChangesAsync();

        return NoContent();
    }

    private bool SupplierExists(int id)
    {
        return secured.Suppliers.Any(e => e.Id == id);
    }
}


[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly AutoPartsStore _secured;

    public SalesController(AutoPartsStore secured)
    {
        _secured = secured;
    }

    // get:   sale(api.....)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Sale>>> GetSales()
    {
        return await _secured.Sales.ToListAsync();
    }

    // get: {id}  sale(api.....)
    [HttpGet("{id}")]
    public async Task<ActionResult<Sale>> GetSale(int id)
    {
        var sale = await _secured.Sales.FindAsync(id);

        if (sale == null)
        {
            return NotFound();
        }

        return sale;
    }

    // put:    sale(api...)
    [HttpPost]
    public async Task<ActionResult<Sale>> PostSale(Sale sale1)
    {
        _secured.Sales.Add(sale1);
        await _secured.SaveChangesAsync();

        return CreatedAtAction("GetSale", new { id = sale1.Id }, sale1);
    }

    // put: id  sale(api...)
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSale(int id, Sale sale1)
    {
        if (id != sale1.Id)
        {
            return BadRequest();
        }

        _secured.Entry(sale1).State = EntityState.Modified;

        try
        {
            await _secured.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SaleExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // delete: id     sale (api.....)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSale(int id)
    {
        var sale = await _secured.Sales.FindAsync(id);
        if (sale == null)
        {
            return NotFound();
        }

        _secured.Sales.Remove(sale);
        await _secured.SaveChangesAsync();

        return NoContent();
    }

    private bool SaleExists(int id)
    {
        return _secured.Sales.Any(e => e.Id == id);
    }
}


[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly AutoPartsStore secured;

    public CustomersController(AutoPartsStore _secured)
    {
        secured = _secured;
    }

    // get:   customer(api...)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        return await secured.Customers.ToListAsync();
    }

    // get: id  customer(api...)
    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomer(int id)
    {
        var customer = await secured.Customers.FindAsync(id);

        if (customer == null)
        {
            return NotFound();
        }

        return customer;
    }

    // post:   customer(api...
    [HttpPost]
    public async Task<ActionResult<Customer>> PostCustomer(Customer custm)
    {
        secured.Customers.Add(custm);
        await secured.SaveChangesAsync();

        return CreatedAtAction("GetCustomer", new { id = custm.Id }, custm);
    }

    // put: id     customer(api...)
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCustomer(int id, Customer customer)
    {
        if (id != customer.Id)
        {
            return BadRequest();
        }

        secured.Entry(customer).State = EntityState.Modified;

        try
        {
            await secured.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CustomerExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // delete: id     customer(api...)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var customer = await secured.Customers.FindAsync(id);
        if (customer == null)
        {
            return NotFound();
        }

        secured.Customers.Remove(customer);
        await secured.SaveChangesAsync();

        return NoContent();
    }

    private bool CustomerExists(int id)
    {
        return secured.Customers.Any(e => e.Id == id);
    }
}
////

[ApiController]
[Route("api/[controller]")]
public class PartsController : ControllerBase
{
    private readonly AutoPartsStore secured;

    public PartsController(AutoPartsStore _secured)
    {
        secured = _secured;
    }

    // get:    patr(api....)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Part>>> GetParts()
    {
        return await secured.Parts.ToListAsync();
    }

    // GET: id   patr(api....)
    [HttpGet("{id}")]
    public async Task<ActionResult<Part>> GetPart(int id)
    {
        var part = await secured.Parts.FindAsync(id);

        if (part == null)
        {
            return NotFound();
        }

        return part;
    }

    // post: patrs(api....)
    [HttpPost]
    public async Task<ActionResult<Part>> PostPart(Part part)
    {
        secured.Parts.Add(part);
        await secured.SaveChangesAsync();

        return CreatedAtAction("GetPart", new { id = part.Id }, part);
    }

    // put: id   patrs(api....)
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPart(int id, Part pa)
    {
        if (id != pa.Id)
        {
            return BadRequest();
        }

        secured.Entry(pa).State = EntityState.Modified;

        try
        {
            await secured.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PartExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // delete: id   patr(api....)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePart(int id)
    {
        var part = await secured.Parts.FindAsync(id);
        if (part == null)
        {
            return NotFound();
        }

        secured.Parts.Remove(part);
        await secured.SaveChangesAsync();

        return NoContent();
    }

    private bool PartExists(int id)
    {
        return secured.Parts.Any(e => e.Id == id);
    }
}
////////////////////////////end/////


