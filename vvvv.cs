using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

public class Program

{
    public static void Main()
    {
        //bais class
        using (var context = new CarPartsStoreContext())
        {
            // add a car
            var car = new Car { Model = "ody", Price = 19500, Quantity = 1, Gear = "Automatic" };
            context.Cars.Add(car);
            context.SaveChanges();

            // add a piece
            var part = new Part { Name = "desk drawer", Price = 500, Quantity = 4, SupplierId = 1 };
            context.Parts.Add(part);
            context.SaveChanges();

            // add a factory
            var supplier = new Supplier { Name = " ali sami", Address = "Aleepo_Azaz" };
            context.Suppliers.Add(supplier);
            context.SaveChanges();

            // read all cars
            var cars = context.Cars.ToList();
            foreach (var c in cars)
            {
                Console.WriteLine($"Car: Id={c.Id}, Model={c.Model}, Price={c.Price}");
            }

            // read all blocks
            var parts = context.Parts.ToList();
            foreach (var p in parts)
            {
                Console.WriteLine($"Part: Id={p.Id}, Name={p.Name}, Price={p.Price}");
            }
        }
    }
}