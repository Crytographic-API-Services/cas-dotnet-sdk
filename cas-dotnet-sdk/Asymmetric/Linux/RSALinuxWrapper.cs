﻿using System;
using System.Runtime.InteropServices;
using static CasDotnetSdk.Asymmetric.RSAWrapper;

namespace CasDotnetSdk.Asymmetric.Linux
{
    internal static class RSALinuxWrapper
    {
        [DllImport("cas_core_lib.so")]
        public static extern RustRsaKeyPairStruct get_key_pair(int key_size);

        [DllImport("cas_core_lib.so")]
        public static extern IntPtr rsa_encrypt(string publicKey, string dataToEncrypt);

        [DllImport("cas_core_lib.so")]
        public static extern RsaEncryptBytesResult rsa_encrypt_bytes(string publicKey, byte[] dataToEncrypt, int dataToEncryptLenght);

        [DllImport("cas_core_lib.so")]
        public static extern IntPtr rsa_decrypt(string publicKey, string dataToDecrypt);

        [DllImport("cas_core_lib.so")]
        public static extern RsaDecryptBytesResult rsa_decrypt_bytes(string privateKey, byte[] dataToDecrypt, int dataToDecryptLenght);

        [DllImport("cas_core_lib.so")]
        public static extern RsaSignResultStruct rsa_sign(string dataToSign, int keySize);

        [DllImport("cas_core_lib.so")]
        public static extern RsaSignBytesResults rsa_sign_with_key_bytes(string privateKey, byte[] dataToSign, int dataToSignLength);

        [DllImport("cas_core_lib.so")]
        public static extern IntPtr rsa_sign_with_key(string publicKey, string dataToSign);

        [DllImport("cas_core_lib.so")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool rsa_verify(string publicKey, string dataToVerify, string signature);

        [DllImport("cas_core_lib.so")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool rsa_verify_bytes(string publicKey, byte[] dataToVerify, int dataToVerifyLength, byte[] signature, int signatureLength);

        [DllImport("cas_core_lib.so")]
        public static extern void free_cstring(IntPtr stringToFree);

        [DllImport("cas_core_lib.so")]
        public static extern void free_bytes(IntPtr bytesToFree);
    }
}
