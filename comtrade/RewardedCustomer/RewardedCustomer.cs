namespace comtrade.RewardedCustomer
{
    public class RewardedCustomers
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SSN { get; set; }
        public string DOB { get; set; }
        public int HomeAddressId { get; set; }
        public Address HomeAddress { get; set; }
        public int OfficeAddressId { get; set; }
        public Address OfficeAddress { get; set; }
        public string FavoriteColors { get; set; }
        public string Age { get; set; }
        public int TimesRewarded { get; set; }
    }

    public class Address
    {
        public int Id { get; set; } // Primary key
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }

    public class ApiUsage
    {
        public int Id { get; set; }
        public string AgentId { get; set; }
        public int CallCount { get; set; }
        public DateTime LastCallTime { get; set; }
    }


}
