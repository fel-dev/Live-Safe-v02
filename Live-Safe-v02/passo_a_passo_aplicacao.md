# Walkthrough

## Criar o projeto `Live_Safe_vXX` no Visual Studio

### Criar Classe: `Expostos.cs`

	Model>Classe>Expostos.cs

*Conteúdo da classe Expostos.cs*

```csharp
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
```

## Configurar o Entity Framework

    Model > criar Classe de contexto > ApplicationDbContext.cs

*conteúdo*

```csharp	
namespace Live_Safe_v02.Models {
    public class ApplicationDbContext : DbContext {
    }
}

```
Clicar no erro e instalar com o `Gerenciador de Pacotes NuGet`
- microsoft.entityframeworkcore
- microsoft.entityframeworkcore.tools 
- microsoft.entityframeworkcore.sqlserver

Voltar no código e importar as bibliotecas `dos erros`
e criar um `construtor`da classe àppDbContext` para fazer a **injeção de dependência**

```csharp
using Microsoft.EntityFrameworkCore;

namespace Live_Safe_v02.Models {
    public class ApplicationDbContext : DbContext {

        // constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}
```

# Fim da configuração do Entity Framework

## Configurar o Startup.cs
para que o Entity Framework possa ser utilizado

```csharp
public void ConfigureServices(IServiceCollection services) {
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
    );

    services.AddControllersWithViews();
}
```

## Configurar string de conexão com o banco de dados

No arquivo `appsettings.json` adicionar a string de conexão com o banco de dados (`conectionString`)
**Obs:** O nome da string de conexão deve ser o mesmo que foi utilizado no `Startup.cs`
**Obs2:** `Database=Live-Safe-v02` é o nome da aplicação

```json
  // Conection string for the database
"ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=Live-Safe-v02;Trusted_Connection=True;MultipleActiveResultSets=true"
  },

  "AllowedHosts": "*"
}
```

## Criar o banco de dados

Em `Models`, nova propriedate em `ApplicationDbContext.cs` tipo `DbSet` para criar a tabela no banco de dados e objeto `<"Exposto">` para referenciar a classe `Exposto.cs`

```csharp
// properties
public DbSet<Expostos> Expostos { get; set; }

```
## Criando BD-Nuget Package Manager Console
Ferramentas>Gerenciador de Pacotes NuGet>Console do Gerenciador de Pacotes.

Dar os comando de migração e atualização do banco de dados:

```
    PM> Add-Migration InitialCreate ou M00
    PM> Update-Database
```

## Criando  o Controller Expostos
Utilizar geração de código automático do Framework para criar o Controller e as Views

```
    Controller>Adicionar>Controlador MVC com exibições, usando o Entity Framework (irá criar tanto o controlador quanto a veiw, que utiliza bootstrap)
```