using BenchmarkDotNet.Attributes;
using CasDotnetSdk.PasswordHashers;

namespace cas_dotnet_benchmarks
{
    public class PasswordHasherBenchmarks
    {
        private string _password { get; set; }
        private string _argon2Hash { get; set; }
        private string _bcryptHash { get; set; }
        private string _scryptHash { get; set; }
        private Argon2Wrapper _argon2 { get; set; }
        private BcryptWrapper _bcrypt { get; set; }
        private SCryptWrapper _scrypt { get; set; }
        public PasswordHasherBenchmarks()
        {
            this._password = Util.GeneratePassword(15);
            this._argon2 = new Argon2Wrapper();
            this._scrypt = new SCryptWrapper();
            this._bcrypt = new BcryptWrapper();
            this._argon2Hash = this._argon2.HashPassword(this._password);
            this._bcryptHash = this._bcrypt.HashPassword(this._password);
            this._scryptHash = this._scrypt.HashPassword(this._password);
        }

        [Benchmark]
        public string Argon2Hash()
        {
            return this._argon2.HashPassword(this._password);
        }

        [Benchmark]
        public bool Argon2Verify()
        {
            return this._argon2.Verify(this._argon2Hash, this._password);
        }

        [Benchmark]
        public string SCryptHash()
        {
            return this._bcrypt.HashPassword(this._password);
        }

        [Benchmark]
        public bool SCryptVerify()
        {
            return this._scrypt.Verify(this._scryptHash, this._password);
        }
    }
}
