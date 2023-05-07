using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Live_Safe_v02.Models {
    // Classe para armazenar os emails e senhas dos usuários que foram expostos, além da data e origem dos mesmos
    
    [Table("Expostos")]
    public class Expostos {

        // Chave primária da tabela
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }

        // Perguntar ao professor se é necessário armazenar a senha
        //public string Senha { get; set; }
        //public DateTime Data { get; set; }
        public string Origem { get; set; }

    }
}
