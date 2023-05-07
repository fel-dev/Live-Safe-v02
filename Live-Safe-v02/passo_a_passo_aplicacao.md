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

## Fim da configuração do Entity Framework

---

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
    Controller>Adicionar>Controlador MVC com exibições, usando o Entity Framework (Com isso, icriará tanto o controlador quanto a view, que utiliza bootstrap)

    Classe Expostos

    Contexto de dados do aplicativo ApplicationDbContext
    Nome do controlador "ExpostosController" (colocar em português)

    Com isso, criará todas as opções de CRUD + View
```

## 10 - Analisando as Views do Controller Expostos

## _Master Page_

_pode ser feito com a aplicação rodando_

`Views>Shared>_Layout.cshtml`

>Substituir `Privacy` para: `Expostos`

```html
@* Talvez isso não faça sentido exibir, mas depois eu vejo.. zZzZ...*@
    <li class="nav-item">
        <a class="nav-link text-dark" asp-area="" asp-controller="Expostos" asp-action="Index">Expostos</a>
    </li>
```
_As views podem ser alteradas enquanto está rodando a aplicação, mas quando for mexer em `código`, vai precisa reiniciar pra surgir efeito na `lógica`_

--- 
## Pulando...
### OBS: Pulando algumas partes, **mas não deixe de ler** ☺

> **A parte de cadastramento de `Consumo do veículo` vou pular. É a mesma coisa que foi feita com `Expostos` e `Consumo do veículo` não é o foco do projeto.**

**Obs:** Ordenar uma lista de forma decrescente (Data) no foreach (vou pular ess parte também, só pra saber, se quiser colocar e não tiver ideia de como fazer)

```html
@foreach (var item in Model.OrderByDescending(x => x.Data)) {
    <tr>
        <td>
            @Html.DisplayFor(modelItem => item.Email)
        </td>
        <td>
            @Html.DisplayFor(modelItem => item.Origem)
        </td>
        <td>
            @Html.DisplayFor(modelItem => item.Data)
        </td>
        <td>
            <a asp-action="Edit" asp-route-id="@item.Id">Editar</a> |
            <a asp-action="Details" asp-route-id="@item.Id">Detalhes</a> |
            <a asp-action="Delete" asp-route-id="@item.Id">Excluir</a>
        </td>
    </tr>
}
```

## Fim do pulo

# Segurança da Aplicação
## Controle de Usuário
### Criando Model Usuário

AspNet Core recurso: Identity (forma simplificada)

*Da pra fazer muita coisa aqui, como conectar via Google etc, mas só tem o básico nesse walkthrough*

`Models` > _Nova Classe_ `Usuario.cs`

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Live_Safe_v02.Models {
    [Table("Usuarios")]
    public class Usuarios {

        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        public  Perfil Perfil { get; set; }
    }

    public enum Perfil {
        Administrador,
        Usuario
    }
}
```
## Configurando o Contexto de Dados do Usuário

`Models` > Clicar `ApplicationDbContext.cs` depois de `Expostos`

```csharp
    public DbSet<Usuarios> Usuarios { get; set; }
```

## Gerando o Banco de Dados do Usuário
*NOTA*: **Pare** a aplicação antes de disso:
```
    PM> Add-Migration M01 (visto q pulei a M00 q seria consumo do veículo)
    PM> Update-Database
```	
>Pode conferir no `SQL Server Object Explorer` se o banco de dados foi criado e se a tabela `Usuarios` foi criada com as colunas `Id`, `Nome`, `Email`, `Senha` e `Perfil`. Se quiser! Se não, fF*oda-se. Já são 6:10 da manhã e eu to acordado desde as 7:00 de ontem. Vou dormir. Boa noite. #sqn* :´-(

**Vamos lá!**
## Criando o Controller Usuário

```
    Controller>Adicionar>Controlador MVC com exibições, usando o Entity Framework (CRUD completo)

    Classe do modelo: Usuarios (Live_Safe_v02.Models)

    Contexto de dados do aplicativo: ApplicationDbContext (Live_Safe_v02.Models)

    Nome: UsuariosController (colocar em português)

    > Executar aplicação <
```
https:// localhost: < PORTA >/Usuarios

Se der certo, significa que não deu errado. Parabnéns! Você é um programador de verdade! :D

## Criando a View de Cadastro de Usuário

`Views>Shared>_Layout.cshtml`

- Colocar nova opção usuário na lista do menú de acesso à dados

```html
    <li class="nav-item">
        <a class="nav-link text-dark" asp-area="" asp-controller="Usuarios" asp-action="Index">Usuario</a>
    </li>
```

