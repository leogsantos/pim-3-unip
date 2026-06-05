// ══ DATA ══
const SERVICES={
  corte:[
    {name:'Corte Rápido',desc:'Corte tradicional masculino',price:'A consultar'},
    {name:'Corte com Lavagem',desc:'Corte + lavagem completa',price:'A consultar'}
  ],
  infantil:[
    {name:'Corte Rápido Infantil',desc:'Para crianças até 12 anos',price:'A consultar'},
    {name:'Corte com Risquinho',desc:'Corte especial com detalhes',price:'A consultar'}
  ],
  combos:[
    {name:'Corte + Barba',desc:'Combo completo',price:'R$ 70'},
    {name:'Corte + Sobrancelha',desc:'Combo masculino',price:'R$ 55'}
  ],
  servicos:[
    {name:'Barba',desc:'Aparar e modelar a barba',price:'R$ 20'},
    {name:'Sobrancelha',desc:'Design de sobrancelha',price:'R$ 15'},
    {name:'Pezinho',desc:'Acabamento no pescoço',price:'R$ 15'}
  ],
  quimicas:[
    {name:'Luzes',desc:'Mechas e luzes',price:'R$ 60'},
    {name:'Botox Capilar',desc:'Tratamento intensivo',price:'R$ 100'},
    {name:'Platinado',desc:'Descoloração completa',price:'R$ 120'}
  ]
};
let servicosDb = [];
const CAT_NAMES={corte:'Corte',infantil:'Corte Infantil',combos:'Combos',servicos:'Serviços',quimicas:'Químicas'};
let BARBERS = [];
const ALL_TIMES=['09:00','09:30','10:00','10:30','11:00','11:30','13:00','13:30','14:00','14:30','15:00','16:00','16:30','17:00','18:00','18:30','19:00'];
const MONTH_NAMES=['Janeiro','Fevereiro','Março','Abril','Maio','Junho','Julho','Agosto','Setembro','Outubro','Novembro','Dezembro'];

let state = {service: null, serviceId: null, date: null, barber: null, barberId: null, time: null, editId: null};
let appointments=[];
let calState={month:0,year:0,selectedDay:null,selectedBarber:null,selectedTime:null,viewMonth:0};

// 25502550 INIT 25502550
window.addEventListener('DOMContentLoaded', async function () {

  await carregarServicos();
  await carregarBarbeiros();
  await carregarDadosUsuario();

  updateStats();
  updateAppts();

  gotoApp('a-home');
});

// ══ APP NAVIGATION ══
const BC_LABELS={'a-home':'Início','a-services':'Serviços','a-calendar':'Agendar','a-confirm':'Confirmar','a-appts':'Agendamentos'};
function gotoApp(id){
  document.querySelectorAll('#app-layout .screen').forEach(s=>s.classList.remove('active'));
  document.getElementById(id).classList.add('active');
  // breadcrumb
  document.getElementById('bc-current').textContent=BC_LABELS[id]||id;
  // sidebar nav
  document.querySelectorAll('.nav-item').forEach(n=>n.classList.remove('active'));
  if(id==='a-home'||id==='a-services'||id==='a-calendar'||id==='a-confirm')
    document.getElementById('nav-home').classList.add('active');
  if(id==='a-appts')document.getElementById('nav-appts').classList.add('active');
}

