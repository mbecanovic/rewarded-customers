using System.Net;

namespace comtrade.Model.Person
{
    public class Person
    {
        public string Name { get; set; }
        public string SSN { get; set; }
        public string DOB { get; set; }
        public Address Home { get; set; }
        public Address Office { get; set; }
        public List<string> FavoriteColors { get; set; }
        public int Age { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}
