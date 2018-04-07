﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Advanced_PassGen.Classes.PASSWORD
{
    /// <summary>
    /// A class to generate random strings
    /// </summary>
    internal sealed class PasswordController
    {
        #region Variables
        /// <summary>
        /// The minimum length of a password
        /// </summary>
        private readonly int _minLength;
        /// <summary>
        /// The maximum length of a password
        /// </summary>
        private readonly int _maxLength;
        /// <summary>
        /// The amount of passwords that need to be generated
        /// </summary>
        private readonly int _amount;
        /// <summary>
        /// The seed that will be used for the Random object
        /// </summary>
        private readonly int _seed;
        /// <summary>
        /// A boolean to indicate if the password should be converted into base64 format
        /// </summary>
        private readonly bool _base64;
        /// <summary>
        /// The character set that can be used to generate passwords
        /// </summary>
        private readonly string _charSet;

        internal List<Password> PasswordList;
        private static Random _rnd = new Random();
        #endregion

        /// <summary>
        /// Initiate a new PasswordGenerator object
        /// </summary>
        /// <param name="charSet">The character set that can be used to generate passwords with</param>
        /// <param name="minLength">The minimum length of a password</param>
        /// <param name="maxLength">The maximum length of a password</param>
        /// <param name="amount">The amount of passwords that need to be generated</param>
        /// <param name="seed">The seed for the random number generator</param>
        /// <param name="base64">A boolean to indicate whether the password should be converted into a base64 string</param>
        internal PasswordController(string charSet, int minLength, int maxLength, int amount, int seed, bool base64)
        {
            _charSet = charSet;
            _minLength = minLength;
            _maxLength = maxLength;
            _amount = amount;
            _seed = seed;
            _base64 = base64;
        }

        /// <summary>
        /// Generate a list of passwords
        /// </summary>
        /// <returns>A list of passwords</returns>
        internal async Task<List<Password>> GeneratePasswords()
        {
            PasswordList = new List<Password>();
            _rnd = new Random(_seed);
            await Task.Run(() =>
            {
                Encoding encoding = Encoding.Default;
                for (int i = 0; i < _amount; i++)
                {
                    var sub = _rnd.Next(_minLength, _maxLength);
                    string pwd = GetRandomString(sub, _charSet);

                    if (_base64)
                    {
                        pwd = Convert.ToBase64String(encoding.GetBytes(pwd));
                    }

                    Password pass = new Password { ActualPassword = pwd };
                    PasswordList.Add(pass);
                }
            });
            return PasswordList;
        }

        /// <summary>
        /// Generate a random string
        /// </summary>
        /// <param name="length">The length of the string that needs to be generated</param>
        /// <param name="characterSet">The characterset that the generator can use</param>
        /// <returns></returns>
        private static string GetRandomString(int length, IEnumerable<char> characterSet)
        {
            char[] characterArray = characterSet.Distinct().ToArray();
            byte[] bytes = new byte[length * 8];
            new RNGCryptoServiceProvider().GetBytes(bytes);
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                ulong value = BitConverter.ToUInt64(bytes, i * 8);
                result[i] = characterArray[value % (uint)characterArray.Length];
            }
            return new string(result);
        }
    }
}