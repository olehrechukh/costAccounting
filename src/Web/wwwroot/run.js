const apiUrl = './api/Shares';

async function getShares() {
  try {
    const response = await fetch(apiUrl);
    if (!response.ok) throw new Error("Failed to fetch shares.");

    const data = await response.json();

    let html = `
                    <table>
                        <tr>
                            <th>Quantity</th>
                            <th>Price Per Share</th>
                            <th>Investment Date</th>
                        </tr>
                `;

    data.forEach(shareLot => {
      html += `
                        <tr>
                            <td>${shareLot.quantity}</td>
                            <td>$${shareLot.pricePerShare.toFixed(2)}</td>
                            <td>${new Date(shareLot.investmentDate).toLocaleDateString()}</td>
                        </tr>
                    `;
    });

    html += `</table>`;

    document.getElementById("sharesOutput").innerHTML = html;
  } catch (error) {
    document.getElementById("sharesOutput").innerHTML = `<span class="error">Error: ${error.message}</span>`;
  }
}

async function calculateRemainingShares() {
  const sharesToSell = document.getElementById("sharesToSell").value;
  const salePrice = document.getElementById("salePrice").value;
  const costStrategy = document.getElementById("costStrategy").value;

  if (sharesToSell <= 0 || salePrice <= 0) {
    document.getElementById("calculationOutput").innerHTML = `<span class="error">Shares to sell and sale price must be greater than 0.</span>`;
    return;
  }



  if (sharesToSell >= 2147483647 || salePrice >= 2147483647) {
    document.getElementById("calculationOutput").innerHTML = `<span class="error">Shares to sell and sale price must be less than 2147483647.</span>`;
    return;
  }
  
  try {
    const response = await fetch(`${apiUrl}/costDetails?sharesToSell=${sharesToSell}&salePrice=${salePrice}&costStrategy=${costStrategy}`, {
      method: 'GET'
    });

    if (!response.ok) {
      const error = await response.json();
      document.getElementById("calculationOutput").innerHTML = `<span class="error">${error.detail}</span>`;
      return;
    }

    const result = await response.json();
    document.getElementById("calculationOutput").innerHTML = `
    <strong>Calculation Results:</strong>
    <table>
        <tr>
            <th>Metric</th>
            <th>Value</th>
        </tr>
        <tr>
            <td>Remaining Shares</td>
            <td><strong>${result.remainingShares}</strong></td>
        </tr>
        <tr>
            <td>Cost Basis of Sold Shares</td>
            <td><strong>${result.costBasisSold}</strong></td>
        </tr>
        <tr>
            <td>Cost Basis of Remaining Shares</td>
            <td><strong>${result.costBasisRemaining}</strong></td>
        </tr>
        <tr>
            <td>Profit or Loss</td>
            <td><strong>${result.profitOrLoss}</strong></td>
        </tr>
    </table>
`;
  } catch (error) {
    document.getElementById("calculationOutput").innerHTML = `<span class="error">Error: ${error.message}</span>`;
  }
}

window.onload = getShares;
