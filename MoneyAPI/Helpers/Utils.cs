using MoneyAPI.Models.Entities;

namespace MoneyAPI.Helpers
{
    public static class Utils
    {
        public static List<Categoria> categoriasPadrões =
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
    }
}