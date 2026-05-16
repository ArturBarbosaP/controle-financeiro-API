namespace MoneyAPI.Models.Entities
{
    public class Limite
    {
        public int Id { get; set; }
        public decimal ValorLimite { get; set; }

        public int CategoriaId { get; set; }

        public Categoria Categoria { get; set; }
    }
}