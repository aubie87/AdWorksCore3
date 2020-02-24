using AdWorksCore3.Core.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace AdWorksCore3.Web.Services
{
    public class PasswordService
    {
        public static Customer GenerateNewHash(Customer customer, string password)
        {
            int hashSize = 96;
            int saltSize = 6;
            byte[] salt = new byte[saltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            customer.PasswordHash = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: hashSize));

            customer.PasswordSalt = Convert.ToBase64String(salt);

            Debug.WriteLine($"len={customer.PasswordHash.Length} Hash={customer.PasswordHash}");
            Debug.WriteLine($"len={customer.PasswordSalt.Length} Salt={customer.PasswordSalt}");

            return customer;
        }

    }
}
