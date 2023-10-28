document.getElementById('userRole').addEventListener('click', () =>
{
	const selectValue = document.getElementById('userRole').value;
	const clientRegisterForm = document.getElementById("registerFormClient");
	const vendorRegisterForm = document.getElementById("registerFormVendor");

	if (selectValue == 'Client') {
		clientRegisterForm.style.display = 'block';
		vendorRegisterForm.style.display = 'none';
	}
	else
	{
		clientRegisterForm.style.display = 'none';
		vendorRegisterForm.style.display = 'block';
	}
});
