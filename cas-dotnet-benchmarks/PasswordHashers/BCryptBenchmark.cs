using BenchmarkDotNet.Attributes;
using CasDotnetSdk.PasswordHashers;

namespace cas_dotnet_benchmarks.PasswordHashers
{
    public class BCryptBenchmark
    {
        private BcryptWrapper _bcrypt { get; set; }
        private string _bcryptHash { get; set; }
        private string _password { get; set; }

        public BCryptBenchmark()
        {
            this._bcrypt = new BcryptWrapper();
            this._password = Util.GeneratePassword(15);
            this._bcryptHash = this._bcrypt.HashPassword(this._password);
        }

        [Benchmark]
        public string BCryptHash()
        {
            return this._bcrypt.HashPassword(this._password);
        }

        [Benchmark]
        public bool BCryptVerify()
        {
            return this._bcrypt.Verify(this._bcryptHash, this._password);
        }
    }
}
