# Contexto do Projeto - PIM III Barbearia JLMGG

## Informações do Projeto

**Projeto:** Sistema E-commerce para Barbearia
**Curso:** Análise e Desenvolvimento de Sistemas - UNIP
**Disciplina:** PIM III
**Grupo:** JLMGG

## Objetivo

Desenvolver um sistema web integrado para uma barbearia fictícia que oferece:
- Agendamento de serviços online
- E-commerce de produtos
- Gestão administrativa
- Dashboard com análise de dados

## Tecnologias

### Backend
- **Framework:** ASP.NET Core MVC (.NET 10.0)
- **Linguagem:** C#
- **ORM:** Entity Framework Core
- **Arquitetura:** MVC + Repository Pattern

### Frontend
- **Tecnologias:** HTML5, CSS3, JavaScript
- **Framework CSS:** Bootstrap 5
- **Responsividade:** Mobile-first
- **Acessibilidade:** WCAG 2.1 + LIBRAS

### Banco de Dados
- **SGBD:** SQL Server
- **Tipo:** Relacional (com possível uso de NoSQL para logs/analytics)
- **Abordagem:** Code-First com Entity Framework

## Estrutura do Negócio

### Nome Fantasia
Barbearia JLMGG

### Segmento
Barbearia premium com serviços presenciais e venda de produtos online

### Serviços Oferecidos
1. **Cortes**
   - Corte Degradê (R$ 45)
   - Corte Social (R$ 40)
   - Corte Infantil (R$ 30)

2. **Barba**
   - Barba Completa (R$ 35)
   - Aparar Barba (R$ 25)
   - Design de Barba (R$ 40)

3. **Combos**
   - Corte + Barba (R$ 70)
   - Pacote Premium (R$ 120)

4. **Outros**
   - Sobrancelha (R$ 20)
   - Pigmentação (R$ 50)

### Público-Alvo
- Homens de 18 a 60 anos
- Classes B e C
- Perfil urbano que valoriza cuidados pessoais
- Busca por conveniência e qualidade

## Funcionalidades do Sistema

### Área Pública
- [x] Página inicial
- [x] Catálogo de serviços
- [x] Sistema de agendamento (calendário interativo)
- [x] Seleção de barbeiro
- [x] Cadastro e login de clientes
- [ ] Catálogo de produtos para venda
- [ ] Carrinho de compras
- [ ] Checkout e pagamento

### Área Administrativa
- [ ] Dashboard com métricas
- [ ] Gestão de agendamentos
- [ ] Gestão de clientes
- [ ] Gestão de barbeiros
- [ ] Gestão de serviços
- [ ] Gestão de produtos
- [ ] Controle de estoque
- [ ] Relatórios de vendas
- [ ] Análise de dados

### Área do Cliente
- [ ] Visualizar agendamentos
- [ ] Editar agendamentos
- [ ] Cancelar agendamentos
- [ ] Histórico de serviços
- [ ] Produtos comprados
- [ ] Perfil e configurações

## Requisitos Não-Funcionais

### Usabilidade
- Interface intuitiva e moderna
- Navegação simples com até 3 cliques para ações principais
- Design responsivo (mobile, tablet, desktop)
- Tema escuro premium (preto, azul, dourado)

### Performance
- Tempo de resposta < 2s para consultas
- Carregamento de página < 3s
- Suporte a 100 usuários simultâneos (simulação)

### Segurança
- Autenticação e autorização (ASP.NET Identity)
- Senhas hasheadas (bcrypt/PBKDF2)
- Proteção contra SQL Injection
- HTTPS obrigatório
- Validação de entrada

### Acessibilidade
- Contraste adequado (WCAG 2.1 AA)
- Suporte a leitores de tela
- Navegação por teclado
- Recurso de LIBRAS (vídeo interpretado)

## Disciplinas Integradas

### 1. Engenharia de Software Ágil
- Backlog de produtos
- User Stories com critérios de aceite
- Sprints planejadas
- Metodologia Scrum/Kanban

### 2. Modelagem de Banco de Dados
- Modelo Entidade-Relacionamento (ER)
- Modelo Lógico Relacional
- Normalização (3FN)
- Scripts SQL DDL/DML

