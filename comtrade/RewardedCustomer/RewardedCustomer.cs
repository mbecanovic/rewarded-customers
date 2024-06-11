namespace comtrade.RewardedCustomer
{
    public class RewardedCustomer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SSN { get; set; }
        public string DOB { get; set; }
        public int HomeAddressId { get; set; } // Foreign key for home address
        public Address HomeAddress { get; set; } // Navigation property for home address
        public int OfficeAddressId { get; set; } // Foreign key for office address
        public Address OfficeAddress { get; set; } // Navigation property for office address
        public string FavoriteColors { get; set; }
        public string Age { get; set; }
    }

    public class Address
    {
        public int Id { get; set; } // Primary key
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public ICollection<RewardedCustomer> RewardedCustomers { get; set; } // Navigation property for customers with this address
    }

}
