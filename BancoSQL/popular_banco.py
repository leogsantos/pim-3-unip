"""
INSTALAÇÃO:
    Abra o CMD e instale as bibliotecas necessárias:
    pip install pyodbc faker

DRIVER ODBC:
    Baixe e instale o ODBC Driver 17 for SQL Server:
    https://go.microsoft.com/fwlink/?linkid=2249004

O QUE ESTE SCRIPT FAZ:
    - Limpar dados antigos (exceto seed data)
    - Criar 10 barbeiros (7 ativos, 3 inativos)
    - Criar usuários clientes
    - Criar agendamentos com status variados baseados na data
    
OBSERVAÇÕES:
    - Dados de teste apenas
    - Seed data (categorias e serviços) são mantidos
    - Administrador padrão é mantido
"""

import pyodbc
import random
from datetime import datetime, timedelta
from faker import Faker
from functools import wraps

# ==================================================================================
# CONFIGURAÇÕES
# ==================================================================================

QTDE_USUARIOS = 10
QTDE_BARBEIROS_EXTRAS = 7  # Total será 10 (3 já existem no seed)
QTDE_AGENDAMENTOS = 200

# Configuração do Faker
fake = Faker('pt_BR')
Faker.seed(42)
random.seed(42)

# ==================================================================================
# DECORATORS E FUNÇÕES AUXILIARES
# ==================================================================================

def handle_db_errors(func):
    """Decorator para tratamento de erros de banco de dados"""
    @wraps(func)
    def wrapper(*args, **kwargs):
        try:
            return func(*args, **kwargs)
        except pyodbc.Error as e:
            print(f"ERRO DE BANCO DE DADOS em {func.__name__}: {e}")
            raise
        except Exception as e:
            print(f"ERRO em {func.__name__}: {e}")
            raise
    return wrapper

def conectar_banco():
    """Estabelece conexão com o banco de dados"""
    try:
        conn = pyodbc.connect(
            'DRIVER={ODBC Driver 17 for SQL Server};'  # Precisa ter o driver instalado -> https://go.microsoft.com/fwlink/?linkid=2249004
            'SERVER=(localdb)\\MSSQLLocalDB;'
            'DATABASE=BarbeariaJLMGG;'  # Nome do nosso banco de dados
            'Trusted_Connection=yes;'
        )
        print("Conectado ao banco de dados com sucesso")
        return conn
    except pyodbc.Error as e:
        print(f"ERRO ao conectar ao banco de dados: {e}")
        print("\nVerifique se:")
        print("  1. SQL Server está rodando")
        print("  2. Banco 'BarbeariaJLMGG' existe")
        print("  3. ODBC Driver 17 está instalado")
        raise

# ==================================================================================
# FUNÇÕES DE LIMPEZA
# ==================================================================================

@handle_db_errors
def limpar_dados(cursor):
    """Remove dados antigos mantendo seed data"""
    print("\nLimpando dados antigos...")
    
    cursor.execute("DELETE FROM Agendamento")
    cursor.execute("DELETE FROM Usuario WHERE UsuarioId > 1")
    cursor.execute("DELETE FROM Barbeiro WHERE BarbeiroId > 3")
    
    print("Dados antigos removidos")

# ==================================================================================
# FUNÇÕES DE CRIAÇÃO DE DADOS
# ==================================================================================

@handle_db_errors
def criar_barbeiros(cursor, quantidade):
    """Cria barbeiros extras (7 ativos, 3 inativos no total de extras)"""
    print(f"\nCriando {quantidade} barbeiros extras...")
    
    barbeiros_dados = [
        ('Ricardo Santos', 'RS', '(12) 98111-1111', 'ricardo@jlmgg.com', 1),
        ('Felipe Oliveira', 'FO', '(12) 98222-2222', 'felipe@jlmgg.com', 1),
        ('Lucas Almeida', 'LA', '(12) 98333-3333', 'lucas@jlmgg.com', 1),
        ('Pedro Costa', 'PC', '(12) 98444-4444', 'pedro@jlmgg.com', 1),
        ('Thiago Ferreira', 'TF', '(12) 98555-5555', 'thiago@jlmgg.com', 0),
        ('Rafael Lima', 'RL', '(12) 98666-6666', 'rafael@jlmgg.com', 0),
        ('Bruno Silva', 'BS', '(12) 98777-7777', 'bruno@jlmgg.com', 0),
    ]
    
    for nome, iniciais, telefone, email, ativo in barbeiros_dados[:quantidade]:
        cursor.execute("""
            INSERT INTO Barbeiro (Nome, Iniciais, Telefone, Email, Ativo)
            VALUES (?, ?, ?, ?, ?)
        """, (nome, iniciais, telefone, email, ativo))
    
    ativos = sum(1 for b in barbeiros_dados[:quantidade] if b[4] == 1)
    inativos = quantidade - ativos
    
    print(f"Barbeiros criados: {quantidade} ({ativos} ativos, {inativos} inativos)")

