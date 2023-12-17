using pos.Entities;

namespace pos.Models.Store
{
    public class DetailStoreModel
    {
        public List<pos.Entities.Order> Orders { get; set; }
        public int TotalProductsSold { get; set; }  
        public decimal TotalRevenuePerStore { get; set;}
        public ApplicationUser? TopSellingUser { get; set; }
        public Customer? TopSpendingCustomer { get; set; }
    }
}
