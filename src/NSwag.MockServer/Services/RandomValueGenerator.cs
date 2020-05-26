using System;
using System.Text;

namespace NSwag.MockServer.Services
{
    public class RandomValueGenerator
    {
        readonly Random rnd = new Random();
        private int counter = 0;

        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        public int MinCollectionLength { get; set; }
        public int MaxCollectionLength { get; set; }

        public bool UpperCase { get; set; }

        public RandomValueGenerator()
        {
            MinValue = -10000;
            MaxValue = 10000;
            MinCollectionLength = 1;
            MaxCollectionLength = 20;
            UpperCase = false;
        }

        public RandomValueGenerator(int minValue, int maxValue, int minCollectionLength, int maxCollectionLength,
            bool generateUpperCaseString)
        {
            if (minCollectionLength < 0 || minCollectionLength > maxCollectionLength)
                throw new ArgumentException(
                    "'MinCollectionLength' must be greater than or equal zero. MaxCollectionLength must be greater than MinCollectionLength. Parameters: minCollectionLength, maxCollectionLength");

            if (maxValue < 0 || minValue > maxValue)
                throw new ArgumentException(
                    "'MaxValue' must be greater than or equal zero. MaxValue must be greater than MinValue. Parameters: maxValue, minValue.");

            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.MinCollectionLength = minCollectionLength;
            this.MaxCollectionLength = maxCollectionLength;
            this.UpperCase = generateUpperCaseString;
        }

        public bool GenerateBoolean()
        {
            return Convert.ToBoolean(rnd.Next(2));
        }

        public char GenerateChar()
        {
            return Convert.ToChar(rnd.Next(32, 126));
        }

        public sbyte GenerateSByte()
        {
            sbyte minSByte = MinValue < sbyte.MinValue ? sbyte.MinValue : (sbyte) MinValue;
            sbyte maxSByte = MaxValue > sbyte.MaxValue ? sbyte.MaxValue : (sbyte) MaxValue;

            return Convert.ToSByte(rnd.Next(minSByte, maxSByte));
        }

        public byte GenerateByte()
        {
            byte maxByte = MaxValue > byte.MaxValue ? byte.MaxValue : (byte) MaxValue;

            return Convert.ToByte(rnd.Next(maxByte));
        }

        public short GenerateInt16()
        {
            short minShort = MinValue < short.MinValue ? short.MinValue : (short) MinValue;
            short maxShort = MaxValue > short.MaxValue ? short.MaxValue : (short) MaxValue;

            return Convert.ToInt16(rnd.Next(minShort, maxShort));
        }

        public ushort GenerateUInt16()
        {
            ushort maxUShort = MaxValue > ushort.MaxValue ? ushort.MaxValue : (ushort) MaxValue;

            return Convert.ToUInt16(rnd.Next(maxUShort));
        }

        public int GenerateInt32()
        {
            return Convert.ToInt32(rnd.Next(MinValue, MaxValue));
        }

        public uint GenerateUInt32()
        {
            return Convert.ToUInt32(rnd.Next(MaxValue));
        }

        public long GenerateInt64()
        {
            return Convert.ToInt64(rnd.Next(MinValue, MaxValue));
        }

        public ulong GenerateUInt64()
        {
            return Convert.ToUInt64(rnd.Next(MaxValue));
        }

        public float GenerateSingle()
        {
            return Convert.ToSingle(rnd.NextDouble() * rnd.Next(MinValue, MaxValue));
        }

        public double GenerateDouble()
        {
            return Convert.ToDouble(rnd.NextDouble() * rnd.Next(MinValue, MaxValue));
        }

        public decimal GenerateDecimal()
        {
            return Convert.ToDecimal(rnd.NextDouble() * rnd.Next(MinValue, MaxValue));
        }

        public DateTime GenerateDateTime()
        {
            return DateTime.Now.AddDays(rnd.Next(-730, 730));
        }

        public string GenerateString()
        {
            counter++;
            char[] consonants = new char[]
            {
                'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'z'
            };
            char[] vowels = new char[] {'a', 'e', 'i', 'o', 'u', 'y'};

            StringBuilder sb = new StringBuilder();

            sb.Append(char.ToUpper(consonants[rnd.Next(0, consonants.Length)]));

            for (int i = 0; i < rnd.Next(2, 6); i++)
            {
                sb.Append(vowels[rnd.Next(0, vowels.Length)]);
                sb.Append(consonants[rnd.Next(0, consonants.Length)]);
            }

            sb.Append("_" + counter);

            return UpperCase ? sb.ToString().ToUpper() : sb.ToString();
        }

        public int GetCollectionLength()
        {
            return rnd.Next(MinCollectionLength, MaxCollectionLength);
        }
    }
}