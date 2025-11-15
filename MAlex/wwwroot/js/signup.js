const submitBtn = document.getElementById("submitBtn");
const errorMsg = document.getElementById("errorMsg");

submitBtn.addEventListener("click", () => {
  const firstName = document.getElementById("firstName").value.trim();
  const lastName = document.getElementById("lastName").value.trim();
  const email = document.getElementById("email").value.trim();
  const password = document.getElementById("password").value.trim();
  const phone = document.getElementById("phone").value.trim();
  const area = document.getElementById("area").value;

  // *Validation*
  if (!firstName || !lastName || !email || !password || !phone || !area) {
    errorMsg.style.color = "#f40303ff";
    errorMsg.textContent = "Please fill in all fields.";
    return;
  }


  const newUser = {
    firstName,
    lastName,
    email,
    password,
    phone,
    area
  };


  localStorage.setItem("user", JSON.stringify(newUser));


  errorMsg.style.color = "#b2ffb2";
  errorMsg.textContent = "Account created successfully! Redirecting...";

 
  setTimeout(() => {
    window.location.href = "index.html";
  }, 1200);
});
