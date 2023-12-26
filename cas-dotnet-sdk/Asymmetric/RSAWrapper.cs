﻿using CasDotnetSdk.Asymmetric.Linux;
using CasDotnetSdk.Asymmetric.Windows;
using CasDotnetSdk.Helpers;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CasDotnetSdk.Asymmetric
{
    public class RSAWrapper
    {
        private readonly OSPlatform _platform;
        public RSAWrapper()
        {
            this._platform = new OperatingSystemDeterminator().GetOperatingSystem();
        }

        public class RsaKeyPairResult
        {
            public string PublicKey { get; set; }
            public string PrivateKey { get; set; }
        }

        public class RsaSignResult
        {
            public string Signature { get; set; }
            public string PublicKey { get; set; }
        }

        internal struct RustRsaKeyPairStruct
        {
            public IntPtr pub_key;
            public IntPtr priv_key;
        }
        internal struct RsaSignResultStruct
        {
            public IntPtr signature;
            public IntPtr public_key;
        }

        internal struct RsaEncryptBytesResult
        {
            public IntPtr encrypted_result_ptr;
            public int length;
        }

        internal struct RsaDecryptBytesResult
        {
            public IntPtr decrypted_result_ptr;
            public int length;
        }

        internal struct RsaSignBytesResults
        {
            public IntPtr signature_raw_ptr;
            public int length;
        }

        public string RsaSignWithKey(string privateKey, string dataToSign)
        {
            if (string.IsNullOrEmpty(privateKey))
            {
                throw new Exception("You must provide a private key to sign your data");
            }
            if (string.IsNullOrEmpty(dataToSign))
            {
                throw new Exception("You must provide data to sign with the private key");
            }

            if (this._platform == OSPlatform.Linux)
            {
                IntPtr signedPtr = RSALinuxWrapper.rsa_sign_with_key(privateKey, dataToSign);
                string signed = Marshal.PtrToStringAnsi(signedPtr);
                RSALinuxWrapper.free_cstring(signedPtr);
                return signed;
            }
            else
            {
                IntPtr signedPtr = RSAWindowsWrapper.rsa_sign_with_key(privateKey, dataToSign);
                string signed = Marshal.PtrToStringAnsi(signedPtr);
                RSAWindowsWrapper.free_cstring(signedPtr);
                return signed;
            }
        }

        public byte[] RsaSignWithKeyBytes(string privateKey, byte[] dataToSign)
        {
            if (string.IsNullOrEmpty(privateKey))
            {
                throw new Exception("You must provide a private key to sign with RSA");
            }
            if (dataToSign == null || dataToSign.Length == 0)
            {
                throw new Exception("You must provide allocated data to sign with RSA");
            }

            if (this._platform == OSPlatform.Linux)
            {
                RsaSignBytesResults signResult = RSALinuxWrapper.rsa_sign_with_key_bytes(privateKey, dataToSign, dataToSign.Length);
                byte[] result = new byte[signResult.length];
                Marshal.Copy(signResult.signature_raw_ptr, result, 0, signResult.length);
                RSALinuxWrapper.free_bytes(signResult.signature_raw_ptr);
                return result;
            }
            else
            {
                RsaSignBytesResults signResult = RSAWindowsWrapper.rsa_sign_with_key_bytes(privateKey, dataToSign, dataToSign.Length);
                byte[] result = new byte[signResult.length];
                Marshal.Copy(signResult.signature_raw_ptr, result, 0, signResult.length);
                RSAWindowsWrapper.free_bytes(signResult.signature_raw_ptr);
                return result;
            }
        }
        public bool RsaVerify(string publicKey, string dataToVerify, string signature)
        {
            if (string.IsNullOrEmpty(publicKey))
            {
                throw new Exception("You must provide a public key to verify the rsa signature");
            }
            if (string.IsNullOrEmpty(dataToVerify))
            {
                throw new Exception("You must provide the original data to verify the rsa signature");
            }
            if (string.IsNullOrEmpty(dataToVerify))
            {
                throw new Exception("You must provide that digital signature that was provided by our signing");
            }

            if (this._platform == OSPlatform.Linux)
            {
                return RSALinuxWrapper.rsa_verify(publicKey, dataToVerify, signature);
            }
            else
            {
                return RSAWindowsWrapper.rsa_verify(publicKey, dataToVerify, signature);
            }
        }

        public bool RsaVerifyBytes(string publicKey, byte[] dataToVerify, byte[] signature)
        {
            if (string.IsNullOrEmpty(publicKey))
            {
                throw new Exception("You must provide a public key to verify with RSA");
            }
            if (dataToVerify == null || dataToVerify.Length == 0)
            {
                throw new Exception("You must provide allocated data to verify with RSA");
            }
            if (signature == null || signature.Length == 0)
            {
                throw new Exception("You must provide an allocated signature to verify with RSA");
            }
            if (this._platform == OSPlatform.Linux)
            {
                return RSALinuxWrapper.rsa_verify_bytes(publicKey, dataToVerify, dataToVerify.Length, signature, signature.Length);
            }
            else
            {
                return RSAWindowsWrapper.rsa_verify_bytes(publicKey, dataToVerify, dataToVerify.Length, signature, signature.Length);
            }
        }

        public RsaSignResult RsaSign(string dataToSign, int keySize)
        {
            if (string.IsNullOrEmpty(dataToSign))
            {
                throw new Exception("You must provide data to sign with RSA");
            }
            if (keySize != 1024 && keySize != 2048 && keySize != 4096)
            {
                throw new Exception("You must provide a valid key bit size to sign with RSA");
            }

            if (this._platform == OSPlatform.Linux)
            {
                RsaSignResultStruct resultPtrStruct = RSALinuxWrapper.rsa_sign(dataToSign, keySize);
                RsaSignResult signed = new RsaSignResult()
                {
                    PublicKey = Marshal.PtrToStringAnsi(resultPtrStruct.public_key),
                    Signature = Marshal.PtrToStringAnsi(resultPtrStruct.signature)
                };
                RSALinuxWrapper.free_cstring(resultPtrStruct.public_key);
                RSALinuxWrapper.free_cstring(resultPtrStruct.signature);
                return signed;
            }
            else
            {
                RsaSignResultStruct resultPtrStruct = RSAWindowsWrapper.rsa_sign(dataToSign, keySize);
                RsaSignResult signed = new RsaSignResult()
                {
                    PublicKey = Marshal.PtrToStringAnsi(resultPtrStruct.public_key),
                    Signature = Marshal.PtrToStringAnsi(resultPtrStruct.signature)
                };
                RSAWindowsWrapper.free_cstring(resultPtrStruct.public_key);
                RSAWindowsWrapper.free_cstring(resultPtrStruct.signature);
                return signed;
            }
        }
        public string RsaDecrypt(string privateKey, string dataToDecrypt)
        {
            if (string.IsNullOrEmpty(privateKey) || string.IsNullOrEmpty(dataToDecrypt))
            {
                throw new Exception("You need to provide a private key and data to decrypt to use RsaCrypt");
            }

            if (this._platform == OSPlatform.Linux)
            {
                IntPtr decryptPtr = RSALinuxWrapper.rsa_decrypt(privateKey, dataToDecrypt);
                string decrypt = Marshal.PtrToStringAnsi(decryptPtr);
                RSALinuxWrapper.free_cstring(decryptPtr);
                return decrypt;
            }
            else
            {
                IntPtr decryptPtr = RSAWindowsWrapper.rsa_decrypt(privateKey, dataToDecrypt);
                string decrypt = Marshal.PtrToStringAnsi(decryptPtr);
                RSAWindowsWrapper.free_cstring(decryptPtr);
                return decrypt;
            }
        }

        public byte[] RsaDecryptBytes(string privateKey, byte[] dataToDecrypt)
        {
            if (string.IsNullOrEmpty(privateKey))
            {
                throw new Exception("You must provide a public key to decrypt with RSA");
            }
            if (dataToDecrypt == null || dataToDecrypt.Length == 0)
            {
                throw new Exception("You must provide allocated data to decrypt with RSA");
            }

            if (this._platform == OSPlatform.Linux)
            {
                RsaDecryptBytesResult decryptResult = RSALinuxWrapper.rsa_decrypt_bytes(privateKey, dataToDecrypt, dataToDecrypt.Length);
                byte[] result = new byte[decryptResult.length];
                Marshal.Copy(decryptResult.decrypted_result_ptr, result, 0, decryptResult.length);
                RSALinuxWrapper.free_bytes(decryptResult.decrypted_result_ptr);
                return result;
            }
            else
            {
                RsaDecryptBytesResult decryptResult = RSAWindowsWrapper.rsa_decrypt_bytes(privateKey, dataToDecrypt, dataToDecrypt.Length);
                byte[] result = new byte[decryptResult.length];
                Marshal.Copy(decryptResult.decrypted_result_ptr, result, 0, decryptResult.length);
                RSAWindowsWrapper.free_bytes(decryptResult.decrypted_result_ptr);
                string testing = Encoding.UTF8.GetString(result);
                return result;
            }
        }

        public string RsaEncrypt(string publicKey, string dataToEncrypt)
        {
            if (string.IsNullOrEmpty(publicKey) || string.IsNullOrEmpty(dataToEncrypt))
            {
                throw new Exception("You need to provide a public key and data to encrypt to use RsaEncrypt");
            }

            if (this._platform == OSPlatform.Linux)
            {
                IntPtr encryptPtr = RSALinuxWrapper.rsa_encrypt(publicKey, dataToEncrypt);
                string encrypt = Marshal.PtrToStringAnsi(encryptPtr);
                RSALinuxWrapper.free_cstring(encryptPtr);
                return encrypt;
            }
            else
            {
                IntPtr encryptPtr = RSAWindowsWrapper.rsa_encrypt(publicKey, dataToEncrypt);
                string encrypt = Marshal.PtrToStringAnsi(encryptPtr);
                RSAWindowsWrapper.free_cstring(encryptPtr);
                return encrypt;
            }
        }

        public byte[] RsaEncryptBytes(string publicKey, byte[] dataToEncrypt)
        {
            if (string.IsNullOrEmpty(publicKey))
            {
                throw new Exception("You must provide a public key to encryp with RSA");
            }
            if (dataToEncrypt == null || dataToEncrypt.Length == 0)
            {
                throw new Exception("You must provide allocated data to encrypt with RSA");
            } 

            if (this._platform == OSPlatform.Linux)
            {
                RsaEncryptBytesResult encryptResult = RSALinuxWrapper.rsa_encrypt_bytes(publicKey, dataToEncrypt, dataToEncrypt.Length);
                byte[] result = new byte[encryptResult.length];
                Marshal.Copy(encryptResult.encrypted_result_ptr, result, 0, encryptResult.length);
                RSALinuxWrapper.free_bytes(encryptResult.encrypted_result_ptr);
                return result;
            }
            else
            {
                RsaEncryptBytesResult encryptResult = RSAWindowsWrapper.rsa_encrypt_bytes(publicKey, dataToEncrypt, dataToEncrypt.Length);
                byte[] result = new byte[encryptResult.length];
                Marshal.Copy(encryptResult.encrypted_result_ptr, result, 0, encryptResult.length);
                RSAWindowsWrapper.free_bytes(encryptResult.encrypted_result_ptr);
                return result;
            }
        }

        public RsaKeyPairResult GetKeyPair(int keySize)
        {
            if (keySize != 1024 && keySize != 2048 && keySize != 4096)
            {
                throw new Exception("Please pass in a valid key size.");
            }

            if (this._platform == OSPlatform.Linux)
            {
                RustRsaKeyPairStruct keyPairStruct = RSALinuxWrapper.get_key_pair(keySize);
                RsaKeyPairResult result = new RsaKeyPairResult()
                {
                    PrivateKey = Marshal.PtrToStringAnsi(keyPairStruct.priv_key),
                    PublicKey = Marshal.PtrToStringAnsi(keyPairStruct.pub_key)
                };
                RSALinuxWrapper.free_cstring(keyPairStruct.pub_key);
                RSALinuxWrapper.free_cstring(keyPairStruct.priv_key);
                return result;
            }
            else
            {
                RustRsaKeyPairStruct keyPairStruct = RSAWindowsWrapper.get_key_pair(keySize);
                RsaKeyPairResult result = new RsaKeyPairResult()
                {
                    PrivateKey = Marshal.PtrToStringAnsi(keyPairStruct.priv_key),
                    PublicKey = Marshal.PtrToStringAnsi(keyPairStruct.pub_key)
                };
                RSAWindowsWrapper.free_cstring(keyPairStruct.pub_key);
                RSAWindowsWrapper.free_cstring(keyPairStruct.priv_key);
                return result;
            }
        }
    }
}