// ══ SERVICES ══
function showServices(catKey){
  document.getElementById('srv-title-desk').textContent=CAT_NAMES[catKey];
  const list=document.getElementById('srv-list-desk');
  list.innerHTML='';
  SERVICES[catKey].forEach(srv=>{
    const el=document.createElement('div');
    el.className='service-item';
    el.innerHTML=`<div><div class="srv-name">${srv.name}</div><div class="srv-desc">${srv.desc}</div></div><div class="srv-price">${srv.price}</div>`;
    el.onclick=()=>selectService(srv);
    list.appendChild(el);
  });
}
//Busca os servicos no banco
function selectService(srv) {

  state.service = srv;

  const servicoBanco =
    servicosDb.find(
      x => x.nome === srv.name
    );

  if (servicoBanco)
    state.serviceId = servicoBanco.id;

  console.log("SERVICO ID:", state.serviceId);

  document.getElementById('cal-srv-label-desk')
    .textContent = srv.name;
  const now=new Date();
  calState.year=now.getFullYear();calState.month=now.getMonth();calState.viewMonth=now.getMonth();
  calState.selectedDay=null;calState.selectedBarber=null;calState.selectedTime=null;
  buildCalendar();
  gotoApp('a-calendar');
}

// ══ CALENDAR ══
function buildCalendar(){
  const now=new Date();
  const curMonth=now.getMonth(),curYear=now.getFullYear();
  const nextMonth=(curMonth+1)%12,nextYear=curMonth===11?curYear+1:curYear;
  const vm=calState.viewMonth;
  const vy=vm<curMonth?nextYear:curYear;

  let html=`<div class="month-tabs">
    <div class="month-tab${vm===curMonth?' active':''}" onclick="switchMonth(${curMonth},${curYear})">${MONTH_NAMES[curMonth]}</div>
    <div class="month-tab${vm===nextMonth?' active':''}" onclick="switchMonth(${nextMonth},${nextYear})">${MONTH_NAMES[nextMonth]}</div>
  </div>`;
  const firstDay=new Date(vy,vm,1).getDay();
  const daysInMonth=new Date(vy,vm+1,0).getDate();
  const todayD=now.getDate(),todayM=now.getMonth(),todayY=now.getFullYear();
  html+=`<div class="cal-month-name">${MONTH_NAMES[vm]} ${vy}</div>`;
  html+=`<div class="weekdays"><span>D</span><span>S</span><span>T</span><span>Q</span><span>Q</span><span>S</span><span>S</span></div>`;
  html+=`<div class="cal-grid">`;
  for(let i=0;i<firstDay;i++) html+=`<div class="cal-day empty"></div>`;
  for(let d=1;d<=daysInMonth;d++){
    const isToday=(d===todayD&&vm===todayM&&vy===todayY);
    const isPast=(vy<todayY)||(vy===todayY&&vm<todayM)||(vy===todayY&&vm===todayM&&d<todayD);
    const isSel=(d===calState.selectedDay&&vm===calState.viewMonth);
    const cls=['cal-day',isToday?'today':'',isPast?'past':'',isSel?'selected':''].filter(Boolean).join(' ');
    const click=isPast?'':`onclick="selectDay(${d},${vm},${vy})"`;
    html+=`<div class="${cls}" ${click}>${d}</div>`;
  }
  html+=`</div>`;
  document.getElementById('cal-wrap-desk').innerHTML=html;
  buildBarbers();
}

function buildBarbers(){
  const wrap=document.getElementById('barbers-wrap-desk');
  if(!calState.selectedDay){
    wrap.innerHTML=`<div class="cal-placeholder"><i class="ti ti-calendar-search"></i><p>Selecione uma data no calendário<br/>para ver os barbeiros disponíveis.</p></div>`;
    return;
  }
  const vm=calState.viewMonth;
  const vy=vm<new Date().getMonth()?(new Date().getFullYear()+1):new Date().getFullYear();
  const dd=String(calState.selectedDay).padStart(2,'0');
  const mm=String(vm+1).padStart(2,'0');
  const dateStr=`${dd}/${mm}/${vy}`;
  let html=`<div class="section-label" style="margin-bottom:16px">Barbeiros disponíveis — ${dateStr}</div>`;
  BARBERS.forEach((b,bi)=>{
    const slots=ALL_TIMES.filter((_,i)=>(i+bi)%3!==0);
    html+=`<div class="barber-card">
      <div class="barber-header">
        <div class="barber-avatar">${b.iniciais}</div>
        <div><div class="barber-name">${b.nome}</div><div class="barber-slots-count">${slots.length} horários disponíveis</div></div>
      </div>
      <div class="time-slots">`;
    slots.forEach(t=>{
      const isSel=(calState.selectedBarber===b.nome&&calState.selectedTime===t);
      html += `<div class="time-slot${isSel ? ' selected' : ''}" onclick="selectSlot(${b.id}, '${b.nome}', '${t}', '${dateStr}')">${t}</div>`;
    });
    html+=`</div></div>`;
  });
  wrap.innerHTML=html;
}

