using System.Security.Cryptography;

namespace RandomDataGenerator.Code
{
    public static class ErrorGenerator
    {
        private static readonly IReadOnlyDictionary<Region, string> _alphabet = new Dictionary<Region, string>()
        {
            { Region.EN, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890" },
            { Region.PL, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZąćęłńóśźżĄĆĘŁŃÓŚŹŻ1234567890" },
            { Region.RU, "абвгдеёжзийклмнопрстуфхцчшщьыъэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯ1234567890" }
        }.AsReadOnly();

        public static string ApplyErrorsWithProbability(string input, double errorProbability, int maxErrors, Region region)
        {
            if (errorProbability == 0) return input;

            if (Random.Shared.NextDouble() <= (errorProbability * 0.01))
            {
                int errorCount = (int)Math.Ceiling(errorProbability * maxErrors);
                return ApplyError(input, errorCount, region);
            }
            return input;
        }

        private static string ApplyError(string input, int errorCount, Region region)
        {
            for (int i = 0; i < errorCount; i++)
            {
                int errorType = RandomNumberGenerator.GetInt32(0, 3);
                input = errorType switch
                {
                    0 => RemoveCharacter(input),
                    1 => AddCharacter(input, region),
                    2 => SwapCharacters(input),
                    _ => input
                };
            }
            return input;
        }

        private static string RemoveCharacter(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            int index = RandomNumberGenerator.GetInt32(0, input.Length);
            return input.Remove(index, 1);
        }

        private static string AddCharacter(string input, Region region)
        {
            int index = RandomNumberGenerator.GetInt32(0, input.Length + 1);
            char randomChar = _alphabet[region][RandomNumberGenerator.GetInt32(_alphabet[region].Length)];

            return input.Insert(index, randomChar.ToString());
        }

        private static string SwapCharacters(string input)
        {
            if (input.Length < 2) return input;

            int index = RandomNumberGenerator.GetInt32(0, input.Length - 1);

            char[] chars = input.ToCharArray();
            (chars[index], chars[index + 1]) = (chars[index + 1], chars[index]);

            return new string(chars);
        }
    }
}
