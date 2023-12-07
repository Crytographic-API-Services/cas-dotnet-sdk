﻿using EasDotnetSdk.Helpers;
using EasDotnetSdk.Signatures;
using System.Runtime.InteropServices;
using Xunit;
using static EasDotnetSdk.Signatures.ED25519Wrapper;

namespace EasDotnetSdk.Tests
{
    public class ED25519WrapperTests
    {
        private readonly ED25519Wrapper _wrapper;
        private readonly OperatingSystemDeterminator _operatingSystem;
        public ED25519WrapperTests()
        {
            this._wrapper = new ED25519Wrapper();
            this._operatingSystem = new OperatingSystemDeterminator();
        }

        [Fact]
        public void GetKeyPair()
        {
            OSPlatform platform = this._operatingSystem.GetOperatingSystem();
            if (platform == OSPlatform.Linux)
            {
                throw new NotImplementedException("Linux version not yet supported");
            }
            else
            {
                IntPtr keyPairPtr = this._wrapper.GetKeyPair();
                string keyPair = Marshal.PtrToStringAnsi(keyPairPtr);
                ED25519Wrapper.free_cstring(keyPairPtr);
                Assert.NotNull(keyPair);
            }
        }

        [Fact]
        public void SignData()
        {
            OSPlatform platform = this._operatingSystem.GetOperatingSystem();
            if (platform == OSPlatform.Linux)
            {
                throw new NotImplementedException("Linux version not yet supported");
            }
            else
            {
                IntPtr keyPairPtr = this._wrapper.GetKeyPair();
                string keyPair = Marshal.PtrToStringAnsi(keyPairPtr);
                Ed25519SignatureResult signedData = this._wrapper.Sign(keyPair, "SignThisData");
                string signature = Marshal.PtrToStringAnsi(signedData.Signature);
                string publicKey = Marshal.PtrToStringAnsi(signedData.Public_Key);
                ED25519Wrapper.free_cstring(keyPairPtr);
                ED25519Wrapper.free_cstring(signedData.Public_Key);
                ED25519Wrapper.free_cstring(signedData.Signature);
                Assert.NotNull(signature);
                Assert.NotNull(publicKey);
            }
        }

        [Fact]
        public void Verify()
        {
            OSPlatform platform = this._operatingSystem.GetOperatingSystem();
            if (platform == OSPlatform.Linux)
            {
                throw new NotImplementedException("Linux version not yet supported");
            }
            else
            {
                IntPtr keyPairPtr = this._wrapper.GetKeyPair();
                string dataToSign = "TestData12345";
                string keyPair = Marshal.PtrToStringAnsi(keyPairPtr);
                Ed25519SignatureResult signatureResult = this._wrapper.Sign(keyPair, dataToSign);
                string signature = Marshal.PtrToStringAnsi(signatureResult.Signature);
                bool isValid = this._wrapper.Verify(keyPair, signature, dataToSign);
                ED25519Wrapper.free_cstring(signatureResult.Signature);
                ED25519Wrapper.free_cstring(signatureResult.Public_Key);
                ED25519Wrapper.free_cstring(keyPairPtr);
                Assert.Equal(true, isValid);
            }
        }

        [Fact]
        public async void VerifyWithPublicKey()
        {
            OSPlatform platform = this._operatingSystem.GetOperatingSystem();
            if (platform == OSPlatform.Linux)
            {
                throw new NotImplementedException("Linux version not yet supported");
            }
            else
            {
                IntPtr keyPairPtr = this._wrapper.GetKeyPair();
                string dataToSign = "welcomeHome";
                string keyPair = Marshal.PtrToStringAnsi(keyPairPtr);
                Ed25519SignatureResult result = this._wrapper.Sign(keyPair, dataToSign);
                string publicKey = Marshal.PtrToStringAnsi(result.Public_Key);
                string siganture = Marshal.PtrToStringAnsi(result.Signature);
                bool isValid = this._wrapper.VerifyWithPublicKey(publicKey, siganture, dataToSign);
                ED25519Wrapper.free_cstring(keyPairPtr);
                ED25519Wrapper.free_cstring(result.Public_Key);
                ED25519Wrapper.free_cstring(result.Signature);
                Assert.Equal(true, isValid);
            }
        }
    }
}