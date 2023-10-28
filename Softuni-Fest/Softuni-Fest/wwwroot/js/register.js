function setup() 
{
	const userRoleSelect = document.getElementById('userRole');
	const clientFields = document.getElementById("clientFields");
	const vendorFields = document.getElementById("vendorFields");

	function showCorrectFormFields(value)
	{
		if (value == 'Client') {
			clientFields.style.display = 'block';
			vendorFields.style.display = 'none';
		}
		else
		{
			clientFields.style.display = 'none';
			vendorFields.style.display = 'block';
		}
	}

	showCorrectFormFields(userRoleSelect.value);
	
	userRoleSelect.addEventListener('click', () =>
	{
		showCorrectFormFields(userRoleSelect.value);
	});
}

setup();
