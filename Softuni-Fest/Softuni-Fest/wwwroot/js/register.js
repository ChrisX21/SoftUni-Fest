document.getElementById('userRole').addEventListener('click', () =>
{
	const selectValue = document.getElementById('userRole').value;
	const clientFields = document.getElementById("clientFields");
	const vendorFields = document.getElementById("vendorFields");

	if (selectValue == 'Client') {
		clientFields.style.display = 'block';
		vendorFields.style.display = 'none';
	}
	else
	{
		clientFields.style.display = 'none';
		vendorFields.style.display = 'block';
	}
});
