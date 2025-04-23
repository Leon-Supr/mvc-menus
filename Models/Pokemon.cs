namespace menus.Models
{
    public class Pokemon
    {
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public int? Weight { get; set; }
        public List<string>? Types { get; set; }
        public List<string>? Abilities { get; set; }
    }
}