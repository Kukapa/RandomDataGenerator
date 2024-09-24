using Bogus;
using User = RandomDataGenerator.Models.User;

namespace RandomDataGenerator.Code
{
    public class FakeDataGenerator
    {
        private readonly Faker<User> _faker;

        private FakeDataGenerator(Faker<User> faker)
            => _faker = faker;

        public IEnumerable<User> Generate(int page, int size, double errorProb)
        {
            var data = _faker.GenerateForever().Skip((page - 1) * size).Take(size).ToList();
            return ApplyErrors(data, errorProb);
        }

        private IEnumerable<User> ApplyErrors(IEnumerable<User> data, double errorProb)
        {
            foreach (var user in data)
            {
                IntroduceErrors(user, errorProb);
            }
            return data;
        }

        private void IntroduceErrors(User user, double errorProb)
        {
            user.FirstName = ErrorGenerator.ApplyErrorsWithProbability(user.FirstName, errorProb, 1, GetRegion());
            user.LastName = ErrorGenerator.ApplyErrorsWithProbability(user.LastName, errorProb, 1, GetRegion());
            user.Street = ErrorGenerator.ApplyErrorsWithProbability(user.Street, errorProb, 1, GetRegion());
            user.City = ErrorGenerator.ApplyErrorsWithProbability(user.City, errorProb, 1, GetRegion());
            user.State = ErrorGenerator.ApplyErrorsWithProbability(user.State, errorProb, 1, GetRegion());
            user.Country = ErrorGenerator.ApplyErrorsWithProbability(user.Country, errorProb, 1, GetRegion());
            user.PhoneNumber = ErrorGenerator.ApplyErrorsWithProbability(user.PhoneNumber, errorProb, 1, GetRegion());
        }

        private Region GetRegion()
            => CountryInfo.GetRegion(_faker.Locale);

        private static string GetRegion(Region region)
            => (region) switch
            {
                Region.EN => "en_US",
                Region.PL => "pl",
                Region.RU => "ru",
                _ => throw new InvalidOperationException("Invalid enum value.")
            };

        private static string GeneratePhoneNumber(Faker f, Region region)
        {
            return region switch
            {
                Region.EN => $"+1 {f.Phone.PhoneNumber("###-###-####")}",      
                Region.PL => $"+48 {f.Phone.PhoneNumber("##-###-##-##")}",      
                Region.RU => $"+7 {f.Phone.PhoneNumber("####-##-##-##")}",         
                _ => f.Phone.PhoneNumber(),                                       
            };
        }

        public static FakeDataGenerator Create(int seed, Region region)
        {
            int index = 0;
            return new(new Faker<User>(GetRegion(region))
                .UseSeed(seed)
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
                .RuleFor(u => u.Identifier, (f, u) => f.Random.Guid())
                .RuleFor(u => u.Index, f => index++)
                .RuleFor(u => u.Street, f => f.Address.StreetAddress())
                .RuleFor(u => u.State, f => f.Address.State())
                .RuleFor(u => u.City, f => f.Address.City())
                .RuleFor(u => u.Country, f => f.Address.LocalizedCountry())
                .RuleFor(p => p.PhoneNumber, (f, p) => GeneratePhoneNumber(f, region))
                );
        }
    }
}
