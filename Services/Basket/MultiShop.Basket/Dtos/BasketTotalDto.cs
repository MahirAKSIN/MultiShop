namespace MultiShop.Basket.Dtos
{
    public class BasketTotalDto
    {
        public string UsreId { get; set; }
        public string DiscountCode { get; set; }
        public int DiscountRate { get; set; }
        public List<BasketItemDto> BasketItems { get; set; }
        public decimal TotolPrice { get => BasketItems.Sum(q => q.Price * q.Quantity); }
    }

}
