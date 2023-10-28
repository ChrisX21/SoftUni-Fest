let valid = true;
function attachOnChangeEventToAllQuantityInputs()
{
	const quantityInputs = document.querySelectorAll('.form-control');
	const span = document.createElement('span');
	span.className = 'text-danger';
	span.textContent = 'Quantity should be between 1 and 20';

	for (const quantityInput of quantityInputs)
	{
		quantityInput.addEventListener('change', (event) =>
		{
			const value = quantityInput.value;
			if (!(value >= 1 && value <= 20)) {
				quantityInput.parentNode.appendChild(span);
				valid = false;
			}
			else {
				valid = true;
			}
		});
		
	}
}

function checkout()
{
	if (valid)
		document.getElementById('checkoutForm').submit();
}

attachOnChangeEventToAllQuantityInputs();