@handle_db_errors
def criar_usuarios(cursor, quantidade):
    """Cria usuários clientes com dados brasileiros"""
    print(f"\nCriando {quantidade} usuarios...")
    
    usuarios_criados = []
    
    for i in range(quantidade):
        nome = fake.name()
        email = fake.email()
        telefone = fake.phone_number()
        senha = 'senha123hash'
        tipo = 'Cliente'
        data_cadastro = fake.date_time_between(start_date='-2y', end_date='now')
        
        cursor.execute("""
            INSERT INTO Usuario (Nome, Email, Telefone, Senha, TipoUsuario, DataCadastro)
            OUTPUT INSERTED.UsuarioId
            VALUES (?, ?, ?, ?, ?, ?)
        """, (nome, email, telefone, senha, tipo, data_cadastro))
        
        usuario_id = cursor.fetchone()[0]
        usuarios_criados.append(usuario_id)
        
        if (i + 1) % 20 == 0:
            print(f"  {i + 1} usuarios criados...")
    
    print(f"Total de usuarios criados: {len(usuarios_criados)}")
    return usuarios_criados

@handle_db_errors
def buscar_servicos_ativos(cursor):
    """Retorna lista de serviços ativos"""
    cursor.execute("SELECT ServicoId, Preco FROM Servico WHERE Ativo = 1")
    return cursor.fetchall()

@handle_db_errors
def buscar_barbeiros_ativos(cursor):
    """Retorna lista de IDs de barbeiros ativos"""
    cursor.execute("SELECT BarbeiroId FROM Barbeiro WHERE Ativo = 1")
    return [row[0] for row in cursor.fetchall()]

def gerar_status_agendamento(data_hora):
    """Determina status do agendamento baseado na data"""
    agora = datetime.now()
    
    if data_hora < agora - timedelta(days=1):
        # Agendamentos passados: 90% concluídos, 10% falta
        return random.choices(['Concluído', 'Falta'], weights=[0.9, 0.1])[0]
    elif data_hora > agora:
        # Agendamentos futuros: todos confirmados
        return 'Confirmado'
    else:
        # Hoje: distribuição variada
        return random.choices(
            ['Confirmado', 'Concluído', 'Cancelado', 'Falta'],
            weights=[0.4, 0.45, 0.10, 0.05]
        )[0]

@handle_db_errors
def criar_agendamentos(cursor, usuarios_ids, servicos, barbeiros_ids, quantidade):
    """Cria agendamentos distribuídos no tempo"""
    print(f"\nCriando {quantidade} agendamentos...")
    
    data_inicio = datetime.now() - timedelta(days=180)
    data_fim = datetime.now() + timedelta(days=30)
    
    for i in range(quantidade):
        usuario_id = random.choice(usuarios_ids)
        barbeiro_id = random.choice(barbeiros_ids)
        servico = random.choice(servicos)
        servico_id, preco = servico
        
        # Gerar data aleatória
        delta = data_fim - data_inicio
        random_days = random.randint(0, delta.days)
        data_hora = data_inicio + timedelta(days=random_days)
        
        # Ajustar para horário comercial (9h às 18h)
        data_hora = data_hora.replace(
            hour=random.randint(9, 17),
            minute=random.choice([0, 30]),
            second=0,
            microsecond=0
        )
        
        status = gerar_status_agendamento(data_hora)
        nome_cliente = fake.name()
        telefone_cliente = fake.phone_number()
        
        observacoes = random.choice([
            None,
            'Cliente fidelizado',
            'Primeira vez',
            'Horário especial',
            'Cliente VIP'
        ])
        
        data_criacao = data_hora - timedelta(days=random.randint(1, 30))
        
        cursor.execute("""
            INSERT INTO Agendamento 
            (UsuarioId, BarbeiroId, ServicoId, DataHora, Status, ValorTotal, 
             NomeCliente, TelefoneCliente, Observacoes, DataCriacao)
            VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
        """, (usuario_id, barbeiro_id, servico_id, data_hora, status, preco,
              nome_cliente, telefone_cliente, observacoes, data_criacao))
        
        if (i + 1) % 100 == 0:
            print(f"  {i + 1} agendamentos criados...")
    
    print(f"Total de agendamentos criados: {quantidade}")

# ==================================================================================
# FUNÇÃO PRINCIPAL
# ==================================================================================

def main():
    """Função principal de execução"""
    print("=" * 70)
    print("POPULADOR DE BANCO DE DADOS - BARBEARIA JLMGG")
    print("=" * 70)
    
    conn = None
    cursor = None
    
    try:
        # Conectar ao banco
        conn = conectar_banco()
        cursor = conn.cursor()
        
        # Limpar dados antigos
        limpar_dados(cursor)
        conn.commit()
        
        # Criar barbeiros
        criar_barbeiros(cursor, QTDE_BARBEIROS_EXTRAS)
        conn.commit()
        
        # Criar usuários
        usuarios_ids = criar_usuarios(cursor, QTDE_USUARIOS)
        conn.commit()
        
        # Buscar dados necessários
        servicos = buscar_servicos_ativos(cursor)
        barbeiros_ids = buscar_barbeiros_ativos(cursor)
        
        # Criar agendamentos
        criar_agendamentos(cursor, usuarios_ids, servicos, barbeiros_ids, QTDE_AGENDAMENTOS)
        conn.commit()
        
        print("\n" + "=" * 70)
        print("PROCESSO CONCLUIDO COM SUCESSO")
        print("=" * 70)
        
    except Exception as e:
        print(f"\nERRO DURANTE A EXECUCAO: {e}")
        if conn:
            conn.rollback()
            print("Transacao revertida")
        raise
        
    finally:
        if cursor:
            cursor.close()
        if conn:
            conn.close()
            print("\nConexao fechada")

if __name__ == "__main__":
    main()