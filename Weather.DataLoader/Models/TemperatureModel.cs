namespace Weather.DataLoader.Models {
    internal class TemperatureModel {
        public DateTime CreatedOn { get; set; }
        public decimal TempHigh { get; set; }
        public decimal TempLow { get; set; }
        public string ZipCode { get; set; }
    }
}
