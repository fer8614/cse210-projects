class Program
{
    static void Main(string[] args)
    {
        Address usaAddress = new Address(
            "742 Evergreen Terrace",
            "Springfield",
            "Oregon",
            "USA");
        Customer usaCustomer = new Customer("Jordan Lee", usaAddress);
        Order usaOrder = new Order(usaCustomer);
        usaOrder.AddProduct(new Product("Notebook", "NB-101", 4.50m, 3));
        usaOrder.AddProduct(new Product("Mechanical Pencil Set", "MP-205", 8.25m, 2));
        usaOrder.AddProduct(new Product("Desk Organizer", "DO-310", 12.99m, 1));

        Address internationalAddress = new Address(
            "25 King Street",
            "Toronto",
            "Ontario",
            "Canada");
        Customer internationalCustomer = new Customer("Avery Morgan", internationalAddress);
        Order internationalOrder = new Order(internationalCustomer);
        internationalOrder.AddProduct(new Product("Travel Mug", "TM-410", 15.75m, 2));
        internationalOrder.AddProduct(new Product("Canvas Backpack", "CB-520", 34.95m, 1));

        List<Order> orders = new List<Order> { usaOrder, internationalOrder };

        foreach (Order order in orders)
        {
            Console.WriteLine("Packing Label:");
            Console.WriteLine(order.GetPackingLabel());
            Console.WriteLine();
            Console.WriteLine("Shipping Label:");
            Console.WriteLine(order.GetShippingLabel());
            Console.WriteLine();
            Console.WriteLine($"Total: {order.GetTotalCost():C}");
            Console.WriteLine(new string('-', 40));
        }
    }
}
