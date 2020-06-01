const button = document.querySelector(".delete-btn");
const confirmButton = document.querySelector(".delete-confirm");

button.addEventListener("click",deleteBtn);

function deleteBtn() {
	alert("Do you want to delete your account permanently?");
	button.style.display = "none";
	confirmButton.style.display = "block";
}