class Product
{
    private string _name;
    private string _id;
    private decimal _unitPrice;
    private int _quantity;

    public Product(string name, string id, decimal unitPrice, int quantity)
    {
        _name = name;
        _id = id;
        _unitPrice = unitPrice;
        _quantity = quantity;
    }

    public decimal GetTotalPrice()
    {
        return _unitPrice * _quantity;
    }

    public string GetPackingLabelLine()
    {
        return $"{_name} (ID: {_id})";
    }
}
