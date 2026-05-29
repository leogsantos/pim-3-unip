// Guarda o e-mail confirmado entre as etapas
let emailConfirmado = "";

function forgotShowStep(n) {
  for (let i = 0; i < 4; i++) {
    const p = document.getElementById("forgot-step-" + i);
    if (p) p.classList.toggle("active", i === n);
  }
}

// Etapa 0 → chama o servidor para checar se o e-mail existe
async function sendCode() {
  const email = document.getElementById("forgot-email").value.trim();
  const erro  = document.getElementById("forgot-email-erro");

  erro.style.display = "none";

  if (!email) {
    erro.textContent   = "Informe seu e-mail cadastrado.";
    erro.style.display = "inline";
    return;
  }

  const resp = await fetch("/Account/VerificarEmail", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email })
  });
  const data = await resp.json();

  if (!data.ok) {
    erro.style.display = "inline";
    return;
  }

  emailConfirmado = email;
  forgotShowStep(1);
}

// Etapa 1 → valida o código fixo 123456 (simula verificação por e-mail)
function verifyCode() {
  const code = document.getElementById("forgot-code").value.trim();
  const CODIGO_FIXO = "123456";

  if (code !== CODIGO_FIXO) {
    alert("Código incorreto. Use: 123456");
    return;
  }

  forgotShowStep(2);
}

// Etapa 2 → envia a nova senha para o servidor salvar no banco
async function saveNewPass() {
  const p1 = document.getElementById("new-pass").value;
  const p2 = document.getElementById("new-pass2").value;
  if (p1.length < 8)  { alert("A senha deve ter pelo menos 8 caracteres."); return; }
  if (p1 !== p2)      { alert("As senhas não coincidem."); return; }

  const resp = await fetch("/Account/RedefinirSenha", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email: emailConfirmado, novaSenha: p1 })
  });
  const data = await resp.json();

  if (!data.ok) {
    alert("Erro ao salvar: " + data.erro);
    return;
  }

  forgotShowStep(3);
}

function togglePass(id, icon) {
  const inp = document.getElementById(id);
  if (inp.type === "password") {
    inp.type = "text";
    icon.classList.replace("ti-eye", "ti-eye-off");
  } else {
    inp.type = "password";
    icon.classList.replace("ti-eye-off", "ti-eye");
  }
}

const STRENGTH_LEVELS = [
  { w: "20%", bg: "#e53935", label: "Muito fraca" },
  { w: "40%", bg: "#ef6c00", label: "Fraca" },
  { w: "60%", bg: "#fbc02d", label: "Razoável" },
  { w: "80%", bg: "#7cb342", label: "Boa" },
  { w: "100%", bg: "#2e7d32", label: "Forte" },
];
function _strengthScore(v) {
  let s = 0;
  if (v.length >= 8)          s++;
  if (/[A-Z]/.test(v))        s++;
  if (/[0-9]/.test(v))        s++;
  if (/[^A-Za-z0-9]/.test(v)) s++;
  if (v.length >= 12)          s++;
  return Math.min(s, 4);
}
function checkStrength(v) {
  const idx = v.length ? _strengthScore(v) : 0;
  const l   = v.length ? STRENGTH_LEVELS[idx] : null;
  document.getElementById("strength-fill2").style.width      = l ? l.w  : "0%";
  document.getElementById("strength-fill2").style.background = l ? l.bg : "";
  document.getElementById("strength-label2").textContent     = l ? l.label : "Digite uma senha";
}
