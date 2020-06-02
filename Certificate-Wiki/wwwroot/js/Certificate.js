const file = document.querySelector("#file");
const url = document.querySelector("#url");

const fileUpload = document.querySelector(".file-upload");
const urlUpload = document.querySelector(".url-upload");

file.addEventListener("change", CheckSelected);
url.addEventListener("change", CheckSelected);

function CheckSelected() {
	console.log("test");
	if (file.checked) {
		fileUpload.style.display = "block";
		urlUpload.style.display = "none";
	} else if (url.checked) {
		urlUpload.style.display = "block";
		fileUpload.style.display = "none";
	}
}