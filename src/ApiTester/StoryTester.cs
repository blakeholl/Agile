﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ApiTester
{
    public class StoryTester
    {
        private readonly HttpClient _client;

        public StoryTester(HttpClient client)
        {
            _client = client;
        }

        public async Task RunTest()
        {
            var id = Guid.NewGuid().ToString("B");

            await _client.PostAsJsonAsync("/api/Stories", new
            {
                Id = id,
                Title = RandomString(25),
                Description = RandomString(400)
            });

            await _client.PostAsJsonAsync("/api/Stories/" + id + "/rename", new
            {
                Title = RandomString(50)
            });

            await _client.DeleteAsync("/api/Stories/" + id);
        }

        private static string RandomString(int length, string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            var outOfRange = Byte.MaxValue + 1 - (Byte.MaxValue + 1) % alphabet.Length;

            return string.Concat(
                Enumerable
                    .Repeat(0, Int32.MaxValue)
                    .Select(e => RandomByte())
                    .Where(randomByte => randomByte < outOfRange)
                    .Take(length)
                    .Select(randomByte => alphabet[randomByte % alphabet.Length])
            );
        }

        private static byte RandomByte()
        {
            using (var randomizationProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[1];
                randomizationProvider.GetBytes(randomBytes);
                return randomBytes.Single();
            }
        }
    }
}
