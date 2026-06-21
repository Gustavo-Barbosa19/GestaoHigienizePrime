# Higienize Prime - Sistema de Gestão

## 📋 SUMÁRIO

1. [Pré-requisitos](#pré-requisitos)
2. [Configuração do Google Sheets](#configuração-do-google-sheets)
3. [Configuração do Google Apps Script](#configuração-do-google-apps-script)
4. [Execução Local](#execução-local)
5. [Publicação no Render](#publicação-no-render)
6. [Estrutura do Projeto](#estrutura-do-projeto)
7. [Primeiro Acesso](#primeiro-acesso)

---

## 🔧 PRÉ-REQUISITOS

- Windows 10/11, Linux ou macOS
- .NET 8 SDK ([download](https://dotnet.microsoft.com/download/dotnet/8.0))
- Visual Studio Code ou Visual Studio 2022+
- Conta Google (para Google Sheets e Apps Script)
- Conta no Render (para hospedagem)

---

## 📊 CONFIGURAÇÃO DO GOOGLE SHEETS

1. Acesse [Google Sheets](https://sheets.google.com)
2. Crie uma nova planilha chamada **HigienizePrime**
3. Crie as seguintes abas (sheets):
   - `Usuarios`
   - `Clientes`
   - `Servicos`
   - `Financeiro`
   - `TipoServicos`

### Estrutura das Abas

**Usuarios**: Id | NomeUsuario | SenhaHash | Email | NomeCompleto | DataCriacao | Ativo

**Clientes**: Id | Nome | Telefone | WhatsApp | Email | CEP | Endereco | Numero | Bairro | Cidade | Estado | Observacoes | DataCadastro | Ativo

**Servicos**: Id | ClienteId | ClienteNome | DataAtendimento | Horario | TipoServico | QuantidadeItens | Valor | FormaPagamento | Status | Observacoes | DataCriacao

**Financeiro**: Id | Tipo | Categoria | Descricao | Valor | Data | ServicoId | DataCriacao

**TipoServicos**: Id | Nome | Descricao | Ativo

> As linhas de cabeçalho serão criadas automaticamente pelo sistema.

---

## ⚙️ CONFIGURAÇÃO DO GOOGLE APPS SCRIPT

1. Na planilha criada, vá em **Extensões > Apps Script**
2. Nomeie o projeto como `HigienizePrimeAPI`
3. Substitua o conteúdo do arquivo `Code.gs` pelo conteúdo do arquivo `GoogleAppsScript/Code.gs`
4. Salve (Ctrl+S)
5. **Implantar > Nova implantação**
6. Tipo: **Web App**
7. Execute como: **Eu**
8. Quem tem acesso: **Qualquer pessoa**
9. Clique em **Implantar**
10. Copie a **URL do Web App** gerada
11. **IMPORTANTE**: Autorize o aplicativo quando solicitado

---

## 🖥️ EXECUÇÃO LOCAL

### 1. Configurar a URL da API

Edite o arquivo `appsettings.json` e substitua `SEU_SCRIPT_ID` pela URL copiada:

```json
"GoogleSheets": {
  "ApiBaseUrl": "https://script.google.com/macros/s/SEU_SCRIPT_ID_AQUI/exec",
  ...
}
```

### 2. Restaurar e Executar

```bash
cd GestaoHigienizePrime
dotnet restore
dotnet run
```

Acesse: https://localhost:5001

---

## ☁️ PUBLICAÇÃO NO RENDER

### Preparação

1. Crie um repositório no GitHub com o projeto
2. Faça o push do código

### No Render

1. Acesse [dashboard.render.com](https://dashboard.render.com)
2. Clique em **New + > Web Service**
3. Conecte seu repositório GitHub
4. Configure:

```
Name: higienize-prime
Environment: .NET 8
Build Command: dotnet restore && dotnet publish -c Release -o out
Start Command: dotnet out/GestaoHigienizePrime.dll
Health Check Path: /
```

5. Em **Advanced > Environment Variables**, adicione:

```
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://0.0.0.0:8080
```

6. Clique em **Create Web Service**

> O Render detecta automaticamente o ASP.NET Core 8.

### Arquivo render.yaml (deploy automático)

```yaml
services:
  - type: web
    name: higienize-prime
    env: dotnet
    buildCommand: dotnet restore && dotnet publish -c Release -o out
    startCommand: dotnet out/GestaoHigienizePrime.dll
    envVars:
      - key: ASPNETCORE_ENVIRONMENT
        value: Production
      - key: ASPNETCORE_URLS
        value: http://0.0.0.0:8080
```

---

## 🚀 PRIMEIRO ACESSO

1. Acesse o sistema (local ou hospedado)
2. Na primeira execução, o sistema criará automaticamente os tipos de serviço padrão
3. **Login inicial**: será necessário criar um usuário manualmente na planilha para teste, ou o sistema pode ser acessado com credenciais configuradas diretamente

### Criar primeiro usuário (manual)

Na aba `Usuarios` da planilha, adicione uma linha com:
- Id: `1`
- NomeUsuario: `admin`
- SenhaHash: Use o hash SHA256 da senha desejada (ex: admin123)
- Email: `admin@higienizeprime.com`
- NomeCompleto: `Administrador`
- DataCriacao: `=NOW()`
- Ativo: `TRUE`

Para gerar o hash SHA256 de uma senha, use sites como https://www.md5hashgenerator.com/ (opção SHA256).

---

## 📁 ESTRUTURA DO PROJETO

```
GestaoHigienizePrime/
├── Controllers/       # Controladores MVC
│   ├── AuthController.cs
│   ├── ClientesController.cs
│   ├── DashboardController.cs
│   ├── FinanceiroController.cs
│   ├── HomeController.cs
│   ├── RelatoriosController.cs
│   ├── ServicosController.cs
│   └── TipoServicosController.cs
├── Models/            # Modelos de domínio
│   ├── Cliente.cs
│   ├── DashboardViewModel.cs
│   ├── Financeiro.cs
│   ├── Servico.cs
│   ├── TipoServico.cs
│   ├── Usuario.cs
│   └── Enums/
│       ├── StatusServico.cs
│       └── TipoTransacao.cs
├── ViewModels/        # ViewModels
│   ├── ClienteViewModel.cs
│   ├── FinanceiroViewModel.cs
│   ├── LoginViewModel.cs
│   ├── RelatorioViewModel.cs
│   └── ServicoViewModel.cs
├── Services/          # Camada de serviços
│   ├── AuthService.cs
│   ├── ClienteService.cs
│   ├── FinanceiroService.cs
│   ├── GoogleSheetsService.cs
│   ├── RelatorioService.cs
│   ├── ServicoService.cs
│   ├── TipoServicoService.cs
│   └── Interfaces (I*.cs)
├── Repositories/      # Repositório Google Sheets
│   ├── GoogleSheetsRepository.cs
│   └── IGoogleSheetsRepository.cs
├── Views/             # Views Razor
├── wwwroot/           # Arquivos estáticos
│   ├── css/site.css
│   └── js/site.js
├── GoogleAppsScript/  # Script para Google Sheets
│   └── Code.gs
├── Program.cs         # Entry point
├── appsettings.json   # Configurações
└── GestaoHigienizePrime.csproj
```

---

## 🔄 MIGRAÇÃO FUTURA PARA POSTGRESQL

Para migrar do Google Sheets para PostgreSQL:

1. Crie as tabelas correspondentes no PostgreSQL
2. Crie `PostgresRepository.cs` implementando `IGoogleSheetsRepository`
3. No `Program.cs`, troque `GoogleSheetsRepository` por `PostgresRepository`:

```csharp
// Antes (Google Sheets)
builder.Services.AddScoped<IGoogleSheetsRepository, GoogleSheetsRepository>();

// Depois (PostgreSQL)
// builder.Services.AddScoped<IGoogleSheetsRepository, PostgresRepository>();
```

A camada de Services permanece intacta, pois depende apenas da interface `IGoogleSheetsRepository`.

---

## 🛡️ SEGURANÇA

- Senhas armazenadas com hash SHA256
- Sessões com tempo de expiração (30 minutos)
- Rotas protegidas por verificação de sessão
- Validação de formulários client-side e server-side
- Anti-forgery tokens nos formulários
- Sanitização de entradas

---

## 📄 LICENÇA

Este projeto é proprietário da Higienize Prime.
