using System;

namespace SHA_2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            SHA_256 sha256 = new SHA_256();

            Console.WriteLine(sha256.Encrypt("Hello"));
        }
    }
}