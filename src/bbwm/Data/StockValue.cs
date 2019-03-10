namespace bbwm
{
    public class StockValue
    {
        public string Symbol { get; set; }
        public decimal Quote { get; set; }
        public string FailureReason { get; set; }
    }
}