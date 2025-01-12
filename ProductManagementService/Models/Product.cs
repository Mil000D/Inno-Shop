namespace ProductManagementService.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public int CreatorUserId { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    }
}
