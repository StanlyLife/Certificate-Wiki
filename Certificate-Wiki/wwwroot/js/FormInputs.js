const inputs = document.querySelectorAll(".text-input");

inputs.forEach((input) => {
  input.addEventListener("focus", AddBorder);
  input.addEventListener("focusout", RemoveBorder);
});

function AddBorder(e) {
  e.target.classList.add("input-border");
}

function RemoveBorder(e) {
  e.target.classList.remove("input-border");
}

console.log("loaded forminput.js");
