namespace SocialFrontEnd.Services.OtpService
{
    public interface IOtpService
    {
        void GenerateOtp(string email, out string otp);
        bool ValidateOtp(string email, string otp);
    }
}
