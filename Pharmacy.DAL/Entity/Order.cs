using Pharmacy.DAL.Entity;

public class Order
{
    public int OrderID { get; set; }
    public int PatientID { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
    public double TotalPrice { get; set; }

    public Patient Patient { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    public Payment Payment { get; set; }
}
