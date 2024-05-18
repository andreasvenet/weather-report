namespace Weather.Report.DataAccess {
    public class WeatherReport {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal AverageHigh { get; set; }
        public decimal AverageLow { get; set; }
        public decimal RainfallTotalInches { get; set; }
        public decimal SnowTotalInches { get; set; }
        public string ZipCode { get; set; }
    }
}
