## Jak pouzit (Windows 10/11) PowerShell
1. Invoke-RestMethod -Uri http://localhost:5000/chain -Method Get
2. $body = @{ sender="odesilatel"; recipient="prijemce"; amount=50 } | ConvertTo-Json
   Invoke-RestMethod -Uri http://localhost:5000/transactions/new -Method Post -Body $body -ContentType "application/json"
3. Invoke-RestMethod -Uri http://localhost:5000/mine -Method Get
4. Invoke-RestMethod -Uri http://localhost:5000/chain -Method Get

### Bonus
Pro lepsi citelnos pisu "http://localhost:5000/chain" do browseru a zmacknu tlacitko pro stiling
