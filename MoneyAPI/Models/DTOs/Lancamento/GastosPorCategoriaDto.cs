namespace MoneyAPI.Models.DTOs.Lancamento
{
    public class GastosPorCategoriaDto
    {
        public string CategoriaNome { get; set; } = string.Empty;
        public string CategoriaCor { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
    }
}