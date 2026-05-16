# Documentação - Banco de Dados Barbearia JLMGG

## Requisitos

### Software necessário
- SQL Server (qualquer versão compatível)
- Python 3.x
- ODBC Driver 17 for SQL Server

### Bibliotecas Python
```bash
pip install pyodbc faker
```

## Instalação do ODBC Driver

Baixe e instale o driver: https://go.microsoft.com/fwlink/?linkid=2249004

## Criação do Banco de Dados

### Opção 1: Restaurar do arquivo .bak (RECOMENDADO)

#### Via Interface Gráfica (SSMS):

1. Abra o SQL Server Management Studio (SSMS)
2. Conecte-se ao servidor
3. Clique com botão direito em "Databases" > "Restore Database..."
4. Selecione "Device" e clique em "..."
5. Clique em "Add" e localize o arquivo `BarbeariaJLMGG.bak`
6. Clique em "OK" e depois em "OK" novamente para iniciar a restauração
7. Aguarde a conclusão do processo

#### Via Comando SQL:

Abra uma nova query no SSMS e execute:

```sql
RESTORE DATABASE BarbeariaJLMGG
FROM DISK = 'C:\caminho\para\BarbeariaJLMGG.bak'
WITH REPLACE,
MOVE 'BarbeariaJLMGG' TO 'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\BarbeariaJLMGG.mdf',
MOVE 'BarbeariaJLMGG_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\BarbeariaJLMGG_log.ldf';
```

**Importante:** Ajuste os caminhos conforme sua instalação do SQL Server.

### Opção 2: Executar script SQL

1. Abra o SQL Server Management Studio
2. Abra o arquivo `CreateDB.sql`
3. Execute o script completo (F5 ou botão Execute)

**Alternativa - criar banco manualmente:**
```sql
CREATE DATABASE BarbeariaJLMGG;
GO
```
Depois execute apenas a partir de `USE BarbeariaJLMGG;` no arquivo `CreateDB.sql`.

## Estrutura do Banco

O banco contém as seguintes tabelas:
- Usuario (clientes e administradores)
- Barbeiro
- CategoriaServico
- Servico
- Agendamento

Dados iniciais (seed data):
- 5 categorias de serviços
- Declaração de variáveis para categorias

## Popular o Banco com Dados de Teste

### Configuração

Verifique a string de conexão no arquivo `popular_banco.py`:
```python
conn = pyodbc.connect(
    'DRIVER={ODBC Driver 17 for SQL Server};'
    'SERVER=localhost;'
    'DATABASE=BarbeariaJLMGG;'
    'Trusted_Connection=yes;'
)
```

Ajuste `SERVER=localhost` se seu SQL Server estiver em outra máquina.

### Quantidades configuráveis

No início do script você pode alterar:
```python
QTDE_USUARIOS = 100
QTDE_BARBEIROS_EXTRAS = 7
QTDE_AGENDAMENTOS = 500
```

### Executar o script

```bash
python popular_banco.py
```

### O que o script faz

1. Limpa dados antigos (mantém seed data)
2. Cria 7 barbeiros extras (4 ativos, 3 inativos)
3. Cria 100 usuários clientes com dados brasileiros
4. Cria 500 agendamentos distribuídos nos últimos 6 meses e próximos 30 dias
5. Status dos agendamentos são definidos automaticamente baseado na data

### Dados mantidos

O script preserva:
- Categorias de serviço
- Serviços cadastrados
- Usuário administrador (UsuarioId = 1)
- Primeiros 3 barbeiros (BarbeiroId 1-3)

## Verificação

Após executar, verifique no SQL Server:
```sql
SELECT COUNT(*) FROM Usuario;
SELECT COUNT(*) FROM Barbeiro;
SELECT COUNT(*) FROM Agendamento;
```

## Troubleshooting

### Erro de conexão
- Verifique se o SQL Server está rodando
- Confirme que o banco BarbeariaJLMGG existe
- Verifique se o ODBC Driver 17+ está instalado

### Erro de permissão
- Execute o script com usuário que tenha permissões no banco

### Erro de encoding
- O script usa `Faker('pt_BR')` para gerar dados brasileiros
- Certifique-se que o Python está configurado para UTF-8

### Erro ao restaurar .bak
- Verifique se o caminho do arquivo está correto
- Confirme que você tem permissões de administrador no SQL Server
- Se o banco já existir, use a opção REPLACE ou delete o banco antes