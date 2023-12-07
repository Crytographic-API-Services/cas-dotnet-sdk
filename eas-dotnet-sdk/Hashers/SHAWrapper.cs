﻿using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace EasDotnetSdk
{
    public class SHAWrapper
    {
        [DllImport("performant_encryption.dll")]
        private static extern IntPtr sha512(string password);
        [DllImport("performant_encryption.dll")]
        private static extern IntPtr sha256(string password);
        [DllImport("performant_encryption.dll")]
        public static extern void free_cstring(IntPtr stringToFree);

        public IntPtr SHA512HashString(string stringTohash)
        {
            if (string.IsNullOrEmpty(stringTohash))
            {
                throw new Exception("Please provide a string to hash");
            }
            return sha512(stringTohash);
        }
        public IntPtr SHA256HashString(string stringToHash)
        {
            if (string.IsNullOrEmpty(stringToHash))
            {
                throw new Exception("Please provide a string to hash");
            }
            return sha256(stringToHash);
        }
    }
}