function switchMonth(m,y){
  calState.viewMonth=m;calState.selectedDay=null;calState.selectedBarber=null;calState.selectedTime=null;
  buildCalendar();
}
function selectDay(d,m,y){
  calState.selectedDay=d;calState.viewMonth=m;calState.selectedBarber=null;calState.selectedTime=null;
  buildCalendar();
}
function selectSlot(barberId, barberName, time, dateStr) {

  state.barberId = barberId;
  state.barber = barberName;

  calState.selectedBarber = barberName;
  calState.selectedTime = time;

  buildBarbers();

  state.date = dateStr;
  state.time = time;

  document.getElementById('cf-srv').textContent = state.service.name;
  document.getElementById('cf-barber').textContent = barberName;
  document.getElementById('cf-date').textContent = dateStr;
  document.getElementById('cf-time').textContent = time;
  document.getElementById('cf-price').textContent = state.service.price;

  gotoApp('a-confirm');
}

// ══ CONFIRM ══ | Agendamento enviado para o servidor através do método POST
async function finalizeBooking() {

  console.log("EDIT ID:", state.editId);
  console.log("STATE:", state);

  const name =
    document.getElementById('inp-name').value.trim();

  const phone =
    document.getElementById('inp-phone').value.trim();

  if (!name || !phone) {
    alert('Preencha nome e telefone.');
    return;
  }

  const partes = state.date.split('/');

  const dataHora =
    `${partes[2]}-${partes[1]}-${partes[0]}T${state.time}:00`;

  const payload = {

    AgendamentoId: state.editId || 0,

    BarbeiroId: state.barberId,

    ServicoId: state.serviceId,

    DataHora: dataHora,

    Status: "Confirmado",

    ValorTotal: null,

    NomeCliente: name,

    TelefoneCliente: phone,

    Observacoes: ""
  };

  const url = state.editId
    ? "/Agendamentoes/AtualizarAgendamento"
    : "/Agendamentoes/CriarAgendamento";

  console.log(
    "Payload enviado:",
    JSON.stringify(payload, null, 2)
  );

  const resp = await fetch(
    url,
    {
      method: "POST",

      headers: {
        "Content-Type": "application/json"
      },

      body: JSON.stringify(payload)
    }
  );

  const data = await resp.json();

  if (data.ok) {

    resetState();
    await updateStats();
    await updateAppts();

    showSuccess(name);

  } else {

    alert("Erro: " + data.erro);

  }
}

// Página de criacão com sucesso
function showSuccess(name) {
  const overlay=document.createElement('div');
  overlay.className='success-overlay';
  overlay.innerHTML=`
    <div class="success-icon"><i class="ti ti-check"></i></div>
    <div class="success-title">Agendado!</div>
    <div class="success-sub">Olá, <strong>${name}</strong>!<br/>Seu horário foi confirmado com sucesso.<br/>Até lá!</div>
    <button class="btn-ok" onclick="this.closest('.success-overlay').remove();updateAppts();gotoApp('a-appts')">VER AGENDAMENTOS</button>`;
  document.body.appendChild(overlay);
}

// ══ APPOINTMENTS ══ | Lê os agendamentos diretamente do banco de dados

