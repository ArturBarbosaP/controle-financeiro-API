using MoneyAPI.Models.Entities;

namespace MoneyAPI.Helpers
{
    public static class Utils
    {
        public static readonly List<Categoria> categoriasPadrões =
        [
            new Categoria
            {
                Nome = "Pagamento de fatura",
                Tipo = "Despesa",
                Cor = "#BCBCBC"
            },
            new Categoria
            {
                Nome = "Transferência",
                Tipo = "Transf.",
                Cor = "#BCBCBC"
            }
        ];

        public static readonly Dictionary<string, int> ordenacaoPadrao = new()
        {
            { "Receita", 1 },
            { "Transf.", 2 },
            { "Despesa", 3 }
        };
    }
}