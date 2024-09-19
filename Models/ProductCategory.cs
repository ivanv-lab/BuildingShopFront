namespace BuildingShopFront.Models
{
    public class ProductCategory
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual ICollection<Product>? Products { get; set; }
    }
}
