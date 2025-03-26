namespace YemekSepeti.Functions
{
    public static class OTPGenerator
    {
        public static string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(1000, 10000).ToString(); 
        }
    }
}
