﻿using CasDotnetSdk.PasswordHashers.Types;
using System;
using System.Runtime.InteropServices;

namespace CasDotnetSdk.PasswordHashers.Linux
{
    internal static class Argon2LinuxWrapper
    {

        [DllImport("Contents/libcas_core_lib.so")]
        public static extern IntPtr argon2_hash(string passToHash);

        [DllImport("Contents/libcas_core_lib.so")]
        public static extern IntPtr argon2_hash_threadpool(string passToHash);

        [DllImport("Contents/libcas_core_lib.so")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool argon2_verify(string hashedPassword, string passToVerify);

        [DllImport("Contents/libcas_core_lib.so")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool argon2_verify_threadpool(string hashedPassword, string passToVerify);

        [DllImport("Contents/libcas_core_lib.so")]
        public static extern void free_cstring(IntPtr stringToFree);
    }
}
