using System.Collections.Concurrent;

namespace SocialFrontEnd.Services.OtpService
{
    public class OtpService : IOtpService
    {
        private readonly ConcurrentDictionary<string, string> _otpStore = new ConcurrentDictionary<string, string>();

        public void GenerateOtp(string email, out string otp)
        {
            otp = new Random().Next(100000, 999999).ToString();
            _otpStore[email] = otp;
        }

        public bool ValidateOtp(string email, string otp)
        {
            if (_otpStore.TryGetValue(email, out var storedOtp))
            {
                return storedOtp == otp;
            }

            return false;
        }
    }
}
