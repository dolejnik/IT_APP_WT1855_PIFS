namespace BusinessLogic.Models
{
    public class OrdersDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int DeviceId { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
    }
}
