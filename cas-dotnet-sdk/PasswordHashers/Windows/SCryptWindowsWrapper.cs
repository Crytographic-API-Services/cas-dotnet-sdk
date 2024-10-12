using System;
using System.Runtime.InteropServices;

namespace CasDotnetSdk.PasswordHashers.Windows
{
    internal static class SCryptWindowsWrapper
    {
        [DllImport("\\Contents\\cas_core_lib.dll")]
        public static extern IntPtr scrypt_hash(string passToHash);

        [DllImport("\\Contents\\cas_core_lib.dll")]
        public static extern IntPtr scrypt_hash_threadpool(string passToHash);

        [DllImport("\\Contents\\cas_core_lib.dll")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool scrypt_verify(string hashedPassword, string password);

        [DllImport("\\Contents\\cas_core_lib.dll")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool scrypt_verify_threadpool(string password, string hash);
    }
}