- Habilitar a opção de usuário na lista de tipo em `Views>Usuarios>Create.cshtml`
*procurar pelo único `<select>` da página está no final
Isso serve para criar um usuário com perfil de administrador ou usuário comum*
e repetir: 

        asp-items="Html.GetEnumSelectList<Perfil>()"

em `Views>Usuarios>Edit.cshtml`

```html
<select asp-for="Perfil" class="form-control" asp-items="Html.GetEnumSelectList<Perfil>()"></select>
```

## Configurando a Cripitografia da Senha

- `Models>Usuario.cs` colocar anotação `[DataType(DataType.Password)]` na propriedade `Senha`

```csharp
    [DataType(DataType.Password)]
    public string Senha { get; set; }
```

- Precisa atualizar o Db? Não sei. Vou fazer isso mesmo assim.

```
    PM> Add-Migration M02
    PM> Update-Database
```

- Armarzenar a senha no banco de dados ( e criptografar usando o Hash)
  - baixar no `Gerenciador de Pacotes Nuget Para Solução`: (**fechar** aplicação antes disso) -- *Baixar* `BCrypt.Net-Next`


## Alterando a criptografia da senha

Controllers>UsuariosController.cs e achar onde pega a senha (q é no Create)

`usuarios.Senha = BCrypt.Net.BCrypt.HashPassword(usuarios.Senha);`

Tipo assim q fik:

```csharp
        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,Senha,Perfil")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                // Antes de salvar a senha no banco de dados, criptografar a senha
                usuarios.Senha = BCrypt.Net.BCrypt.HashPassword(usuarios.Senha);
                // Salvar a senha criptografada no banco de dados
                _context.Add(usuarios);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuarios);
        }
```

- Repetir isso tb no edit
    
    ```csharp
            // POST: Usuarios/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,Senha,Perfil")] Usuarios usuarios)
            {
                if (id != usuarios.Id)
                {
                    return NotFound();
                }
    
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Antes de salvar a senha no banco de dados, criptografar a senha
                        usuarios.Senha = BCrypt.Net.BCrypt.HashPassword(usuarios.Senha);
                        // Salvar a senha criptografada no banco de dados
                        _context.Update(usuarios);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UsuariosExists(usuarios.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(usuarios);
            }
    ```

    ## Remover a exibição da senha(hash) na lista de usuários

    - Remover tanto em `Details` quanto no `Index`

        `Views>Usuarios>Details.cshtml`

```html
<dt>
    ~~@Html.DisplayNameFor(model => model.Senha)~~
    ********
</dt>
```

## Criando a View de Login e Logout do Usuário pt1

`Controllers>UsuariosControllers.cs`

public IActionResult Login()

```csharp
        [AllowAnonymous] // <--- Anotação para liberar o acesso sem login
        public IActionResult Login() {
            return View();
        }
```
- Parar aplicação.
- **BDM** em `Login()` > add exibição view do Razor;
  - nome: Login 
  - modelo: Create
  - classe do modelo: Usuarios(Live_Safe_v02.Models)
  - Click em `Adicionar`

## Criando a View de Login e Logout do Usuário pt2
- Executar aplicação
 - Adicionar link para login no menu de navegação
    - `Views>Shared>_Layout.cshtml`
    - _tipo_ assim:

```html
        <li class="nav-item">
            <a class="nav-link text-dark" asp-area="" asp-controller="Usuarios" asp-action="Login">Sign In</a>
        </li>
    </ul>
</div>
</div>
</nav>
```

- Em: `Views>Usuarios>Login.cshtml` remover o campo `Nome` e `Perfil` e deixar apenas `Email` e `Senha`.

- o botão de `Criar` mudar para `Entrar`
- o botão de `Voltar` mudar para `Você já tem uma conta?` e apontar para `Login`
    - pode querer fazer algo parecido em `Create`. Fica a seu critério.

# Unidade 2 - Segurança
#### ASP.NET CORE Identity
## Configurando da lógica do Identity
Implementação necessária para o Identity funcionar

copiar o public IActionResult Login() do `UsuariosController.cs` e colar abaixo dele mesmo, mas com uma anotação `[HttpPost]` e passando esses parametros Email, Senha.

```csharp
        [AllowAnonymous] // <--- Anotação para liberar o acesso sem login
        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        [AllowAnonymous] // <--- Anotação para liberar o acesso sem login
        public async Task<IActionResult> Login([Bind("Email,Senha")] Usuarios usuario) {
            var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == usuario.Email);
            if (usuario == null) {
                ViewBag.Erro = "Usuário não encontrado";
                return View();
            }
            bool senhaCorreta = BCrypt.Net.BCrypt.Verify(usuario.Senha, usuario.Senha);
            if (senhaCorreta) {
                ViewBag.Message = "Usuário ok";
                return View();
            }
            ViewBag.Erro = "Senha incorreta";
        }
```

