namespace BaltaDataAccess.Models
{
  public class Career
  {
    public Career()
    {
      CareerItems = new List<CareerItem>();
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public List<CareerItem> CareerItems { get; set; }
  }
}