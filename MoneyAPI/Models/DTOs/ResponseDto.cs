namespace MoneyAPI.Models.DTOs
{
    public class ResponseDto
    {
        public bool Sucesso { get; set; }
        public int StatusCode { get; set; }
        public string? Erro { get; set; }
        public object? Entidade { get; set; }
    }
}