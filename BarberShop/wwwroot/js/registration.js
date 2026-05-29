let regCurrentStep = 0;

function regShowStep(n) {
  for (let i = 0; i < 3; i++) {
    document.getElementById("reg-step-" + i).classList.toggle("active", i === n);
    const d = document.getElementById("rdot-" + i);
    d.classList.remove("active", "done");
    if (i === n) d.classList.add("active");
    else if (i < n) d.classList.add("done");
  }
  regCurrentStep = n;
}

function regNext(from) {
  if (from === 0) {
    const name  = document.getElementById("reg-name").value.trim();
    const phone = document.getElementById("reg-phone").value.trim();
    if (!name || !phone) { alert("Preencha nome e telefone para continuar."); return; }
    regShowStep(1);
  } else if (from === 1) {
    const email = document.getElementById("reg-email").value.trim();
    const pass  = document.getElementById("reg-pass").value;
    const pass2 = document.getElementById("reg-pass2").value;
    if (!email)          { alert("Informe o e-mail."); return; }
    if (pass.length < 8) { alert("A senha deve ter pelo menos 8 caracteres."); return; }
    if (pass !== pass2)  { alert("As senhas não coincidem."); return; }

    document.getElementById("sum-name").textContent  = document.getElementById("reg-name").value.trim();
    document.getElementById("sum-phone").textContent = document.getElementById("reg-phone").value.trim();
    document.getElementById("sum-email").textContent = email;
    regShowStep(2);
  }
}

function regBack(from) { regShowStep(from - 1); }

// Envia os dados para o servidor e salva no banco
async function finishRegister() {
  const payload = {
    nome:     document.getElementById("reg-name").value.trim(),
    telefone: document.getElementById("reg-phone").value.trim(),
    email:    document.getElementById("reg-email").value.trim(),
    senha:    document.getElementById("reg-pass").value,
  };

  let resp, data;
  try {
    resp = await fetch("/Account/Registrar", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(payload),
    });
    data = await resp.json();
  } catch (e) {
    alert("Erro de conexão. Verifique sua internet e tente novamente.");
    return;
  }

  if (!data.ok) {
    alert(data.erro ?? "Erro ao cadastrar. Tente novamente.");
    return;
  }

  // Cadastro OK — redireciona para o login
  window.location.href = "/Account/Login";
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
  document.getElementById("strength-fill").style.width      = l ? l.w  : "0%";
  document.getElementById("strength-fill").style.background = l ? l.bg : "";
  document.getElementById("strength-label").textContent     = l ? l.label : "Digite uma senha";
}
