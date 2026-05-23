namespace MoneyAPI.Helpers
{
    public class PasswordHelper
    {
        private const int workFactor = 13;

        /// <summary>
        ///Cria o hash da senha
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Uma string da senha em hash</returns>
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor);
        }

        /// <summary>
        /// Compara a senha com um hash
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hash"></param>
        /// <returns>True se a senha corresponde com o hash, caso contrário False</returns>
        public static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}