namespace comtrade.RewardedCustomer
{
    public class RewardedCustomerDTO
    {
        public string Name { get; set; }
        public string SSN { get; set; }
        public string DOB { get; set; }
        public AddressDTO Home { get; set; }
        public AddressDTO Office { get; set; }
        public string FavoriteColors { get; set; }
        public string Age { get; set; }
    }
    public class AddressDTO
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}
