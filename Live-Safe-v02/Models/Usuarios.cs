using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Live_Safe_v02.Models {
    [Table("Usuarios")]
    public class Usuarios {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        [MaxLength(100, ErrorMessage = "O campo Nome recebe no máximo 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo E-mail é inválido")]
        public string Email { get; set; }


        [Required(ErrorMessage = "O campo Senha é obrigatório")]
        [DataType(DataType.Password)]
        [MaxLength(100, ErrorMessage = "O campo Senha recebe no máximo 100 caracteres")]
        public string Senha { get; set; }

        
        [Required(ErrorMessage = "O campo Perfil é obrigatório")]
        public Perfil Perfil { get; set; }
    }

    public enum Perfil {        
        Usuario,
        Administrador
    }
}
