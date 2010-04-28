namespace Shrinkr.Infrastructure
{
    using System;
    using System.Linq;

    public class BaseX : IBaseX
    {
        private static readonly char[] baseThirtySixCharacterSet = "0123456789abcdefghijklmnoqrstuvwxyz".ToCharArray(); // There is no P as it is a Controller Action
        private static readonly char[] baseSixtyTwoCharacterSet = "0123456789ABCDEFGHIJKLMNOQRSTUVWXYZabcdefghijklmnoqrstuvwxyz".ToCharArray(); // No P too

        private readonly char[] characters;

        public BaseX(BaseType baseType)
        {
            characters = baseType == BaseType.BaseThirtySix ? baseThirtySixCharacterSet : baseSixtyTwoCharacterSet;
        }

        public string Encode(long value)
        {
            Check.Argument.IsNotZeroOrNegative(value, "value");

            string result = string.Empty;

            do
            {
                result = characters[value % characters.Length] + result;
                value /= characters.Length;
            }
            while (value != 0);

            return result;
        }

        public long Decode(string value)
        {
            Check.Argument.IsNotNullOrEmpty(value, "value");

            char[] keyArray = value.ToCharArray();
            Array.Reverse(keyArray);

            long result = 0;

            for (int i = 0; i < keyArray.Length; i++)
            {
                int index = Array.IndexOf(characters, keyArray[i]);
                double power = Math.Pow(characters.Length, i);

                result += Convert.ToInt64(index * power);
            }

            return result;
        }

        public bool IsValid(string value)
        {
            Check.Argument.IsNotNullOrEmpty(value, "value");

            return value.All(c => (c == 'P' || c == 'p' || Array.IndexOf(baseSixtyTwoCharacterSet, c) > -1));
        }
    }
}