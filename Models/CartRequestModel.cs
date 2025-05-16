namespace GP.Models
{
    public class CartRequestModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string SelectedColor { get; set; } = "";


    }
}
