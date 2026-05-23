namespace MoneyAPI.Models.DTOs.Auth
{
    public class ResponseLoginDTO
    {
        public int UsuarioId { get; set; }

        public string Nome { get; set; }

        public string Token { get; set; }
    }
}