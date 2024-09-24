using Bogus.DataSets;

namespace RandomDataGenerator.Code
{
    public static class CountryInfo
    {
        public static string LocalizedCountry(this Address address)
            => GetCountry(GetRegion(address.Locale));

        public static string GetCountry(Region region)
            => (region) switch
            {
                Region.EN => "United States",
                Region.PL => "Polska",
                Region.RU => "Россия",
                _ => throw new InvalidOperationException("Invalid enum value.")
            };

        public static Region GetRegion(string region)
            => (region) switch
            {
                "en_US" => Region.EN,
                "pl" => Region.PL,
                "ru" => Region.RU,
                _ => throw new InvalidOperationException("Invalid enum value.")
            };
    }
 }
