
# Be3 Health Tech — Cadastro de Pacientes

Este projeto é uma aplicação completa para cadastro de pacientes, desenvolvida para o desafio técnico da Be3 Health Tech, incluindo **backend (.NET 8 + SQL Server)** e **frontend (Angular 15)**.

---

## Arquitetura Utilizada

- **Backend:** .NET 8, REST API, Dapper, SQL Server
- **Frontend:** Angular 15, Bootstrap, ngx-mask
- **Padrões:** Clean Code, SOLID, boas práticas REST, tratamento de erros amigáveis
- **Banco:** SQL Server com persistência relacional, uso de migrations e scripts SQL
- **Separação de camadas:** 
  - Backend e Frontend independentes (pasta `api/` e `cadastro-pacientes-web/`)
- **Validações:** 
  - CPF (com máscara)
  - Obrigatoriedade de campos
  - Formato de e-mail
  - Pelo menos um telefone (fixo ou celular)

---

## Instruções de Instalação e Execução

### 1. Clone o repositório

```bash
git clone https://github.com/AdrianFidelis/Be3-Health-Tech.git
cd Be3-Health-Tech
```

### 2. Configure o banco de dados

- É necessário ter o **SQL Server Express** instalado e rodando na máquina local.
- Crie o banco **CadastroPacientesDB** usando o script abaixo (ou via migration do projeto).

#### Script para criar o banco e tabelas:

```sql
CREATE DATABASE CadastroPacientesDB;
GO

USE CadastroPacientesDB;
GO

CREATE TABLE Convenios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL
);

CREATE TABLE Pacientes (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Sobrenome VARCHAR(100) NOT NULL,
    DataNascimento DATE NOT NULL,
    Genero VARCHAR(20) NOT NULL,
    CPF VARCHAR(14),
    RG VARCHAR(20) NOT NULL,
    UfRg VARCHAR(2) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Celular VARCHAR(20),
    TelefoneFixo VARCHAR(20),
    ConvenioId INT NOT NULL,
    NumeroCarteirinha VARCHAR(50) NOT NULL,
    ValidadeCarteirinha VARCHAR(7) NOT NULL,
    Ativo BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Pacientes_Convenios FOREIGN KEY (ConvenioId) REFERENCES Convenios(Id)
);
```

#### Popule a tabela de convênios (exemplo):

```sql
INSERT INTO Convenios (Nome) VALUES
('Plano Ouro'),
('Plano Prata'),
('Plano Bronze'),
('Plano Familiar'),
('Plano Empresarial');
```

---

### 3. Configurar a Connection String

- No projeto `api` (backend), edite o arquivo `appsettings.json` para apontar para sua instância SQL Server.  
Exemplo:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=NOTE-ADRIAN\SQLEXPRESS;Database=CadastroPacientesDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

---

### 4. Executando o Backend

Acesse a pasta do backend e rode:

```bash
dotnet restore
dotnet build
dotnet run
```
- A API estará disponível em `https://localhost:7139/swagger/index.html`

---

### 5. Executando o Frontend

Acesse a pasta do frontend:

```bash
cd cadastro-pacientes-web
npm install
ng serve
```
- Acesse em `http://localhost:4200`

---

## Funcionalidades Implementadas

- Cadastro, edição e exclusão de pacientes
- Validação dos dados conforme regras de negócio (CPF, e-mail, telefones)
- Pesquisa e listagem de pacientes
- Cadastro e listagem de convênios
- Mensagens amigáveis de erro para o usuário

---

## Observações e Requisitos

- O campo CPF **não é obrigatório**, mas se preenchido deve seguir o formato `000.000.000-00`
- Pelo menos um telefone (fixo ou celular) deve ser preenchido
- Para rodar a aplicação, é necessário o **Node.js 18+** e o **.NET 8 SDK**
- O backend não está dockerizado, mas pode ser facilmente adaptado

---

## Autor

Adrian Soares Fidelis  
[LinkedIn](https://www.linkedin.com/in/adrianfidelis/) — [GitHub](https://github.com/AdrianFidelis)

---

## Licença

Este projeto é de uso acadêmico/desafio técnico. Consulte o autor para reutilização comercial.

---
