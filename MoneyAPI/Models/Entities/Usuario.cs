namespace MoneyAPI.Models.Entities
{
    public class Usuario : BaseEntity
    {
        public string Nome { get; set; }

        public string NomeUsuario { get; set; }

        public string Senha { get; set; }

        public List<Categoria> Categorias { get; set; }

        public List<Conta> Contas { get; set; }

        public List<Lancamento> Lancamentos { get; set; }
    }
}