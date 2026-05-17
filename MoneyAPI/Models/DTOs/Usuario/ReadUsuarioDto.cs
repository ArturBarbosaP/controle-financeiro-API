using MoneyAPI.Helpers.Attributes;
using MoneyAPI.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace MoneyAPI.Models.DTOs.Usuario
{
    public class ReadUsuarioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public string NomeUsuario { get; set; }
    }
}