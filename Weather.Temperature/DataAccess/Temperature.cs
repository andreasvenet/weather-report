namespace Weather.Temperature.DataAccess {
    public class Temperature {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal TempHigh { get; set; }
        public decimal TempLow { get; set; }
        public string ZipCode { get; set; }
    }
}
