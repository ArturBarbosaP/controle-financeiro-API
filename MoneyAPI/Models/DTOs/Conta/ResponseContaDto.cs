namespace MoneyAPI.Models.DTOs.Conta
{
    public class ResponseContaDto
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public decimal Saldo { get; set; }
    }
}