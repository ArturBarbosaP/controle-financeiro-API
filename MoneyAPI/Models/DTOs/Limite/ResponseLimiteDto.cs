namespace MoneyAPI.Models.DTOs.Limite
{
    public class ResponseLimiteDto
    {
        public int Id { get; set; }

        public decimal ValorLimite { get; set; }

        public int CategoriaId { get; set; }

        public string? CategoriaNome { get; set; }
    }
}