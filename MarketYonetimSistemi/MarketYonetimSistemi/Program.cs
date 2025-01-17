using System;
using System.Collections.Generic;
using System.Linq;

public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }

    public Product(string name, decimal price, int stockQuantity)
    {
        Name = name;
        Price = price;
        StockQuantity = stockQuantity;
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= StockQuantity)
        {
            StockQuantity -= quantity;
        }
        else
        {
            throw new Exception("Yetersiz stok.");
        }
    }
}

public abstract class Customer
{
    public string Name { get; set; }
    public string ContactInfo { get; set; }
    public int LoyaltyPoints { get; set; }

    public Customer(string name, string contactInfo)
    {
        Name = name;
        ContactInfo = contactInfo;
        LoyaltyPoints = 0;
    }

    public void AddLoyaltyPoints(int points)
    {
        LoyaltyPoints += points;
    }

    public abstract void DisplayInfo();
}

public class IndividualCustomer : Customer
{
    public IndividualCustomer(string name, string contactInfo) : base(name, contactInfo) { }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Bireysel Müşteri: {Name}, İletişim: {ContactInfo}");
    }
}

public class CorporateCustomer : Customer
{
    public string CompanyName { get; set; }

    public CorporateCustomer(string name, string contactInfo, string companyName) : base(name, contactInfo)
    {
        CompanyName = companyName;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Kurumsal Müşteri: {CompanyName}, İletişim: {ContactInfo}");
    }
}

public abstract class Payment
{
    public decimal Amount { get; set; }

    public Payment(decimal amount)
    {
        Amount = amount;
    }

    public abstract void ProcessPayment();
}

public class CreditCardPayment : Payment
{
    public string CardNumber { get; set; }

    public CreditCardPayment(decimal amount, string cardNumber) : base(amount)
    {
        CardNumber = cardNumber;
    }

    public override void ProcessPayment()
    {
        Console.WriteLine($"Kredi kartı ile ödeme alındı: {Amount} TL");
    }
}

public class CashPayment : Payment
{
    public CashPayment(decimal amount) : base(amount) { }

    public override void ProcessPayment()
    {
        Console.WriteLine($"Nakit ödeme alındı: {Amount} TL");
    }
}

public class TransferPayment : Payment
{
    public string BankAccount { get; set; }

    public TransferPayment(decimal amount, string bankAccount) : base(amount)
    {
        BankAccount = bankAccount;
    }

    public override void ProcessPayment()
    {
        Console.WriteLine($"Havale ile ödeme alındı: {Amount} TL");
    }
}

public class Cart
{
    private List<Product> products;

    public Cart()
    {
        products = new List<Product>();
    }

    public void AddProduct(Product product)
    {
        products.Add(product);
    }

    public void RemoveProduct(Product product)
    {
        products.Remove(product);
    }

    public decimal GetTotalAmount()
    {
        return products.Sum(p => p.Price);
    }
}

public abstract class Discount
{
    public abstract decimal ApplyDiscount(decimal amount);
}

public class PercentageDiscount : Discount
{
    private readonly decimal percentage;

    public PercentageDiscount(decimal percentage)
    {
        this.percentage = percentage;
    }

    public override decimal ApplyDiscount(decimal amount)
    {
        return amount - (amount * percentage / 100);
    }
}

public class FixedDiscount : Discount
{
    private readonly decimal discountAmount;

    public FixedDiscount(decimal discountAmount)
    {
        this.discountAmount = discountAmount;
    }

    public override decimal ApplyDiscount(decimal amount)
    {
        return amount - discountAmount;
    }
}

public enum OrderStatus
{
    Onaylandi,
    Hazirlaniyor,
    TeslimEdildi
}

public class Order
{
    public List<Product> Products { get; set; }
    public Customer Customer { get; set; }
    public Payment Payment { get; set; }
    public OrderStatus Status { get; set; }

    public Order(Customer customer)
    {
        Customer = customer;
        Products = new List<Product>();
        Status = OrderStatus.Onaylandi;
    }

    public void AddProduct(Product product, int quantity)
    {
        product.DecreaseStock(quantity);
        for (int i = 0; i < quantity; i++)
        {
            Products.Add(product);
        }
    }

    public void ProcessOrder()
    {
        try
        {
            Payment.ProcessPayment();
            Status = OrderStatus.Hazirlaniyor;

            Customer.AddLoyaltyPoints((int)GetTotalAmount());
            Status = OrderStatus.TeslimEdildi;
            Console.WriteLine("Sipariş başarıyla teslim edildi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Sipariş işlemi sırasında bir hata oluştu: {ex.Message}");
        }
    }

    public decimal GetTotalAmount()
    {
        return Products.Sum(p => p.Price);
    }

    public void CancelOrder()
    {
        if (Status == OrderStatus.Onaylandi || Status == OrderStatus.Hazirlaniyor)
        {
            Status = OrderStatus.Onaylandi;
            foreach (var product in Products)
            {
                product.StockQuantity += 1;
            }
            Console.WriteLine("Sipariş iptal edildi.");
        }
        else
        {
            Console.WriteLine("Teslim edilen siparişler iptal edilemez.");
        }
    }

    public void ReturnOrder()
    {
        if (Status == OrderStatus.TeslimEdildi)
        {
            Status = OrderStatus.Onaylandi;
            foreach (var product in Products)
            {
                product.StockQuantity += 1;
            }
            Console.WriteLine("Sipariş iade edildi.");
        }
        else
        {
            Console.WriteLine("Sipariş teslim edilmeden iade edilemez.");
        }
    }
}

public class Category
{
    public string Name { get; set; }
    public List<Product> Products { get; set; }

    public Category(string name)
    {
        Name = name;
        Products = new List<Product>();
    }

    public void AddProduct(Product product)
    {
        Products.Add(product);
    }
}

public class Supplier
{
    public string Name { get; set; }
    public string ContactInfo { get; set; }

    public Supplier(string name, string contactInfo)
    {
        Name = name;
        ContactInfo = contactInfo;
    }

    public void SupplyProduct(Product product, int quantity)
    {
        product.StockQuantity += quantity;
        Console.WriteLine($"{quantity} adet {product.Name} tedarik edildi.");
    }
}

public class Employee
{
    public string Name { get; set; }
    public string Position { get; set; }
    public string AuthorityLevel { get; set; }

    public Employee(string name, string position, string authorityLevel)
    {
        Name = name;
        Position = position;
        AuthorityLevel = authorityLevel;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Çalışan: {Name}, Pozisyon: {Position}, Yetki Seviyesi: {AuthorityLevel}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Ürün ve kategori oluşturma
        Category beverages = new Category("İçecekler");
        Product cola = new Product("Kola", 10, 100);
        beverages.AddProduct(cola);

        // Müşteri ve ödeme oluşturma
        Customer customer = new IndividualCustomer("Ahmet Yılmaz", "ahmet@mail.com");
        Payment payment = new CreditCardPayment(100, "1234-5678-9101-1121");

        // Sipariş oluşturma
        Order order = new Order(customer);
        order.AddProduct(cola, 5);
        order.Payment = payment;

        // Siparişi işleme
        order.ProcessOrder();

        // Çalışan oluşturma
        Employee employee = new Employee("Ayşe Kaya", "Kasiyer", "Yüksek");
        employee.DisplayInfo();

        // Siparişi iptal etme
        order.CancelOrder();

        // Tedarikçi oluşturma ve ürün tedarik etme
        Supplier supplier = new Supplier("Tedarikçi A.Ş.", "tedarikci@mail.com");
        supplier.SupplyProduct(cola, 50);
    }
}

