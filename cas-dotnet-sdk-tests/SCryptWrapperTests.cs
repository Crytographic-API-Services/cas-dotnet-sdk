using CasDotnetSdk.PasswordHashers;
using Xunit;

namespace CasDotnetSdkTests.Tests
{
    public class SCryptWrapperTests
    {
        private readonly SCryptWrapper _scrypt;
        private readonly string _password;
        public SCryptWrapperTests()
        {
            this._scrypt = new SCryptWrapper();
            this._password = "TestPasswordToHash";
        }

        [Fact]
        public void HashPassword()
        {
            string hashedPassword = this._scrypt.HashPassword(this._password);
            Assert.NotNull(hashedPassword);
            Assert.NotEqual(hashedPassword, this._password);
        }

        [Fact]
        public void HashPasswordThreadPool()
        {
            string hashedPassword = this._scrypt.HashPasswordThreadPool(this._password);
            Assert.NotNull(hashedPassword);
            Assert.NotEqual(hashedPassword, this._password);
        }

        [Fact]
        public void VerifyPassword()
        {
            string hashedPassword = this._scrypt.HashPassword(this._password);
            bool isValid = this._scrypt.Verify(hashedPassword, this._password);
            Assert.True(isValid);
        }

        [Fact]
        public void VerifyPasswordThread()
        {
            string hashedPassword = this._scrypt.HashPasswordThreadPool(this._password);
            bool isValid = this._scrypt.VerifyThreadPool(hashedPassword, this._password);
        }

        [Fact]
        public void FactoryTest()
        {
            IPasswordHasherBase wrapper = PasswordHasherFactory.Get(PasswordHasherType.SCrypt);
            string badPassword = "Don't DO It";
            string hahed = wrapper.HashPassword(badPassword);
            Assert.NotNull(wrapper);
            Assert.NotEqual(badPassword, hahed);
            Assert.Equal(typeof(SCryptWrapper), wrapper.GetType());
        }

    }
}