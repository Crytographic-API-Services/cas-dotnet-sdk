using System;
using System.Runtime.InteropServices;

namespace CasDotnetSdk.PasswordHashers.Linux
{
    internal static class SCryptLinuxWrapper
    {
        [DllImport("Contents/libcas_core_lib.so")]
        public static extern IntPtr scrypt_hash(string passToHash);

        [DllImport("Contents/libcas_core_lib.so")]
        public static extern IntPtr scrypt_hash_threadpool(string passToHash);

        [DllImport("Contents/libcas_core_lib.so")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool scrypt_verify(string hashedPassword, string password);

        [DllImport("Contents/libcas_core_lib.so")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool scrypt_verify_threadpool(string password, string hash);
    }
}
