namespace MoneyAPI.Models.DTOs.Limite
{
    public class ResponseLimiteDto
    {
        public int Id { get; set; }

        public decimal ValorLimite { get; set; }

        public int CategoriaId { get; set; }

        public decimal ValorGasto { get; set; }

        public decimal ValorRestante { get; set; }

        public string CategoriaNome { get; set; }

        public string CategoriaCor { get; set; }
    }
}