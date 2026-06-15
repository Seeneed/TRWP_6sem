$methods = @("GET", "POST", "PUT", "DELETE")
$results = @{ "X" = 0; "Y" = 0; "Z" = 0 }

Write-Host "We are executing requests to the Custom Balancer (waiting...)"

foreach ($method in $methods) {	
    for ($i = 1; $i -le 5; $i++) {
        $response = Invoke-RestMethod -Uri "http://localhost:5000/lb" -Method $method -ErrorAction SilentlyContinue
        if ($response.Nick) {
            $results[$response.Nick]++
        }
    }
}

Write-Host "`n=== Results Custom Balancer ==="
Write-Host "Processed by server X: $($results['X'])"
Write-Host "Processed by server Y: $($results['Y'])"
Write-Host "Processed by server Z: $($results['Z'])"
Read-Host "Tab Enter for exit..."