
  const profileLink = document.querySelector('.nav-item:first-child .nav-link');
  const ticketsLink = document.querySelector('.nav-item:nth-child(2) .nav-link');

  const profileSection = document.querySelector('.inner');        // قسم الـ Profile info
  const ticketsSection = document.querySelector('.inner_ticket'); // قسم التذاكر

  if (!profileLink || !ticketsLink || !profileSection || !ticketsSection) {
    console.warn('Profile/tickets elements not found — check selectors');
  } else {

    function showTickets(e) {
      e.preventDefault();
      profileSection.classList.add('d-none');
      ticketsSection.classList.remove('d-none');

      profileLink.classList.remove('active');
      ticketsLink.classList.add('active');
    }

    function showProfile(e) {
      e.preventDefault();
      ticketsSection.classList.add('d-none');
      profileSection.classList.remove('d-none');

      profileLink.classList.add('active');
      ticketsLink.classList.remove('active');
    }

    // Events
    ticketsLink.addEventListener('click', showTickets);
    profileLink.addEventListener('click', showProfile);
  }

// ^Edit section
var editBtn = document.querySelector(".edit_btn a");
var btnn = document.querySelector(".edit-layer");
var formBox = document.querySelector(".edit_form");
var close = document.querySelector(".close_btn");

editBtn.addEventListener("click", function (e) {
  e.preventDefault();
  btnn.classList.remove("d-none");
});

close.addEventListener("click", function (e) {
  e.preventDefault();
  btnn.classList.add("d-none");
});

btnn.addEventListener("click", function (e) {
  if (!formBox.contains(e.target)) {
    btnn.classList.add("d-none");
  }
});

document.addEventListener("keydown", function (e) {
  if (e.key === "Escape") {
    btnn.classList.add("d-none");
  }
});








(function () {
  'use strict';

  var forms = document.querySelectorAll('.needs-validation');

 
  var patterns = {
    firstname: /^[a-zA-Z_-]{3,15}$/,
    lastname: /^[a-zA-Z_-]{3,15}$/,
    email: /^[^@ \t\r\n]+@[^@ \t\r\n]+\.[^@ \t\r\n]+$/,
    phone: /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/,
    region: /^[a-zA-Z_-]{3,15}$/
  };

 
  var messages = {
    firstname: "Name must be 3–15 letters only (no numbers or symbols).",
    lastname: "Name must be 3–15 letters only (no numbers or symbols).",
    email: "Please enter a valid email address (e.g. name@example.com).",
    phone: "Please enter a valid phone number (e.g. +123-456-7890).",
    region: "Region name must contain only letters (3–15 characters)."
  };

  Array.prototype.forEach.call(forms, function (form) {
    form.addEventListener('submit', function (event) {
      var valid = true;

      var inputs = form.querySelectorAll('input');
      Array.prototype.forEach.call(inputs, function (input) {
        var pattern = patterns[input.name];
        var errorMessage = messages[input.name];
        var feedback = input.nextElementSibling;

      
        if (pattern && !pattern.test(input.value.trim())) {
          valid = false;
          input.classList.remove('is-valid');
          input.classList.add('is-invalid'); 
          if (feedback) feedback.textContent = errorMessage;
        } else if (pattern) {
          input.classList.remove('is-invalid');
          input.classList.add('is-valid');
        }
      });

    
      if (!valid || !form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
      }

      form.classList.add('was-validated');
    }, false);
  });
})();
