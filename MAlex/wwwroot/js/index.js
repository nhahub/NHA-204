submitBtn.addEventListener("click", () => {
  const email = document.getElementById("email").value.trim();
  const password = document.getElementById("password").value.trim();

  if (!email || !password) {
    errorMsg.textContent = "Please fill in all fields.";
    return;
  }

  const savedUser = JSON.parse(localStorage.getItem("user"));
  if (savedUser && savedUser.email === email && savedUser.password === password) {
    errorMsg.style.color = "#b2ffb2";
    errorMsg.textContent = "Login successful! Redirecting...";
    setTimeout(() => {
      window.location.href = "home.html";
    }, 1200);
  } else {
    errorMsg.textContent = "Invalid email or password.";
  }
});