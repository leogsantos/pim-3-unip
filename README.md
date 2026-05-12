# pim-3-unip

### Stack Tecnologica - SQL Server (Express)

# O que esse projeto faz?
- Cadastro de clientes - Tela de login e cadastro
- Agendamento de cortes de cabelo, tratamentos quimicos, etc
- Visualização dos proprios agendamentos

## Separação de pastas e responsabilidades
UI e UX: João -> criação de telas, design, etc.
- [ ] Tela de login
- [ ] Tela de cadastro
- [ ] Tela de erro de login
- [ ] Tela de esqueci minha senha
- [ ] Readequar para visualização WEB
- [ ] Possibilidade de Cancelar e excluir agendamentos
- [ ] Adequar Libras em algum local do site (acessibilidade) -> https://vlibras.gov.br/doc/widget/installation/webpageintegration.html?_ga=2.205222480.1595640842.1682445746-816840059.1655413110

Frontend: Guilherme (pasta *wwwroot* e *Views*)
- [ ] Construção das páginas cshtml de acordo com o que vier do UI/UX
- [ ] Home page
- [ ] Modularização de arquivos (js, css, etc)
- [ ] Responsividade - focar apenas no desenvolvimento pra tela de PC

Backend: Marcos
- [ ] CRUD de agendamentos
- [ ] CRUD cadastro e logins (recuperar senha e etc)
- [ ] Utilizar Razor

Banco de dados: Leo, Gabriel
- [ ] Criar banco de dados
- [ ] Documentar banco
- [ ] Criar pasta "Data" para dar acesso as classes
