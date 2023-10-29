const searchInput = document.getElementById('search-bar');
console.log(searchInput);

const vendors = document.querySelectorAll('.vendor-name');

function calculateDistance(str1, str2)
{
	const str1Len = str1.length;
	const str2Len = str2.length;

	if (str1Len == 0)
		return str2Len;

	if (str2Len == 0)
		return str1Len;

	const matrix = new Array(str1Len + 1);
	for (var i = 0; i < str1; i++) {
		matrix[i] = new Array(str2Len + 1);
	}

	for (var i = 0; i <= str1Len; i++)
	{
		matrix[i, 0] = i;
	}
	for (var j = 1; j <= str2Len; j++)
	{
		matrix[0, j] = j;
	}

	for (var i = 1; i <= str1Len; i++) {
		for (var j = 1; j <= str2Len; j++) {
			//if (str1[i - 1] == str2[j - 1]) {
			//	matrix[i, j] = matrix[i - 1, j - 1];
			//}
			//else
			//{
			//	const minimum = Number.MAX_VALUE;
			//	if (matrix[i - 1, j] + 1 < minimum)
			//		minimum = matrix[i - 1, j] + 1;

			//	if (matrix[i, j - 1] + 1 < minimum)
			//		minimum = matrix[i, j - 1] + 1;

			//	if (matrix[i - 1, j - 1] + 1 < minimum)
			//		minimum = matrix[]
			//}
			var cost = (str2[j - 1] == str1[i - 1]) ? 0 : 1;

			matrix[i, j] = Math.min(
				Math.min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
				matrix[i - 1, j - 1] + cost);
		}
	}

	return matrix[str1Len, str2Len];
}

searchInput.addEventListener('input', e =>
{
	const value = e.target.value;
	let userSimilarity = new Map();

	for (const vendor of vendors)
	{
		//const isvisible = vendor.textContent
		//const similarity = calculateDistance(value, vendor.textContent);
		//userSimilarity.set(vendor.textContent, similarity);
		//console.log(vendor.textContent);
	}

	//let sorted = Array.from(userSimilarity).sort((a, b) => a[1] - b[1]);
	////const maxVendorCount = 3;
	////for (let i = 0; i < maxVendorCount; i++)
	////	sorted.shift();

	//console.log(sorted);
});
