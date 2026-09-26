using System.Text;

class Order
{
    private Customer _customer;
    private List<Product> _products;

    public Order(Customer customer)
    {
        _customer = customer;
        _products = new List<Product>();
    }

    public void AddProduct(Product product)
    {
        _products.Add(product);
    }

    public decimal GetTotalCost()
    {
        decimal productTotal = 0;

        foreach (Product product in _products)
        {
            productTotal += product.GetTotalPrice();
        }

        decimal shippingCost = _customer.IsInUSA() ? 5m : 35m;
        return productTotal + shippingCost;
    }

    public string GetPackingLabel()
    {
        StringBuilder label = new StringBuilder();

        foreach (Product product in _products)
        {
            label.AppendLine(product.GetPackingLabelLine());
        }

        return label.ToString().TrimEnd();
    }

    public string GetShippingLabel()
    {
        return $"{_customer.GetName()}\n{_customer.GetAddress()}";
    }
}
