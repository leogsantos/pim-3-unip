-- Rode apenas se você não criou o banco manualmente
CREATE DATABASE BarbeariaJLMGG;
GO

USE BarbeariaJLMGG;
GO

-- Se esta abrindo o banco de dados manualmente, copie e cole essa query daqui pra baixo
-- USUÁRIOS (clientes e administradores)
CREATE TABLE Usuario (
    UsuarioId INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    Telefone NVARCHAR(20) NOT NULL,
    Senha NVARCHAR(255) NOT NULL,
    TipoUsuario NVARCHAR(20) NOT NULL DEFAULT 'Cliente',
    DataCadastro DATETIME NOT NULL DEFAULT GETDATE(),
    Ativo BIT NOT NULL DEFAULT 1
);

-- BARBEIROS
CREATE TABLE Barbeiro (
    BarbeiroId INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    Iniciais NVARCHAR(5) NOT NULL,
    Telefone NVARCHAR(20),
    Email NVARCHAR(150),
    Ativo BIT NOT NULL DEFAULT 1
);

-- CATEGORIAS DE SERVIÇOS
CREATE TABLE CategoriaServico (
    CategoriaServicoId INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(50) NOT NULL UNIQUE,
    Icone NVARCHAR(50) NULL,
    Ativo BIT NOT NULL DEFAULT 1
);

-- SERVIÇOS
CREATE TABLE Servico (
    ServicoId INT IDENTITY(1,1) PRIMARY KEY,
    CategoriaServicoId INT NOT NULL,
    Nome NVARCHAR(100) NOT NULL,
    Preco DECIMAL(10,2) NULL, -- NULL = "A consultar"
    DuracaoMinutos INT NOT NULL DEFAULT 30,
    Ativo BIT NOT NULL DEFAULT 1,
    
    FOREIGN KEY (CategoriaServicoId) REFERENCES CategoriaServico(CategoriaServicoId)
);

-- AGENDAMENTOS
CREATE TABLE Agendamento (
    AgendamentoId INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId INT NOT NULL,
    BarbeiroId INT NOT NULL,
    ServicoId INT NOT NULL,
    DataHora DATETIME NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Confirmado',
    ValorTotal DECIMAL(10,2) NULL, -- Pode ser NULL se preço "a consultar"
    NomeCliente NVARCHAR(100) NOT NULL, -- Nome informado no agendamento
    TelefoneCliente NVARCHAR(20) NOT NULL, -- Telefone informado
    Observacoes NVARCHAR(500),
    DataCriacao DATETIME NOT NULL DEFAULT GETDATE(),
    
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(UsuarioId),
    FOREIGN KEY (BarbeiroId) REFERENCES Barbeiro(BarbeiroId),
    FOREIGN KEY (ServicoId) REFERENCES Servico(ServicoId)
);

GO


-- Categorias
INSERT INTO CategoriaServico (Nome, Icone) VALUES
('Corte', 'ti-scissors'),
('Corte Infantil', 'ti-baby-carriage'),
('Combos', 'ti-package'),
('Serviços', 'ti-star'),
('Químicas', 'ti-flask');

-- Inserir categorias e capturar IDs
DECLARE @CatCorte INT;
DECLARE @CatInfantil INT;
DECLARE @CatCombos INT;
DECLARE @CatServicos INT;
DECLARE @CatQuimicas INT;

-- CORTE
INSERT INTO CategoriaServico (Nome, Icone) VALUES ('Corte', 'ti-scissors');
SET @CatCorte = SCOPE_IDENTITY();

-- CORTE INFANTIL
INSERT INTO CategoriaServico (Nome, Icone) VALUES ('Corte Infantil', 'ti-baby-carriage');
SET @CatInfantil = SCOPE_IDENTITY();

-- COMBOS
INSERT INTO CategoriaServico (Nome, Icone) VALUES ('Combos', 'ti-package');
SET @CatCombos = SCOPE_IDENTITY();

-- SERVIÇOS
INSERT INTO CategoriaServico (Nome, Icone) VALUES ('Serviços', 'ti-star');
SET @CatServicos = SCOPE_IDENTITY();

-- QUÍMICAS
INSERT INTO CategoriaServico (Nome, Icone) VALUES ('Químicas', 'ti-flask');
SET @CatQuimicas = SCOPE_IDENTITY();

-- CORTE (preços "a consultar" = NULL)
INSERT INTO Servico (CategoriaServicoId, Nome, Preco, DuracaoMinutos) VALUES
(@CatCorte, 'Corte Rápido', NULL, 30),
(@CatCorte, 'Corte com Lavagem', NULL, 45);

-- CORTE INFANTIL (preços "a consultar" = NULL)
INSERT INTO Servico (CategoriaServicoId, Nome, Preco, DuracaoMinutos) VALUES
(@CatInfantil, 'Corte Rápido Infantil', NULL, 25),
(@CatInfantil, 'Corte com Risquinho', NULL, 35);

-- COMBOS (com preços)
INSERT INTO Servico (CategoriaServicoId, Nome, Preco, DuracaoMinutos) VALUES
(@CatCombos, 'Corte + Barba', 70.00, 60),
(@CatCombos, 'Corte + Sobrancelha', 55.00, 50);

-- SERVIÇOS (com preços)
INSERT INTO Servico (CategoriaServicoId, Nome, Preco, DuracaoMinutos) VALUES
(@CatServicos, 'Barba', 20.00, 20),
(@CatServicos, 'Sobrancelha', 15.00, 15),
(@CatServicos, 'Pezinho', 15.00, 15);

-- QUÍMICAS (com preços)
INSERT INTO Servico (CategoriaServicoId, Nome, Preco, DuracaoMinutos) VALUES
(@CatQuimicas, 'Luzes', 60.00, 90),
(@CatQuimicas, 'Botox Capilar', 100.00, 120),
(@CatQuimicas, 'Platinado', 120.00, 150);

GO