- Em nossa página de Login precisa exibir a mensagem pra gente debugar. Então: abaixo do formulário prinicpal, adicionar:

```html
    <div class="row">
        <div class="col-md-12">
            @ViewBag.Message
            @ViewBag.Erro
        </div>
    </div>
```

## Configuração do Identity em si

- Se a senha estiver correta, precisa criar uma sessão para o usuário. Para isso, precisamos configurar o Identity.

```csharp
if (senhaCorreta) {
    // Cria a credencial que ficará no cashe da aplicação
    // https://docs.microsoft.com/pt-br/aspnet/core/security/authentication/identity?view=aspnetcore-5.0&tabs=visual-studio
    var claims = new List<Claim> {
        new Claim(ClaimTypes.Name, user.Nome),
        new Claim(ClaimTypes.NameIdentifier, user.Nome),
        new Claim(ClaimTypes.Role, user.perfil.ToString())
    };

    // Criar a validação desses dados
    var userIdentity = new ClaimsIdentity(claims, "login");

    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

    // Configurando o tempo de expiração do cookie
    var props = new AuthenticationProperties {
        AllowRefresh = true,
        ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
        IsPersistent = false
    };

    // Inclusão do usuário na sessão da aplicação
    await HttpContext.SignInAsync(principal, props);

    return Redirect("/"); // Se der tudo ok, var redirecionar paro Home, desta vez autenticado.

    //ViewBag.Message = "Usuário encontrado!";
    //return View();
}

ViewBag.Message = "Usuário e/ou Senha não encontrado!";
return View();
}

// Acesso negado
[AllowAnonymous] // <--- Anotação para liberar o acesso sem login
public IActionResult AccessDenied() {
return View();
}
```

- para mostrar o nome do usuário logado, precisamos alterar o `Views>Shared>_Layout.cshtml` e adicionar o seguinte código antes da lista:

```html
<ul class="navbar-nav flex-grow-1">
    <li class="nav-item">
        <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="Index">Home</a>
    </li>

    @if (User.Identity.IsAuthenticated) 
    {

    @*Só enxerga se o usuário estiver logado*@

    @if (User.IsInRole("Admin"))
        {
            <li class="nav-item">
                <a class="nav-link text-dark" asp-area="" asp-controller="Usuarios" asp-action="Index">Lista de Usuários</a>
            </li>
        }
    }                   
</ul>
<ul class="navbar-nav">
    
    @*Verifica se o usuário está logado na aplicação*@
    @if (User.Identity.IsAuthenticated) 
    {
        @* Vai aprensentar o nome do usuário *@
        <li class="nav-item">
            <a class="nav-link text-dark" asp-area="" asp-controller="Usuarios" asp-action="Logout">Olá, @User.Identity.Name!</a>
            <a class="nav-link text-dark" asp-area="" asp-controller="Usuarios" asp-action="Logout">Logout</a>
        </li>

    } 
    else 
    {
        @* Vai apresentar o link para o usuário logar *@
        <li class="nav-item">
            <a class="nav-link text-dark" asp-area="" asp-controller="Usuarios" asp-action="Login">Login</a>
        </li>
    }                        
</ul>
```

Precisa fazer alteração no `Startup.cs` para que o Identity funcione

```csharp
public void ConfigureServices(IServiceCollection services) {
    
    services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
    );

    // para fazer configuração de senha direto do site da MS mesmo.
    // https://docs.microsoft.com/pt-br/aspnet/core/security/gdpr?view=aspnetcore-5.0
    services.Configure<CookiePolicyOptions>(options => {
        // This lambda determines whether user consent for non-essential cookies is needed for a given request.                
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
    });

    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options => {
            options.AccessDeniedPath = "/Usuarios/AccessDenied";
            options.LoginPath = "/Usuarios/Login";                    
    });

    services.AddControllersWithViews();
}

```

essa ordem é importante tbm fica em ´startup.cs´

```csharp
    app.UseCookiePolicy();
    app.UseAuthentication();
    app.UseAuthorization();
```

- com isso, falta criar a página de acesso negado e a de logout do usuário. Vamos lá!
Criar o código abaixo em `Controllers>UsuariosController.cs`
- Clicar BDM em `AccessDenied` e `Logout` e criar a view do Razor (só add, sem configurar nada).

```csharp
        // Acesso negado
        [AllowAnonymous] // <--- Anotação para liberar o acesso sem login
        public IActionResult AccessDenied() {
            return View();
        }

        // Logout redirecionando pro login
        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Usuarios");
        }
```