async function updateAppts() {

  const resp = await fetch('/Agendamentoes/MeusAgendamentos');
  appointments = await resp.json();

  const list = document.getElementById('appts-list-desk');
  const count = document.getElementById('appts-count-desk');

  if (appointments.length === 0) {
    list.innerHTML = `<div class="empty-state">
      <i class="ti ti-calendar-off"></i>
      <p>Nenhum agendamento registrado.<br/>
      Faça sua reserva na aba Início.</p>
    </div>`;

    count.textContent = 'Nenhum agendamento';
    return;
  }

  // Constrói os agendamentos na página

  count.textContent = `${appointments.length} agendamento${appointments.length > 1 ? 's' : ''}`;

  list.innerHTML = appointments.map((a, idx) => `
<div class="appt-card">

    <div class="appt-top">
        <span class="appt-service">${a.service}</span>
        <span class="appt-badge">Confirmado</span>
    </div>

    <div class="appt-row">
        <i class="ti ti-user"></i>
        <span>Barbeiro: <span class="hl">${a.barber}</span></span>
    </div>

    <div class="appt-row">
        <i class="ti ti-calendar"></i>
        <span>${a.date} às <span class="hl">${a.time}</span></span>
    </div>

    <div class="appt-row">
    <i class="ti ti-tag"></i>
    <span>
        Valor:
        <span class="hl-blue">
            ${a.price == null ? 'A consultar' : 'R$ ' + a.price}
        </span>
    </span>
    </div>

    <div class="appt-row">
  <i class="ti ti-user-circle"></i>
  <span class="hl">${a.name}</span>
  <span>&nbsp;·&nbsp;${a.phone}</span>
</div>
    <div class="appt-actions">

        <button
            class="appt-btn appt-btn-edit"
            onclick="editAppt(${a.id})"
            <i class="ti ti-pencil"></i> ALTERAR
        </button>

        <button
            class="appt-btn appt-btn-cancel"
            onclick="deleteAppt(${a.id})">
            <i class="ti ti-trash"></i> CANCELAR
        </button>

    </div>

</div>
`).join('');
}

// Atualiza quantidade total de agendamentos

async function updateStats() {

  const resp = await fetch('/Agendamentoes/MeusAgendamentos');
  const appointments = await resp.json();

  document.getElementById('stat-total').textContent =
    appointments.length;
}


// Exclui agendamento

async function deleteAppt(id) {

  if (!confirm('Deseja cancelar este agendamento?'))
    return;

  await fetch(
    `/Agendamentoes/DeleteApi?id=${id}`,
    {
      method: 'POST'
    }
  );

  await updateAppts();
  await updateStats();
}

// Altera/Atualiza o agendamento
function editAppt(id) {

  state.editId = id;

  const a = appointments.find(x => x.id === id);

  if (!a) return;

  const catKey =
    Object.keys(SERVICES).find(k =>
      SERVICES[k].some(s => s.name === a.service)
    ) || 'corte';

  showServices(catKey);

  gotoApp('a-services');
}




// Redirecionamento para página de logout
function doLogout() {
  window.location.href = "/Account/Logout";
}

// Reseta os valores para null
function resetState() {

  state = {
    service: null,
    serviceId: null,
    date: null,
    barber: null,
    barberId: null,
    time: null,
    editId: null
  };

}

// carrega os dados do usuario no formulario de agendamento
async function carregarDadosUsuario() { 

  const resp = await fetch('/Account/UsuarioLogado');

  if (!resp.ok)
    return;

  const usuario = await resp.json();

  document.getElementById('inp-name').value =
    usuario.nome;

  document.getElementById('inp-phone').value =
    usuario.telefone;
}
//lista barbeiros em json
async function carregarBarbeiros() {

  const resp =
    await fetch('/Agendamentoes/ListarBarbeiros');

  BARBERS =
    await resp.json();

  console.log("BARBEIROS:", BARBERS);
}
//lista servicos em json
async function carregarServicos() {

  const resp =
    await fetch('/Agendamentoes/ListarServicos');

  servicosDb =
    await resp.json();

  console.log("SERVICOS:", servicosDb);
}
