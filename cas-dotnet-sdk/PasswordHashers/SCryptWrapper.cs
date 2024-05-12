﻿using CasDotnetSdk.Http;
using CasDotnetSdk.PasswordHashers.Linux;
using CasDotnetSdk.PasswordHashers.Windows;
using CASHelpers;
using CASHelpers.Types.HttpResponses.BenchmarkAPI;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CasDotnetSdk.PasswordHashers
{
    public class SCryptWrapper : IPasswordHasherBase
    {
        private readonly OSPlatform _platform;
        private readonly BenchmarkSender _benchmarkSender;
        public SCryptWrapper()
        {
            this._platform = new OperatingSystemDeterminator().GetOperatingSystem();
            this._benchmarkSender = new BenchmarkSender();
        }
        public string HashPassword(string passToHash)
        {
            if (string.IsNullOrEmpty(passToHash))
            {
                throw new Exception("Please provide a password to hash");
            }

            DateTime start = DateTime.UtcNow;
            if (this._platform == OSPlatform.Linux)
            {
                IntPtr hashedPtr = SCryptLinuxWrapper.scrypt_hash(passToHash);
                string hashed = Marshal.PtrToStringAnsi(hashedPtr);
                SCryptLinuxWrapper.free_cstring(hashedPtr);
                DateTime end = DateTime.UtcNow;
                this._benchmarkSender.SendNewBenchmarkMethod(MethodBase.GetCurrentMethod().Name, start, end, BenchmarkMethodType.Hash, nameof(SCryptWrapper));
                return hashed;
            }
            else
            {
                IntPtr hashedPtr = SCryptWindowsWrapper.scrypt_hash(passToHash);
                string hashed = Marshal.PtrToStringAnsi(hashedPtr);
                SCryptWindowsWrapper.free_cstring(hashedPtr);
                DateTime end = DateTime.UtcNow;
                this._benchmarkSender.SendNewBenchmarkMethod(MethodBase.GetCurrentMethod().Name, start, end, BenchmarkMethodType.Hash, nameof(SCryptWrapper));
                return hashed;
            }
        }

        /// <summary>
        /// Hashes the password using the SCrypt algorithm in a separate thread.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string HashPasswordThreadPool(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new Exception("Please provide a password to hash");
            }

            DateTime start = DateTime.UtcNow;
            if (this._platform == OSPlatform.Linux)
            {
                IntPtr hashedPtr = SCryptLinuxWrapper.scrypt_hash_threadpool(password);
                string hashed = Marshal.PtrToStringAnsi(hashedPtr);
                SCryptLinuxWrapper.free_cstring(hashedPtr);
                DateTime end = DateTime.UtcNow;
                this._benchmarkSender.SendNewBenchmarkMethod(MethodBase.GetCurrentMethod().Name, start, end, BenchmarkMethodType.Hash, nameof(SCryptWrapper));
                return hashed;
            }
            else
            {
                IntPtr hashedPtr = SCryptWindowsWrapper.scrypt_hash_threadpool(password);
                string hashed = Marshal.PtrToStringAnsi(hashedPtr);
                SCryptWindowsWrapper.free_cstring(hashedPtr);
                DateTime end = DateTime.UtcNow;
                this._benchmarkSender.SendNewBenchmarkMethod(MethodBase.GetCurrentMethod().Name, start, end, BenchmarkMethodType.Hash, nameof(SCryptWrapper));
                return hashed;
            }
        }

        /// <summary>
        /// Verifies an unhashed password against a hashed password using the SCrypt algorithm.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool Verify(string password, string hash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash))
            {
                throw new Exception("Please provide a password and a hash to verify");
            }

            DateTime start = DateTime.UtcNow;
            if (this._platform == OSPlatform.Linux)
            {
                bool result = SCryptLinuxWrapper.scrypt_verify(password, hash);
                DateTime end = DateTime.UtcNow;
                this._benchmarkSender.SendNewBenchmarkMethod(MethodBase.GetCurrentMethod().Name, start, end, BenchmarkMethodType.Hash, nameof(SCryptWrapper));
                return result;
            }
            else
            {

                bool result = SCryptWindowsWrapper.scrypt_verify(password, hash);
                DateTime end = DateTime.UtcNow;
                this._benchmarkSender.SendNewBenchmarkMethod(MethodBase.GetCurrentMethod().Name, start, end, BenchmarkMethodType.Hash, nameof(SCryptWrapper));
                return result;
            }
        }

        /// <summary>
        /// Verifies an unhashed password against a hashed password using the SCrypt algorithm in a separate thread.
        /// </summary>
        /// <param name="hashedPassword"></param>
        /// <param name="verifyPassword"></param>
        /// <returns></returns>
        public bool VerifyThreadPool(string hashedPassword, string verifyPassword)
        {
            if (string.IsNullOrEmpty(hashedPassword) || string.IsNullOrEmpty(verifyPassword))
            {
                throw new Exception("Please provide a password and a hash to verify");
            }

            DateTime start = DateTime.UtcNow;
            if (this._platform == OSPlatform.Linux)
            {
                bool result = SCryptLinuxWrapper.scrypt_verify_threadpool(verifyPassword, hashedPassword);
                DateTime end = DateTime.UtcNow;
                this._benchmarkSender.SendNewBenchmarkMethod(MethodBase.GetCurrentMethod().Name, start, end, BenchmarkMethodType.Hash, nameof(SCryptWrapper));
                return result;
            }
            else
            {
                bool result = SCryptWindowsWrapper.scrypt_verify_threadpool(verifyPassword, hashedPassword);
                DateTime end = DateTime.UtcNow;
                this._benchmarkSender.SendNewBenchmarkMethod(MethodBase.GetCurrentMethod().Name, start, end, BenchmarkMethodType.Hash, nameof(SCryptWrapper));
                return result;
            }
        }
    }
}