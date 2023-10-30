function setup()
{
	attachEventToQuantitySelects();
}

function attachEventToQuantitySelects()
{
	const quantityInputs = document.getElementsByName('quantitySelect');
	for (const quantityInput of quantityInputs)
    {
        const id = quantityInput.parentNode.parentNode.attributes['data-order-item-id'].value;
		quantityInput.addEventListener('change', (event) =>
		{
            $.ajax({
                type: "POST",
                url: "/cart?handler=UpdateItem",
                data: {
                    orderItemId: id,
                    quantity: event.target.value
                },
                headers: {
                    RequestVerificationToken:
                        $('input:hidden[name="__RequestVerificationToken"]').val()
                },
                complete: (response) => {
                    if(response.status == 200)
                        $('#totalPrice').load('/cart?handler=PricePartial');
                }
            });
		});

	}
}

async function checkout()
{
	document.getElementById('checkoutForm').submit();
}

setup();
