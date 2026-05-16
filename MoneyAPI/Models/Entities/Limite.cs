namespace MoneyAPI.Models.Entities
{
    public class Limite : BaseEntity
    {
        public decimal ValorLimite { get; set; }

        public int CategoriaId { get; set; }

        public Categoria Categoria { get; set; }
    }
}