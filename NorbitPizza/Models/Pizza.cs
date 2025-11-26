namespace NorbitPizza.Models
{
    public class Pizza
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = "/images/pizza-1.jpg";
        public List<string> Ingredients { get; set; } = new List<string>();
        public string Category { get; set; } = "Classic";
        public bool IsCustom { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}