### 3. Programação Orientada a Objetos (C#)
- Classes, herança, polimorfismo
- Encapsulamento
- SOLID principles
- Design Patterns (Repository, Service)

### 4. Desenvolvimento Web Responsivo
- Layout adaptável
- Grid system (Bootstrap)
- Media queries
- Progressive enhancement

### 5. UX/UI Design
- Personas
- Wireframes
- Protótipos navegáveis
- Testes de usabilidade

### 6. Machine Learning e Análise de Dados
- Relatórios de vendas
- Serviços mais populares
- Horários de pico
- Previsão de demanda (simples)
- Indicadores de performance (KPIs)

### 7. Comunicação e Liderança
- Documentação técnica clara
- Comunicação entre camadas
- Versionamento (Git)

### 8. LIBRAS
- Vídeos com intérprete
- Glossário de termos
- Acessibilidade inclusiva

## Estrutura do Trabalho Acadêmico

### Elementos Pré-textuais
- [ ] Capa
- [ ] Folha de rosto
- [ ] Resumo (PT)
- [ ] Abstract (EN)
- [ ] Sumário
- [ ] Lista de figuras
- [ ] Lista de tabelas

### Elementos Textuais (20-30 páginas)
1. Introdução
2. Caracterização do Negócio
3. Engenharia de Software Ágil
4. Modelagem de Banco de Dados
5. Programação Orientada a Objetos
6. Desenvolvimento Web
7. UX/UI Design
8. Análise de Dados
9. Comunicação e Acessibilidade
10. Integração e Avaliação
11. Conclusão

### Elementos Pós-textuais
- [ ] Referências (ABNT)
- [ ] Apêndices (código, prints)
- [ ] Anexos (documentos externos)

## Cronograma Acadêmico

### Fase 1 - Planejamento (Semana 1-2)
- Definição do negócio
- Levantamento de requisitos
- Backlog e user stories
- Modelagem de dados

### Fase 2 - Desenvolvimento Backend (Semana 3-5)
- Configuração do projeto ASP.NET
- Criação das entidades
- Implementação do repositório
- Desenvolvimento dos controllers

### Fase 3 - Desenvolvimento Frontend (Semana 6-7)
- Implementação das views
- Responsividade
- Integração com backend

### Fase 4 - Análise de Dados (Semana 8)
- Implementação de relatórios
- Dashboard administrativo
- Indicadores

### Fase 5 - Testes e Ajustes (Semana 9)
- Testes funcionais
- Testes de usabilidade
- Correções

### Fase 6 - Documentação (Semana 10)
- Redação do trabalho
- Diagramas e prints
- Revisão final

## Padrões e Convenções

### Nomenclatura C#
- PascalCase para classes, métodos, propriedades
- camelCase para variáveis locais
- Prefixo "I" para interfaces
- Sufixo "Repository", "Service", "Controller"

### Nomenclatura Banco de Dados
- PascalCase para tabelas (ex: `Usuario`, `Agendamento`)
- PascalCase para colunas (ex: `UsuarioId`, `DataCriacao`)
- Prefixo "PK_" para chaves primárias
- Prefixo "FK_" para chaves estrangeiras
- Prefixo "IX_" para índices

### Git
- Commits em português
- Branches: `feature/nome`, `bugfix/nome`
- Mensagens descritivas

## Observações Importantes

### Avaliação
- Trabalho pode ser individual ou em grupo (até 6 pessoas)
- Entrega via plataforma UNIP + cópia impressa
- Uso de software anti-plágio
- Citações conforme ABNT

### Critérios de Qualidade
- Código limpo e organizado
- Separação de responsabilidades
- Comentários quando necessário
- Aderência aos padrões ABNT
- Originalidade (não plagiar)

## Referências Iniciais

- Microsoft Docs - ASP.NET Core MVC
- Entity Framework Core Documentation
- Bootstrap 5 Documentation
- ABNT NBR 14724:2011 (Trabalhos Acadêmicos)
- Sommerville, I. - Engenharia de Software
- Pressman, R. - Engenharia de Software
- Martin, R. - Clean Code
