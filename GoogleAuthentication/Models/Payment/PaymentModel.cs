namespace GoogleAuthentication.Models.Payment
{
    public class PaymentModel
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public decimal Amount { get; set; }
    }
    public class OrderModel
    {
        public decimal OrderAmount { get; set; }
        public decimal OrderAmountInSubUnits
        {
            get
            {
                return OrderAmount * 100;
            }
        }
        public string Currency { get; set; }
        public int Payment_Capture { get; set; }
        public Dictionary<string, string> Notes { get; set; }
    }
    public class RazorPayOptionsModel
    {
        public string Key { get; set; }
        public decimal AmountInSubUnits { get; set; }
        public string Currency { get; set; }
       public string Name { get; set; }
        public string Description { get; set; }
        public string ImagelogUr1 { get; set; }
        public string OrderId { get; set; }
        public string ProfileName { get; set; }
        public string ProfileContact { get; set; }
        public string ProfileEmail { get; set; }
        public Dictionary<string, string> Notes { get; set; }
    }
}
